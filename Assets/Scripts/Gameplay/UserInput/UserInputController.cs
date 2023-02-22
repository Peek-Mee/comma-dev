using UnityEngine.InputSystem;
using UnityEngine;
using Comma.Global.PubSub;
using UnityEngine.Events;

namespace Comma.Gameplay.UserInput
{
    public class UserInputController : MonoBehaviour
    {
        private static UserInputManager _userInputManager;

        public static UserInputManager UserInputManager => _userInputManager;
        
        public static UnityEvent OnInputManagerLoaded = new UnityEvent();

        private void Start()
        {
            _userInputManager = new UserInputManager();
            _userInputManager.Player.Enable();
            _userInputManager.Player.Movement.performed += OnMoveInput;
            _userInputManager.Player.Movement.canceled += OnMoveInput;
            _userInputManager.Player.Jump.performed += OnJumpInput;
            //_userInputManager.Player.Jump.canceled += OnJumpInput;
            _userInputManager.Player.Interact.performed += OnInteractInput;
            //_userInputManager.Player.Interact.canceled += OnInteractInput;
            _userInputManager.Player.Sprint.performed += OnSprintInput;
            _userInputManager.Player.Sprint.canceled += OnSprintInput;
            _userInputManager.Player.Pause.performed += OnPauseInput;
            OnInputManagerLoaded?.Invoke();
        }
        private void OnDisable()
        {
            _userInputManager.Player.Movement.performed -= OnMoveInput;
            _userInputManager.Player.Movement.canceled -= OnMoveInput;
            _userInputManager.Player.Jump.performed -= OnJumpInput;
            //_userInputManager.Player.Jump.canceled += OnJumpInput;
            _userInputManager.Player.Interact.performed -= OnInteractInput;
            //_userInputManager.Player.Interact.canceled += OnInteractInput;
            _userInputManager.Player.Sprint.performed -= OnSprintInput;
            _userInputManager.Player.Sprint.canceled -= OnSprintInput;
            _userInputManager.Player.Pause.performed -= OnPauseInput;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventConnector.Publish("OnPlayerMove", new OnPlayerMove((Vector2)context.ReadValueAsObject()));
            }

            if (context.canceled)
            {
                EventConnector.Publish("OnPlayerMove", new OnPlayerMove(Vector2.zero));
            }
        }

        private void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventConnector.Publish("OnPlayerJump", new OnPlayerJump());
            }
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventConnector.Publish("OnPlayerInteract", new OnPlayerInteract());
            }
        }

        private void OnSprintInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventConnector.Publish("OnPlayerSprint", new OnPlayerSprint(true));
            }

            if (context.canceled)
            {
                EventConnector.Publish("OnPlayerSprint", new OnPlayerSprint(false));

            }
        }
        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventConnector.Publish("OnPauseInput", 0);
            }
        }
    }
}
