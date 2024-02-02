using System.Collections;
using System.Linq;
using UnityEngine;

namespace PMFramework
{
    public class SingletonManager : MonoBehaviour
    {
        private ISingleton[] m_singletons;
        public static SingletonManager m_Isntance;
        public bool IsAllSetup { get; private set; }
        private Coroutine m_currentCoroutine;

        private void Awake()
        {
            m_singletons = 
                FindObjectsByType<Singleton>(FindObjectsInactive.Include, 
                FindObjectsSortMode.None);

            m_singletons = m_singletons.Distinct().OrderBy(t =>
            t.ExecutionOrder).ToArray();

            // Initialize SM
            if(m_Isntance != null && m_Isntance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                m_Isntance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        private void Start()
        {
            // Start initializing singletons on start
            // some components will act strangey if initialized/accessed in Awake
            // e.g. AudioMixerGroup etc
            m_currentCoroutine = StartCoroutine(InitializeSingletons());
        }
        IEnumerator InitializeSingletons()
        {
            for (int i = 0; i < m_singletons.Length; i++) 
            {
                var crt = m_singletons[i];
                Debug.Log($"Start Initializing {crt.SingletonName}");
                yield return crt?.Initialize();
                if (!crt.DestroyOnLoad)
                {
                    Debug.Log($"Attach {crt.SingletonName} to manager to prevent from destroying");
                    crt.AttachToManager(transform);
                }
                Debug.Log($"{crt.SingletonName} is Ready!");
            }

            // kill the coroutine
            if(m_currentCoroutine != null)
            {
                StopCoroutine(m_currentCoroutine);
                m_currentCoroutine = null;
            }
            IsAllSetup = true;
        }
    }
}
