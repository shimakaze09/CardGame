using System;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;

public class DamageAction : GameAction, IAbilityLoader
{
    public int amount;
    public List<IDestructable> targets;

    #region IAbility

    public void Load(IContainer game, Ability ability)
    {
        var targetSelector = ability.GetAspect<ITargetSelector>();
        var cards = targetSelector.SelectTargets(game);
        targets = new List<IDestructable>();
        foreach (var card in cards)
        {
            var destructable = card as IDestructable;
            if (destructable != null)
                targets.Add(destructable);
        }

        amount = Convert.ToInt32(ability.userInfo);
    }

    #endregion

    #region Constructors

    public DamageAction()
    {
    }

    public DamageAction(IDestructable target, int amount)
    {
        targets = new List<IDestructable>(1);
        targets.Add(target);
        this.amount = amount;
    }

    public DamageAction(List<IDestructable> targets, int amount)
    {
        this.targets = targets;
        this.amount = amount;
    }

    #endregion
}