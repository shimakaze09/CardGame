using UnityEngine.UI;

namespace TheLiquidFire.UI
{
    public class VerticalContainer : IContainer
    {
        private readonly ScrollRect ScrollRect;
        private readonly ISpacer Spacer;

        public VerticalContainer(ScrollRect scrollRect, ISpacer spacer)
        {
            ScrollRect = scrollRect;
            Spacer = spacer;

            if (ScrollRect.content.anchorMax.y > 0.5f)
                Flow = new TopToBottom(scrollRect, spacer);
            else
                Flow = new BottomToTop(scrollRect, spacer);
        }

        public IFlow Flow { get; }

        public void AutoSize()
        {
            var contentSize = ScrollRect.content.sizeDelta;
            contentSize.y = Spacer.TotalSize;
            ScrollRect.content.sizeDelta = contentSize;
        }
    }
}