using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class AttackSystem : Aspect, IObserve
{
    public const string FilterAttackersNotification = "AttackSystem.ValidateAttackerNotification";
    public const string FilterTargetsNotification = "AttackSystem.ValidateTargetNotification";
    public List<Card> validAttackers { get; private set; }
    public List<Card> validTargets { get; private set; }

    public void Awake()
    {
        this.AddObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.AddObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.RemoveObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    public List<Card> GetActive(Player player)
    {
        var list = new List<Card> { player[Zones.Hero][0] };
        list.AddRange(player[Zones.Battlefield]);
        return list;
    }

    private List<Card> GetFiltered(Player player, string filterNotificationName)
    {
        var list = GetActive(player);
        container.PostNotification(filterNotificationName, list);
        return list;
    }

    public void Refresh()
    {
        var match = container.GetMatch();
        validAttackers = GetFiltered(match.CurrentPlayer, FilterAttackersNotification);
        validTargets = GetFiltered(match.OpponentPlayer, FilterTargetsNotification);
    }

    private void OnValidateAttackAction(object sender, object args)
    {
        var action = sender as AttackAction;
        if (!validAttackers.Contains(action.attacker) || !validTargets.Contains(action.target))
        {
            var validator = args as Validator;
            validator.Invalidate();
        }
    }

    private void OnPerformAttackAction(object sender, object args)
    {
        var action = sender as AttackAction;
        var attacker = action.attacker as ICombatant;
        attacker.remainingAttacks--;

        var target = action.target as IDestructable;
        var damageAction = new DamageAction(target, attacker.attack);
        container.AddReaction(damageAction);
    }
}