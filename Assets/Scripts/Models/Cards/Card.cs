using System;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;

public class Card : Container
{
    public int cost;
    public string id;
    public string name;
    public int orderOfPlay = int.MaxValue;
    public int ownerIndex;
    public string text;
    public Zones zone = Zones.Deck;

    public virtual void Load(Dictionary<string, object> data)
    {
        id = (string)data["id"];
        name = (string)data["name"];
        text = (string)data["text"];
        cost = Convert.ToInt32(data["cost"]);
    }
}