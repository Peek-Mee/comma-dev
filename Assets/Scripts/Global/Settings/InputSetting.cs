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
        public Button SetToDefault;
        [HideInInspector] public TMP_Text Text;
        [HideInInspector] public InputAction InputAction;
        [HideInInspector] public string DefaultBinding;

        public void Init()
        {
            this.InputAction = UserInputController.UserInputManager.FindAction(Name);
            this.Text = Button.gameObject.GetComponentInChildren<TMP_Text>();
            this.Text.text = InputControlPath.ToHumanReadableString(
                InputAction.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
            DefaultBinding = InputAction.bindings[0].effectivePath;
        }
    }

    public class InputSetting : MonoBehaviour
    {
        [SerializeField] private KeyBind[] _keyBinds;

        private bool _isRebinding = false;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private string _previousBinding;

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
                keyBind.SetToDefault.onClick.RemoveAllListeners();
                keyBind.Button.onClick.AddListener(delegate { StartRebind(keyBind); });
                keyBind.SetToDefault.onClick.AddListener(delegate { SetToDefault(keyBind); });
                keyBind.Init();
                _keyBinds[i] = keyBind;
            }
        }

        private void StartRebind(KeyBind keyBind)
        {
            if (!_isRebinding)
            {
                UserInputController.UserInputManager.Disable();
                _previousBinding = keyBind.InputAction.bindings[0].effectivePath;
                Rebinding(keyBind);
            }
        }

        private void Rebinding(KeyBind keyBind)
        {
            _isRebinding = true;

            keyBind.Text.text = "Press input..";
            keyBind.Button.interactable = false;

            _rebindingOperation = keyBind.InputAction.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("/<Mouse>/leftButton/")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(operation => SetToPreviousBind(keyBind))
                .OnComplete(operation => CheckForDuplicate(keyBind))
                .Start();
        }

        private void CheckForDuplicate(KeyBind keyBind)
        {
            for (int i = 0; i < _keyBinds.Length; i++)
            {
                if (_keyBinds[i].Name == keyBind.Name)
                {
                    continue;
                }
                if (keyBind.InputAction.bindings[0].effectivePath == _keyBinds[i].InputAction.bindings[0].effectivePath)
                {
                    CancelRebind(keyBind);
                    return;
                }
            }

            RebindComplete(keyBind);
        }

        private void CancelRebind(KeyBind keyBind)
        {
            _rebindingOperation.Dispose();
            Rebinding(keyBind);
        }

        private void SetToPreviousBind(KeyBind keyBind)
        {
            keyBind.InputAction.ApplyBindingOverride(_previousBinding);
            RebindComplete(keyBind);
        }

        private void RebindComplete(KeyBind keyBind)
        {
            _rebindingOperation.Dispose();

            keyBind.Text.text = InputControlPath.ToHumanReadableString(
                keyBind.InputAction.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            UserInputController.UserInputManager.Enable();
            keyBind.Button.interactable = true;
            _isRebinding = false;
        }

        private void SetToDefault(KeyBind keyBind)
        {
            keyBind.InputAction.ApplyBindingOverride(keyBind.DefaultBinding);
            keyBind.Text.text = InputControlPath.ToHumanReadableString(
                keyBind.InputAction.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}