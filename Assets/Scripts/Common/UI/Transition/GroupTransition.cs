using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheLiquidFire.UI
{
    public class GroupTransition : BaseTransition
    {
        public List<BaseTransition> transitions;

        public override void Show(Action didShow = null)
        {
            var count = transitions.Count;
            for (var i = 0; i < transitions.Count; ++i)
                transitions[i].Show(delegate
                {
                    count--;
                    if (count == 0)
                        if (didShow != null)
                            didShow();
                });
        }

        public override void Hide(Action didHide = null)
        {
            var count = transitions.Count;
            for (var i = 0; i < transitions.Count; ++i)
                transitions[i].Hide(delegate
                {
                    count--;
                    if (count == 0)
                        if (didHide != null)
                            didHide();
                });
        }
    }
}