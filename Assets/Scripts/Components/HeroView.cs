using TheLiquidFire.Notifications;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour
{
    public Sprite active;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI attack;
    public Image avatar;
    public TextMeshProUGUI health;
    public Sprite inactive;
    public Hero hero { get; private set; }

    public void SetHero(Hero hero)
    {
        this.hero = hero;
        Refresh();
    }

    private void OnEnable()
    {
        this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
    }

    private void Refresh()
    {
        if (hero == null) return;

        avatar.sprite = inactive; // TODO: Add activation logic
        attack.text = hero.attack.ToString();
        health.text = hero.hitPoints.ToString();
        armor.text = hero.armor.ToString();
    }

    private void OnPerformDamageAction(object sender, object args)
    {
        var action = args as DamageAction;
        if (action.targets.Contains(hero)) Refresh();
    }
}