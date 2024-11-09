using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace PMFramework
{
    public class SceneManager : Singleton<SceneManager>
    {
        public bool IsLoadingProcess { get; private set; }
        public UnityEvent<SceneTransition> OnWillLoadScene = new();
        public UnityEvent<SceneTransition> OnSceneLoaded = new();
        private Coroutine m_workingCoroutine;

        public void LoadSceneWithLoading(SceneTransition sceneTransition)
        {
            LoadSceneWithLoading(sceneTransition, LoadSceneMode.Single);
        }
        public void LoadSceneWithLoading(SceneTransition transition, LoadSceneMode loadMode)
        {
            switch(loadMode)
            {
                case LoadSceneMode.Single:
                    m_workingCoroutine = StartCoroutine(
                        SingleLoadSceneWithLoadingInternal(transition));
                    break;
                case LoadSceneMode.Additive:
                    break;
            }
        }
        IEnumerator LoadSceneWithLoadingInternal(SceneTransition transition, LoadSceneMode mode)
        {
            // Start loading new scene
            var time = Time.unscaledTime;
            IsLoadingProcess = true;
            OnWillLoadScene.Invoke(transition);
            var isUsingLoadingUI = string.IsNullOrWhiteSpace(transition.LoadingScene);
            
            if (isUsingLoadingUI)
            {

            }
            else
            {

            }

            // Check if we need to add waiting time
            // based on the minimum loading time
            // Can be ignored []
            var deltaTime = Time.unscaledTime - time;
            if (deltaTime < transition.MinLoadingTime)
            {
                yield return new WaitForSecondsRealtime(transition.MinLoadingTime
                    - deltaTime);
            }
            // Scene is loaded completely
            // It's safe to broadcast the OnSceneLoaded
            OnSceneLoaded.Invoke(transition);
            IsLoadingProcess = false;
            StopCoroutine(m_workingCoroutine);
        }
        IEnumerator AdditiveLoadSceneWithLoadingInternal(SceneTransition transition)
        {
            var time = Time.unscaledTime;
            IsLoadingProcess = true;
            OnWillLoadScene.Invoke(transition);

            var isUsingLoadingUI = string.IsNullOrWhiteSpace(transition.LoadingScene);
            if (isUsingLoadingUI)
            {
                if (transition.LoadingUI == null)
                    throw new System.Exception("Loading UI cannot be null!");
                transition.LoadingUI.SetActive(true);
                yield return new WaitForEndOfFrame();

                if (string.IsNullOrWhiteSpace(transition.DestinationScene))
                    throw new System.Exception("Destination scene name cannot be null or empty string!");
                var dest = USceneManager.LoadSceneAsync(transition.DestinationScene, 
                    LoadSceneMode.Additive);
                yield return new WaitUntil(() => dest.isDone);
                var deltaTime = Time.unscaledTime - time;
                if (deltaTime < transition.MinLoadingTime)
                {
                    yield return new WaitForSecondsRealtime(transition.MinLoadingTime
                        - deltaTime);
                }
                transition.LoadingUI.SetActive(false);
                OnSceneLoaded.Invoke(transition);
                IsLoadingProcess = false;
                StopCoroutine(m_workingCoroutine);
            }
            else
            {

            }

        }

        IEnumerator SingleLoadSceneWithLoadingInternal(SceneTransition transition)
        {
            var time = Time.unscaledTime;
            IsLoadingProcess = true;
            OnWillLoadScene.Invoke(transition);
            // Start the loading scene
            if (string.IsNullOrWhiteSpace(transition.LoadingScene))
                throw new System.Exception("Loading scene name cannot be null or empty string!");
            USceneManager.LoadScene(transition.LoadingScene);
            yield return new WaitForEndOfFrame();

            // Load the destination scene
            if (string.IsNullOrWhiteSpace(transition.DestinationScene))
                throw new System.Exception("Destination scene name cannot be null or empty string!");
            var dest = USceneManager.LoadSceneAsync(transition.DestinationScene, 
                LoadSceneMode.Additive);
            // Wait till all the scene components
            // are loaded completely
            yield return new WaitUntil(() => dest.isDone);

            // Check if we need to add waiting time
            // based on the minimum loading time
            // Can be ignored []
            var deltaTime = Time.unscaledTime - time;
            if (deltaTime < transition.MinLoadingTime)
            {
                yield return new WaitForSecondsRealtime(transition.MinLoadingTime 
                    - deltaTime);
            }

            // Unload the loading screen
            USceneManager.UnloadSceneAsync(transition.LoadingScene);
            OnSceneLoaded.Invoke(transition);
            IsLoadingProcess = false;

            // Just to make sure we freed the coroutine
            StopCoroutine(m_workingCoroutine);
        }
    }

    [System.Serializable]
    public struct SceneTransition
    {
        [Tooltip("Name of the destination scene")]
        [SerializeField] private string m_destinationScene;
        [Tooltip("Name of the loading scene used when loading the destination scene")]
        [SerializeField] private string m_LoadingScene;
        [Tooltip("UI to be shown when loading additive scene")]
        [SerializeField] private GameObject m_loadingUI;
        [Tooltip("The minimum time to be used when showing the loading scene")]
        [SerializeField] private float m_minLoadingTime;
        public readonly string DestinationScene => m_destinationScene;
        public readonly string LoadingScene => m_LoadingScene;
        public readonly GameObject LoadingUI => m_loadingUI;
        public readonly float MinLoadingTime => m_minLoadingTime;
    }
}
