using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class DestructableSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), container);
    }

    private void OnPerformDamageAction(object sender, object args)
    {
        var action = args as DamageAction;
        foreach (var target in action.targets) target.hitPoints -= action.amount;
    }
}