using System.Collections.Generic;
using TheLiquidFire.AspectContainer;

public interface ITargetSelector : IAspect
{
    List<Card> SelectTargets(IContainer game);
}