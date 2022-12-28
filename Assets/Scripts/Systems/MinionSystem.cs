using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class MinionSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnPreparePlayCard, Global.PrepareNotification<PlayCardAction>(), container);
        this.AddObserver(OnPerformSummonMinion, Global.PerformNotification<SummonMinionAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPreparePlayCard, Global.PrepareNotification<PlayCardAction>(), container);
        this.RemoveObserver(OnPerformSummonMinion, Global.PerformNotification<SummonMinionAction>(), container);
    }

    private void OnPreparePlayCard(object sender, object args)
    {
        var action = args as PlayCardAction;
        var minion = action.card as Minion;
        if (minion != null)
        {
            var summon = new SummonMinionAction(minion);
            container.AddReaction(summon);
        }
    }

    private void OnPerformSummonMinion(object sender, object args)
    {
        var cardSystem = container.GetAspect<CardSystem>();
        var summon = args as SummonMinionAction;
        cardSystem.ChangeZone(summon.minion, Zones.Battlefield);
    }
}