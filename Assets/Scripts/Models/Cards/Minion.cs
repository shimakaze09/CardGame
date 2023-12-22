﻿using System.Collections.Generic;

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
}