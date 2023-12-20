using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class DeathSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnDeathReaperNotification, ActionSystem.deathReaperNotification);
        this.AddObserver(OnPerformDeath, Global.PerformNotification<DeathAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnDeathReaperNotification, ActionSystem.deathReaperNotification);
        this.RemoveObserver(OnPerformDeath, Global.PerformNotification<DeathAction>(), container);
    }

    private void OnDeathReaperNotification(object sender, object args)
    {
        var match = container.GetMatch();
        foreach (var player in match.players)
        foreach (var card in player[Zones.Battlefield])
            if (ShouldReap(card))
                TriggerReap(card);
    }

    private void OnPerformDeath(object sender, object args)
    {
        var action = args as DeathAction;
        var cardSystem = container.GetAspect<CardSystem>();
        cardSystem.ChangeZone(action.card, Zones.Graveyard);
    }

    private bool ShouldReap(Card card)
    {
        var target = card as IDestructable;
        return target != null && target.hitPoints <= 0;
    }

    private void TriggerReap(Card card)
    {
        var action = new DeathAction(card);
        container.AddReaction(action);
    }
}