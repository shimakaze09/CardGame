using UnityEngine.UI;

public class HeroView : BattlefieldCardView
{
    public Text armor;
    public Hero hero { get; private set; }
    public override Card card => hero;

    public void SetHero(Hero hero)
    {
        this.hero = hero;
        Refresh();
    }

    protected override void Refresh()
    {
        if (hero == null)
            return;
        avatar.sprite = isActive ? active : inactive;
        attack.text = hero.attack.ToString();
        health.text = hero.hitPoints.ToString();
        armor.text = hero.armor.ToString();
    }
}