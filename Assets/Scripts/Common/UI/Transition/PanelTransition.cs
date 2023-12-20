﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TheLiquidFire.Animation;

namespace TheLiquidFire.UI
{
    public class PanelTransition : BaseTransition
    {
        [Serializable]
        public class Data
        {
            public string position;
            public float duration;
            public Func<float, float, float, float> equation;
        }

        public Panel panel;

        public Data showData = new()
        {
            position = "Show",
            duration = 0.5f,
            equation = EasingEquations.EaseOutBack
        };

        public Data hideData = new()
        {
            position = "Hide",
            duration = 0.5f,
            equation = EasingEquations.EaseInBack
        };

        public override void Show(Action didShow)
        {
            panel.SetPosition(hideData.position, false);
            StartCoroutine(Process(showData, didShow));
        }

        public override void Hide(Action didHide)
        {
            panel.SetPosition(showData.position, false);
            StartCoroutine(Process(hideData, didHide));
        }

        private IEnumerator Process(Data data, Action complete)
        {
            var tweener = panel.SetPosition(data.position, true);
            tweener.duration = data.duration;
            tweener.equation = data.equation;
            while (tweener != null)
                yield return null;
            if (complete != null)
                complete();
        }
    }
}