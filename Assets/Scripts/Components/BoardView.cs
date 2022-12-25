using System.Collections.Generic;
using TheLiquidFire.Pooling;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public GameObject damageMarkPrefab;
    public List<PlayerView> playerViews;
    public SetPooler cardPooler;
}