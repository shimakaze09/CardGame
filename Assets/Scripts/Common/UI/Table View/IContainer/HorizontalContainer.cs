using UnityEngine.UI;

namespace TheLiquidFire.UI
{
    public class HorizontalContainer : IContainer
    {
        private readonly ScrollRect ScrollRect;
        private readonly ISpacer Spacer;

        public HorizontalContainer(ScrollRect scrollRect, ISpacer spacer)
        {
            ScrollRect = scrollRect;
            Spacer = spacer;

            if (ScrollRect.content.anchorMax.x < 0.5f)
                Flow = new LeftToRight(scrollRect, spacer);
            else
                Flow = new RightToLeft(scrollRect, spacer);
        }

        public IFlow Flow { get; }

        public void AutoSize()
        {
            var contentSize = ScrollRect.content.sizeDelta;
            contentSize.x = Spacer.TotalSize;
            ScrollRect.content.sizeDelta = contentSize;
        }
    }
}