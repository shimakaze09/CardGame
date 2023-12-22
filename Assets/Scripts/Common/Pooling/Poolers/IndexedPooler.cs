using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.Pooling
{
    public class IndexedPooler : BasePooler
    {
        #region Fields / Properties

        public List<Poolable> Collection = new();

        #endregion

        #region Events

        public Action<Poolable, int> willEnqueueAtIndex;
        public Action<Poolable, int> didDequeueAtIndex;

        #endregion

        #region Public

        public Poolable GetItem(int index)
        {
            if (index < 0 || index >= Collection.Count)
                return null;
            return Collection[index];
        }

        public U GetScript<U>(int index) where U : MonoBehaviour
        {
            var item = GetItem(index);
            if (item != null)
                return item.GetComponent<U>();
            return null;
        }

        public void EnqueueByIndex(int index)
        {
            if (index < 0 || index >= Collection.Count)
                return;
            Enqueue(Collection[index]);
        }

        public override void Enqueue(Poolable item)
        {
            base.Enqueue(item);
            var index = Collection.IndexOf(item);
            if (index != -1)
            {
                if (willEnqueueAtIndex != null)
                    willEnqueueAtIndex(item, index);
                Collection.RemoveAt(index);
            }
        }

        public override Poolable Dequeue()
        {
            var item = base.Dequeue();
            Collection.Add(item);
            if (didDequeueAtIndex != null)
                didDequeueAtIndex(item, Collection.Count - 1);
            return item;
        }

        public override void EnqueueAll()
        {
            for (var i = Collection.Count - 1; i >= 0; --i)
                Enqueue(Collection[i]);
        }

        #endregion
    }
}