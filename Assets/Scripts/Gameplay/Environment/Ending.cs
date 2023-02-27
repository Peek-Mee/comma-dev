using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Comma.Gameplay.Environment
{
    public class Ending : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutScene;

        public void PlayCutscene()
        {
            _cutScene.Play();
        }
    }
}
