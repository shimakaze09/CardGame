using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class AttackSystem : Aspect
{
    public const string FilterAttackersNotification = "AttackSystem.ValidateAttackerNotification";
    public const string FilterTargetsNotification = "AttackSystem.ValidateTargetNotification";
    public const string DidUpdateNotification = "AttackSystem.DidUpdateNotification";
    public List<Card> validAttackers { get; private set; }
    public List<Card> validTargets { get; private set; }

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
        container.PostNotification(DidUpdateNotification);
    }

    public void Clear()
    {
        validAttackers.Clear();
        validTargets.Clear();
        container.PostNotification(DidUpdateNotification);
    }
}