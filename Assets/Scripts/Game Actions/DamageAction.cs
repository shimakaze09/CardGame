using System.Collections.Generic;

public class DamageAction : GameAction
{
    public int amount;
    public List<IDestructable> targets;

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
}