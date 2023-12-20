using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class DestructableSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), container);
        this.AddObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), container);
        this.RemoveObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    private void OnPerformDamageAction(object sender, object args)
    {
        var action = args as DamageAction;
        foreach (var target in action.targets) target.hitPoints -= action.amount;
    }

    private void OnFilterAttackTargets(object sender, object args)
    {
        var candidates = args as List<Card>;
        for (var i = candidates.Count - 1; i >= 0; --i)
        {
            var destructable = candidates[i] as IDestructable;
            if (destructable == null)
                candidates.RemoveAt(i);
        }
    }
}