public class AttackAction : GameAction
{
    public Card attacker;
    public Card target;

    public AttackAction(Card attacker, Card target)
    {
        this.attacker = attacker;
        this.target = target;
    }
}