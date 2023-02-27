
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Comma.CutScene
{
    public class TransitionLevel : MonoBehaviour
    {
        private Scene current;
        public void ChangeLevel(string levelName)
        {
            current = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            StartCoroutine(WaitLoadScene(levelName));
        }

        IEnumerator WaitLoadScene(string level)
        { 
            yield return new WaitUntil(() =>  SceneManager.GetSceneByName(level).isLoaded);
            SceneManager.UnloadSceneAsync(current);
        }
    }

}
