using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class MinionView : MonoBehaviour
{
    public Sprite active;
    public Sprite activeTaunt;
    public Text attack;
    public Image avatar;
    public Text health;
    public Sprite inactive;
    public Sprite inactiveTaunt;

    public Minion minion { get; private set; }
    private bool isActive;

    private void OnEnable()
    {
        this.AddObserver(OnAttackSystemUpdate, AttackSystem.DidUpdateNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnAttackSystemUpdate, AttackSystem.DidUpdateNotification);
    }

    public void Display(Minion minion)
    {
        this.minion = minion;
        Refresh();
    }

    private void Refresh()
    {
        if (minion == null)
            return;
        avatar.sprite = isActive ? active : inactive;
        attack.text = minion.attack.ToString();
        health.text = minion.hitPoints.ToString();
    }

    private void OnAttackSystemUpdate(object sender, object args)
    {
        var container = sender as Container;
        isActive = container.GetAspect<AttackSystem>().validAttackers.Contains(minion);
        Refresh();
    }
}