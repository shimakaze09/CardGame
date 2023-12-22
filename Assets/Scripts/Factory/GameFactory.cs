using TheLiquidFire.AspectContainer;

public static class GameFactory
{
    public static Container Create()
    {
        var game = new Container();

        // Add Systems
        game.AddAspect<ActionSystem>();
        game.AddAspect<AttackSystem>();
        game.AddAspect<CardSystem>();
        game.AddAspect<CombatantSystem>();
        game.AddAspect<DataSystem>();
        game.AddAspect<DeathSystem>();
        game.AddAspect<DestructableSystem>();
        game.AddAspect<EnemySystem>();
        game.AddAspect<ManaSystem>();
        game.AddAspect<MatchSystem>();
        game.AddAspect<MinionSystem>();
        game.AddAspect<PlayerSystem>();
        game.AddAspect<TargetSystem>();
        game.AddAspect<TauntSystem>();
        game.AddAspect<VictorySystem>();

        // Add Other
        game.AddAspect<StateMachine>();
        game.AddAspect<GlobalGameState>();

        return game;
    }
}