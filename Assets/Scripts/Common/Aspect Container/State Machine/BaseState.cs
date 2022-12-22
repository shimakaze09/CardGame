namespace TheLiquidFire.AspectContainer
{
    public interface IState : IAspect
    {
        string identifier { get; }
        void Enter();
        bool CanTransition(IState other);
        void Exit();
    }

    public abstract class BaseState : Aspect, IState
    {
        public BaseState()
        {
            identifier = GetType().Name;
        }

        public string identifier { get; }

        public virtual void Enter()
        {
        }

        public virtual bool CanTransition(IState other)
        {
            return true;
        }

        public virtual void Exit()
        {
        }

        public virtual void Transition<T>() where T : class, IState, new()
        {
            var stateMachine = container.GetAspect<StateMachine>();
            stateMachine.ChangeState<T>();
        }
    }
}