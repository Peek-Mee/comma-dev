using System.Collections;
using UnityEngine;

namespace PMFramework
{
    public interface ISingleton
    {
        public int ExecutionOrder { get; }
        public bool IsRunning { get; }
        public System.Type SingletonName { get; } 
        public IEnumerator Initialize();
        public void Shutdown();
        public void AttachToManager(Transform manager);
        public bool DestroyOnLoad { get; }
    }
}

