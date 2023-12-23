using System;
using System.Collections.Generic;

public class Minion : Card, ICombatant, IDestructable
{
    // Other
    public List<string> mechanics;

    public string race;

    // ICombatant
    public int attack { get; set; }
    public int remainingAttacks { get; set; }
    public int allowedAttacks { get; set; }

    // IDestructable
    public int hitPoints { get; set; }
    public int maxHitPoints { get; set; }

    public override void Load(Dictionary<string, object> data)
    {
        base.Load(data);
        attack = Convert.ToInt32(data["attack"]);
        hitPoints = maxHitPoints = Convert.ToInt32(data["hit points"]);
        allowedAttacks = 1;
    }
}