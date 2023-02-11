using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Comma.Gameplay.UserInput;
using TMPro;
using Comma.Global.SaveLoad;

namespace Comma.Global.Settings
{
    [System.Serializable]
    public struct KeyBind
    {
        public string Name;
        public int Index;
        public Button Button;
        [HideInInspector] public TMP_Text Text;
        [HideInInspector] public InputAction InputAction;
        [HideInInspector] public string DefaultBinding;

        public void Init()
        {
            this.InputAction = UserInputController.UserInputManager.FindAction(Name);
            this.Text = Button.gameObject.GetComponentInChildren<TMP_Text>();
            DefaultBinding = InputAction.bindings[Index].effectivePath;
        }
    }

    public class InputSetting : MonoBehaviour
    {
        [SerializeField] private KeyBind[] _keyBinds;
        [SerializeField] private Button _setToDefault;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _cancelButton;

        private bool _isRebinding = false;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private string _previousBinding;
        private InputSaveData _currentInputSaveData;
        private InputSaveData _newInputSaveData;
        public static InputSetting Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            UserInputController.OnInputManagerLoaded.AddListener(Init);
            _setToDefault.onClick.RemoveAllListeners();
            _applyButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _setToDefault.onClick.AddListener(SetToDefault);
            _applyButton.onClick.AddListener(ApplyInputSetting);
            _cancelButton.onClick.AddListener(CancelInputSetting);
        }

        private void Init()
        {
            _currentInputSaveData = SaveSystem.GetInputSetting();
            _newInputSaveData = (InputSaveData)_currentInputSaveData.Clone();

            for (int i = 0; i < _keyBinds.Length; i++)
            {
                KeyBind keyBind = _keyBinds[i];
                keyBind.Button.onClick.RemoveAllListeners();
                keyBind.Button.onClick.AddListener(delegate { StartRebind(keyBind); });
                keyBind.Init();
                _keyBinds[i] = keyBind;
            }

            UserInputController.UserInputManager.LoadBindingOverridesFromJson(_currentInputSaveData.GetBindingOverride());

            RefreshUI();
        }

        private void RefreshUI()
        {
            for (int i = 0; i < _keyBinds.Length; i++)
            {
                KeyBind keyBind = _keyBinds[i];
                string newText = InputControlPath.ToHumanReadableString(
                    keyBind.InputAction.bindings[keyBind.Index].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
                keyBind.Text.text = newText.Replace("Right", "R.").Replace("Left", "L.");
            }
        }

        private void StartRebind(KeyBind keyBind)
        {
            if (!_isRebinding)
            {
                UserInputController.UserInputManager.Disable();
                _previousBinding = keyBind.InputAction.bindings[keyBind.Index].effectivePath;
                Rebinding(keyBind);
            }
        }

        private void Rebinding(KeyBind keyBind)
        {
            _isRebinding = true;

            keyBind.Text.text = "Press input...";
            keyBind.Button.interactable = false;

            _rebindingOperation = keyBind.InputAction.PerformInteractiveRebinding(keyBind.Index)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("/<Mouse>/leftButton/")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(operation => CancelRebind(keyBind))
                .OnComplete(operation => CheckForDuplicate(keyBind))
                .Start();
        }

        private void CheckForDuplicate(KeyBind keyBind)
        {
            for (int i = 0; i < _keyBinds.Length; i++)
            {
                if (_keyBinds[i].Name == keyBind.Name)
                {
                    if (_keyBinds[i].Index == keyBind.Index)
                    {
                        continue;
                    }
                }
                if (keyBind.InputAction.bindings[keyBind.Index].effectivePath == _keyBinds[i].InputAction.bindings[_keyBinds[i].Index].effectivePath)
                {
                    SetToPreviousBind(keyBind);
                    return;
                }
            }

            RebindComplete(keyBind);
        }

        private void SetToPreviousBind(KeyBind keyBind)
        {
            _rebindingOperation.Dispose();
            Rebinding(keyBind);
        }

        private void CancelRebind(KeyBind keyBind)
        {
            keyBind.InputAction.ApplyBindingOverride(keyBind.Index, _previousBinding);
            RebindComplete(keyBind);
        }

        private void RebindComplete(KeyBind keyBind)
        {
            _rebindingOperation.Dispose();

            keyBind.Text.text = InputControlPath.ToHumanReadableString(
                keyBind.InputAction.bindings[keyBind.Index].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            UserInputController.UserInputManager.Enable();
            keyBind.Button.interactable = true;
            _isRebinding = false;

            _newInputSaveData.ChangeBindingOverride(UserInputController.UserInputManager.SaveBindingOverridesAsJson());
        }

        private void SetToDefault()
        {
            UserInputController.UserInputManager.RemoveAllBindingOverrides();
            RefreshUI();
            _newInputSaveData.ChangeBindingOverride(UserInputController.UserInputManager.SaveBindingOverridesAsJson());
        }

        private void ApplyInputSetting()
        {
            _currentInputSaveData = (InputSaveData)_newInputSaveData.Clone();
            SaveSystem.ChangeDataReference(_currentInputSaveData);
            SaveSystem.SaveDataToDisk();
        }

        private void CancelInputSetting()
        {
            UserInputController.UserInputManager.RemoveAllBindingOverrides();
            UserInputController.UserInputManager.LoadBindingOverridesFromJson(_currentInputSaveData.GetBindingOverride());
            RefreshUI();
        }
    }
}