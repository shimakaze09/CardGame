using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.Animation;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;
using TheLiquidFire.Notifications;
using TheLiquidFire.Pooling;
using UnityEngine;

public class TableView : MonoBehaviour
{
    public List<MinionView> minions = new();
    private SetPooler minionPooler;

    private void Awake()
    {
        var board = GetComponentInParent<BoardView>();
        minionPooler = board.minionPooler;
    }

    private void OnEnable()
    {
        this.AddObserver(OnPrepareSummon, Global.PrepareNotification<SummonMinionAction>());
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPrepareSummon, Global.PrepareNotification<SummonMinionAction>());
    }

    private void OnPrepareSummon(object sender, object args)
    {
        var action = args as SummonMinionAction;
        if (GetComponentInParent<PlayerView>().player.index == action.minion.ownerIndex)
            action.perform.viewer = SummonMinion;
    }

    public IEnumerator SummonMinion(IContainer game, GameAction action)
    {
        var summon = action as SummonMinionAction;
        var playerView = GetComponentInParent<PlayerView>();
        var cardView = playerView.hand.GetView(summon.minion);
        playerView.hand.Dismiss(cardView);
        StartCoroutine(playerView.hand.LayoutCards(true));

        var minionView = minionPooler.Dequeue().GetComponent<MinionView>();
        minionView.transform.ResetParent(transform);
        minions.Add(minionView);
        minionView.gameObject.SetActive(true);

        minionView.Display(summon.minion);
        var pos = GetComponentInParent<PlayerView>().hand.activeHandle.position;
        minionView.transform.position = pos;

        var tweener = LayoutMinions();
        tweener.duration = 0.5f;
        tweener.equation = EasingEquations.EaseOutBounce;
        while (tweener != null) yield return null;
    }

    private Tweener LayoutMinions()
    {
        var xPos = minions.Count / -2f + 0.5f;
        Tweener tweener = null;
        foreach (var minion in minions)
        {
            tweener = minion.transform.MoveToLocal(new Vector3(xPos, 0, 0), 0.25f);
            xPos += 1;
        }

        return tweener;
    }
}