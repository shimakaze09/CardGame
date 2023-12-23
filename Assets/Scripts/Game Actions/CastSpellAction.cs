public class CastSpellAction : GameAction
{
    public Spell spell;

    public CastSpellAction(Spell spell)
    {
        this.spell = spell;
    }
}