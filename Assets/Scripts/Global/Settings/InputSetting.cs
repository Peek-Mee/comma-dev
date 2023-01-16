using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Comma.Gameplay.UserInput;
using TMPro;

namespace Comma.Global.Setting
{
    [System.Serializable]
    public struct KeyBind
    {
        public string Name;
        public Button Button;
        [HideInInspector] public TMP_Text Text;
        [HideInInspector] public InputAction InputAction;

        public void Init()
        {
            this.InputAction = UserInputController.UserInputManager.FindAction(Name);
            this.Text = Button.gameObject.GetComponentInChildren<TMP_Text>();
            this.Text.text = InputControlPath.ToHumanReadableString(
                UserInputController.UserInputManager.FindAction(Name).bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }

    public class InputSetting : MonoBehaviour
    {
        [SerializeField] private KeyBind[] _keyBinds;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private void Start()
        {
            UserInputController.OnInputManagerLoaded.AddListener(Init);
        }

        private void Init()
        {
            for (int i = 0; i < _keyBinds.Length; i++)
            {
                KeyBind keyBind = _keyBinds[i];
                keyBind.Button.onClick.RemoveAllListeners();
                keyBind.Button.onClick.AddListener(delegate { StartRebind(keyBind); });
                keyBind.Init();
            }
        }

        private void StartRebind(KeyBind keyBind)
        {
            UserInputController.UserInputManager.Disable();

            keyBind.Text.text = "Press input..";

            _rebindingOperation = keyBind.InputAction.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete(keyBind))
                .Start();
        }

        private void RebindComplete(KeyBind keyBind)
        {
            _rebindingOperation.Dispose();

            Debug.Log("Done rebind");

            keyBind.Text.text = InputControlPath.ToHumanReadableString(
                UserInputController.UserInputManager.FindAction(keyBind.Name).bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            UserInputController.UserInputManager.Enable();
        }
    }
}