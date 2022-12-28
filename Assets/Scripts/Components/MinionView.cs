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

    public void Display(Minion minion)
    {
        this.minion = minion;
        Refresh();
    }

    private void Refresh()
    {
        if (minion == null)
            return;
        avatar.sprite = inactive;
        attack.text = minion.attack.ToString();
        health.text = minion.hitPoints.ToString();
    }
}