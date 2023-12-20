using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TheLiquidFire.UI
{
    public class HorizontalContainer : IContainer
    {
        public IFlow Flow { get; private set; }
        private ScrollRect ScrollRect;
        private ISpacer Spacer;

        public HorizontalContainer(ScrollRect scrollRect, ISpacer spacer)
        {
            ScrollRect = scrollRect;
            Spacer = spacer;

            if (ScrollRect.content.anchorMax.x < 0.5f)
                Flow = new LeftToRight(scrollRect, spacer);
            else
                Flow = new RightToLeft(scrollRect, spacer);
        }

        public void AutoSize()
        {
            var contentSize = ScrollRect.content.sizeDelta;
            contentSize.x = Spacer.TotalSize;
            ScrollRect.content.sizeDelta = contentSize;
        }
    }
}