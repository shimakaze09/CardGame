﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class VictorySystem : Aspect
{
    public bool IsGameOver()
    {
        var match = container.GetMatch();
        foreach (var p in match.players)
        {
            var h = p.hero[0] as Hero;
            if (h.hitPoints <= 0) return true;
        }

        return false;
    }
}