using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Animation;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using TMPro;
using UnityEngine;

public class FatigueView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fatigueLabel;

    private void OnEnable()
    {
        this.AddObserver(OnPrepareFatigue, Global.PrepareNotification<FatigueAction>());
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPrepareFatigue, Global.PrepareNotification<FatigueAction>());
    }

    private void OnPrepareFatigue(object sender, object args)
    {
        var action = args as FatigueAction;
        action.perform.viewer = FatigueViewer;
    }

    private IEnumerator FatigueViewer(IContainer game, GameAction action)
    {
        yield return true;
        var fatigue = action as FatigueAction;

        fatigueLabel.text = $"Fatigue\n{fatigue.player.fatigue}";

        var tweener = transform.ScaleTo(Vector3.one, 0.5f, EasingEquations.EaseOutBack);
        while (tweener != null) yield return null;

        tweener = transform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);
        while (tweener != null) yield return null;
    }
}