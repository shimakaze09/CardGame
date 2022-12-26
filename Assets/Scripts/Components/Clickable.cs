using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public const string ClickedNotification = "Clickable.ClickedNotification";


    public void OnPointerClick(PointerEventData eventData)
    {
        this.PostNotification(ClickedNotification, eventData);
    }
}