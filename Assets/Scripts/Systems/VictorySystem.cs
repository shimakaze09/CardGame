using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class VictorySystem : Aspect
{
    public bool IsGameOver()
    {
        var match = container.GetMatch();
        return match.players.Select(p => p.hero[0] as Hero).Any(h => h.hitPoints <= 0);
    }
}