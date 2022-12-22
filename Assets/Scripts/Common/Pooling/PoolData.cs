using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.Pooling
{
    public class PoolData
    {
        public int maxCount;
        public Queue<Poolable> pool;
        public GameObject prefab;
    }
}