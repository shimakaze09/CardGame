using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public const string BeginDragNotification = "Dragable.BeginDragNotification";
    public const string EndDragNotification = "Dragable.EndDragNotification";

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Send a BeginDragNotification notification when the pointer down event occurs
        this.PostNotification(BeginDragNotification, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Send an EndDragNotification notification when the pointer up event occurs
        this.PostNotification(EndDragNotification, eventData);
    }
}