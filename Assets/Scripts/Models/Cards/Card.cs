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
}