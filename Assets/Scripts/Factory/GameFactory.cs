using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public static class GameFactory
{
    public static Container Create()
    {
        var game = new Container();

        // Add Systems
        game.AddAspect<ActionSystem>();
        game.AddAspect<DataSystem>();
        game.AddAspect<MatchSystem>();
        game.AddAspect<PlayerSystem>();

        // Add Other
        game.AddAspect<StateMachine>();
        game.AddAspect<GlobalGameState>();

        return game;
    }
}