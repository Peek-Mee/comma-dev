using System;
using System.Collections;
using UnityEngine;

namespace PMFramework
{
    public abstract class Singleton<T> : Singleton
        where T : Singleton<T>
    {
        public static T m_Instance;
        protected virtual void OnInitialize()
        {
            var temp = FindFirstObjectByType<T>() ?? 
                throw new Exception($"Can't find {typeof(T)} in the scene");
            m_Instance = temp;
            IsRunning = true;
        }
        public sealed override IEnumerator Initialize()
        {
            OnInitialize();
            yield return new WaitUntil(() => IsRunning);
        }
        public sealed override void AttachToManager(Transform manager)
        {
            transform.parent = manager;
        }
        public sealed override void Shutdown()
        {
            Destroy(gameObject);
        }
        public sealed override Type SingletonName => typeof(T);
    }

    public abstract class Singleton : MonoBehaviour, ISingleton
    {
        [SerializeField] private int m_executionOrder;
        [SerializeField] private bool m_destroyOnLoad = false;
        public int ExecutionOrder => m_executionOrder;
        public bool IsRunning { get; protected set; }

        public abstract IEnumerator Initialize();

        public abstract void Shutdown();
        public abstract void AttachToManager(Transform manager);
        public abstract Type SingletonName { get;}
        public virtual bool DestroyOnLoad => m_destroyOnLoad;
    }
}

