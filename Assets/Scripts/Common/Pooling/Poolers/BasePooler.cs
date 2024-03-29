﻿using System;
using UnityEngine;

namespace TheLiquidFire.Pooling
{
    public abstract class BasePooler : MonoBehaviour
    {
        #region Events

        public Action<Poolable> willEnqueue;
        public Action<Poolable> didDequeue;

        #endregion

        #region Fields / Properties

        public string key = string.Empty;
        public GameObject prefab;
        public int prepopulate;
        public int maxCount = int.MaxValue;
        public bool autoRegister = true;
        public bool autoClear = true;
        public bool isRegistered { get; private set; }

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            if (autoRegister)
                Register();
        }

        protected virtual void OnDestroy()
        {
            EnqueueAll();
            if (autoClear)
                UnRegister();
        }

        protected virtual void OnApplicationQuit()
        {
            EnqueueAll();
        }

        #endregion

        #region Public

        public void Register()
        {
            if (string.IsNullOrEmpty(key))
                key = prefab.name;
            GameObjectPoolController.AddEntry(key, prefab, prepopulate, maxCount);
            isRegistered = true;
        }

        public void UnRegister()
        {
            GameObjectPoolController.ClearEntry(key);
            isRegistered = false;
        }

        public virtual void Enqueue(Poolable item)
        {
            if (willEnqueue != null)
                willEnqueue(item);
            GameObjectPoolController.Enqueue(item);
        }

        public virtual void EnqueueObject(GameObject obj)
        {
            var item = obj.GetComponent<Poolable>();
            if (item != null)
                Enqueue(item);
        }

        public virtual void EnqueueScript(MonoBehaviour script)
        {
            var item = script.GetComponent<Poolable>();
            if (item != null)
                Enqueue(item);
        }

        public virtual Poolable Dequeue()
        {
            var item = GameObjectPoolController.Dequeue(key);
            if (didDequeue != null)
                didDequeue(item);
            return item;
        }

        public virtual U DequeueScript<U>() where U : MonoBehaviour
        {
            var item = Dequeue();
            return item.GetComponent<U>();
        }

        public abstract void EnqueueAll();

        #endregion
    }
}