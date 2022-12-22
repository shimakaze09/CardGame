namespace TheLiquidFire.AspectContainer
{
    public class StateMachine : Aspect
    {
        public delegate void StateChangeHandler(IState fromState, IState toState);

        public IState currentState { get; private set; }
        public event StateChangeHandler didChangeStateEvent;

        public void ChangeState<T>() where T : class, IState, new()
        {
            var previousState = currentState;
            var nextState = container.GetAspect<T>() ?? container.AddAspect<T>();

            if (previousState != null)
            {
                if (previousState == nextState || previousState.CanTransition(nextState) == false)
                    return;
                previousState.Exit();
            }

            currentState = nextState;

            var handler = didChangeStateEvent;
            if (handler != null)
                handler(previousState, nextState);

            nextState.Enter();
        }
    }
}