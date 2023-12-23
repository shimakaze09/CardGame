﻿using System.Collections.Generic;
using TheLiquidFire.AspectContainer;

public class AllTarget : Aspect, ITargetSelector
{
    public Mark mark;

    public List<Card> SelectTargets(IContainer game)
    {
        var result = new List<Card>();
        var system = game.GetAspect<TargetSystem>();
        var card = (container as Ability).card;
        var marks = system.GetMarks(card, mark);
        result.AddRange(marks);
        return result;
    }

    public void Load(Dictionary<string, object> data)
    {
        var markData = (Dictionary<string, object>)data["mark"];
        mark = new Mark(markData);
    }
}