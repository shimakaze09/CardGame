using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class DataSystem : Aspect
{
    public Match match = new();
}

public static class DataSystemExtensions
{
    public static Match GetMatch(this IContainer game)
    {
        var dataSystem = game.GetAspect<DataSystem>();
        return dataSystem.match;
    }
}