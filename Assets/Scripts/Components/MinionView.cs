using UnityEngine;

public class MinionView : BattlefieldCardView
{
    public Sprite inactiveTaunt;
    public Sprite activeTaunt;

    public Minion minion { get; private set; }
    public override Card card => minion;

    public void Display(Minion minion)
    {
        this.minion = minion;
        Refresh();
    }

    protected override void Refresh()
    {
        if (minion == null)
            return;
        avatar.sprite = isActive ? active : inactive;
        attack.text = minion.attack.ToString();
        health.text = minion.hitPoints.ToString();
    }
}