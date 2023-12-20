using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheLiquidFire.Notifications;

public class ManaView : MonoBehaviour
{
    public List<Image> slots;
    public Sprite available;
    public Sprite unavailable;
    public Sprite locked;
    public Sprite slot;

    private void OnEnable()
    {
        this.AddObserver(OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
    }

    private void OnManaValueChangedNotification(object sender, object args)
    {
        var mana = args as Mana;
        for (var i = 0; i < mana.Available; ++i) SetSpriteForImageSlot(available, i);
        for (var i = mana.Available; i < mana.Unlocked; ++i) SetSpriteForImageSlot(unavailable, i);
        for (var i = mana.Unlocked - mana.overloaded; i < mana.Unlocked; ++i) SetSpriteForImageSlot(locked, i);
        for (var i = mana.Unlocked; i < Mana.MaxSlots; ++i) SetSpriteForImageSlot(slot, i);
    }

    private void SetSpriteForImageSlot(Sprite sprite, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count)
            slots[slotIndex].sprite = sprite;
    }
}