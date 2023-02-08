using UnityEngine;
using UnityEngine.UI;

namespace Comma.Utility.Collections
{
    public class LogoSwitcher : MonoBehaviour
    {
        [SerializeField] private Image[] _logoCollection;
        private void OnEnable()
        {
            Randomizer();
        }

        private void Randomizer()
        {
            float random = Random.value * 10f;
            int next = (int) random % _logoCollection.Length;
            for (int i = 0; i < _logoCollection.Length; i++)
            {
                if (i == next)
                {
                    _logoCollection[i].gameObject.SetActive(true);
                }
                else
                {
                    _logoCollection[i].gameObject.SetActive(false);
                }
            }
        }
    }

}
