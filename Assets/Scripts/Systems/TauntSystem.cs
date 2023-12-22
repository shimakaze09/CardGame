using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class TauntSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
    }

    private void OnFilterAttackTargets(object sender, object args)
    {
        var candidates = args as List<Card>;
        if (TargetsContainTaunt(candidates) == false)
            return;

        for (var i = candidates.Count - 1; i >= 0; --i)
            if (candidates[i].GetAspect<Taunt>() == null)
                candidates.RemoveAt(i);
    }

    private bool TargetsContainTaunt(List<Card> cards)
    {
        foreach (var card in cards)
            if (card.GetAspect<Taunt>() != null)
                return true;
        return false;
    }
}