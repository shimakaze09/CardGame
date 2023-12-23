﻿using System;
using System.Collections.Generic;

public class Mark
{
    public Alliance alliance;
    public Zones zones;

    public Mark(Alliance alliance, Zones zones)
    {
        this.alliance = alliance;
        this.zones = zones;
    }

    public Mark(Dictionary<string, object> data)
    {
        alliance = (Alliance)Enum.Parse(typeof(Alliance), (string)data["alliance"]);
        zones = (Zones)Enum.Parse(typeof(Zones), (string)data["zone"]);
    }
}