using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class CardSystem : Aspect
{
    public void ChangeZone(Card card, Zones zone, Player toPlayer = null)
    {
        var fromPlayer = container.GetMatch().players[card.ownerIndex];
        toPlayer ??= fromPlayer;
        fromPlayer[card.zone].Remove(card);
        toPlayer[zone].Add(card);
        card.zone = zone;
        card.ownerIndex = toPlayer.index;
    }
}