﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class CardSystem : Aspect
{
    public List<Card> playable = new();

    public void Refresh()
    {
        var match = container.GetMatch();
        playable.Clear();
        foreach (var card in match.CurrentPlayer[Zones.Hand])
        {
            var playAction = new PlayCardAction(card);
            if (playAction.Validate())
                playable.Add(card);
        }
    }

    public void ChangeZone(Card card, Zones zone, Player toPlayer = null)
    {
        var fromPlayer = container.GetMatch().players[card.ownerIndex];
        toPlayer = toPlayer ?? fromPlayer;
        fromPlayer[card.zone].Remove(card);
        toPlayer[zone].Add(card);
        card.zone = zone;
        card.ownerIndex = toPlayer.index;
    }
}