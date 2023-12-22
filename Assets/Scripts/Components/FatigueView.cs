﻿using System.Collections;
using TheLiquidFire.Animation;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;
using UnityEngine.UI;

public class FatigueView : MonoBehaviour
{
    [SerializeField] private Text fatigueLabel;

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

        fatigueLabel.text = string.Format("Fatigue\n{0}", fatigue.player.fatigue);

        var tweener = transform.ScaleTo(Vector3.one, 0.5f, EasingEquations.EaseOutBack);
        while (tweener != null)
            yield return null;

        tweener = transform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);
        while (tweener != null)
            yield return null;
    }
}