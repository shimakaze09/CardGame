using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class CombatantSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
        this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
        this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
    }

    private void OnFilterAttackers(object sender, object args)
    {
        var candidates = args as List<Card>;
        for (var i = candidates.Count - 1; i >= 0; i--)
        {
            var combatant = candidates[i] as ICombatant;
            if (!CanAttack(combatant))
                candidates.RemoveAt(i);
        }
    }

    private bool CanAttack(ICombatant combatant)
    {
        return combatant is { attack: > 0, remainingAttacks: > 0 };
    }

    private void OnPerformChangeTurn(object sender, object args)
    {
        var action = args as ChangeTurnAction;
        var player = container.GetMatch().players[action.targetPlayerIndex];
        var active = container.GetAspect<AttackSystem>().GetActive(player);
        foreach (var combatant in active.OfType<ICombatant>()) combatant.remainingAttacks = combatant.allowedAttacks;
    }
}