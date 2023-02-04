using UnityEngine;
using UnityEngine.Playables;

namespace Comma.CutScene
{
    [RequireComponent(typeof(Collider2D))]
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutsceneToBePlay;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                    _cutsceneToBePlay.Play();

            }            
        }

    }

}
