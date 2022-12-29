using TheLiquidFire.AspectContainer;
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
    private bool isActive;

    public void SetHero(Hero hero)
    {
        this.hero = hero;
        Refresh();
    }

    private void OnEnable()
    {
        this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
        this.AddObserver(OnAttackSystemUpdate, AttackSystem.DidUpdateNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>());
        this.RemoveObserver(OnAttackSystemUpdate, AttackSystem.DidUpdateNotification);
    }

    private void Refresh()
    {
        if (hero == null) return;

        avatar.sprite = isActive ? active : inactive;
        attack.text = hero.attack.ToString();
        health.text = hero.hitPoints.ToString();
        armor.text = hero.armor.ToString();
    }

    private void OnPerformDamageAction(object sender, object args)
    {
        var action = args as DamageAction;
        if (action.targets.Contains(hero)) Refresh();
    }

    private void OnAttackSystemUpdate(object sender, object args)
    {
        var container = sender as Container;
        isActive = container.GetAspect<AttackSystem>().validAttackers.Contains(hero);
        Refresh();
    }
}