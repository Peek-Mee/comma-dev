using Comma.Global.SaveLoad;
using UnityEngine;
using UnityEngine.Playables;

namespace Comma.CutScene
{
    [RequireComponent(typeof(Collider2D))]
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutsceneToBePlay;
        [SerializeField] private string _targetCutsceneId;

        private void Awake()
        {
            if (SaveSystem.GetPlayerData().IsCutsceneInCollection(_targetCutsceneId))
                gameObject.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _cutsceneToBePlay.Play();
                gameObject.SetActive(false);

            }            
        }

    }

}
