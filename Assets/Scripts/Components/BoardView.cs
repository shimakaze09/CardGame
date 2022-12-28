using System.Collections.Generic;
using TheLiquidFire.Pooling;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public GameObject damageMarkPrefab;
    public List<PlayerView> playerViews;
    public SetPooler cardPooler;
    public SetPooler minionPooler;

    private void Start()
    {
        var match = GetComponentInParent<GameViewSystem>().container.GetMatch();
        for (var i = 0; i < match.players.Count; i++) playerViews[i].SetPlayer(match.players[i]);
    }
}