using UnityEngine.InputSystem;
using UnityEngine;

namespace Comma.Gameplay.UserInput
{
    public class UserInputController : MonoBehaviour
    {
        private UserInputManager _userInputManager;

        private void Start()
        {
            _userInputManager = new UserInputManager();
            _userInputManager.Player.Enable();
            _userInputManager.Player.Movement.performed += OnMoveInput;
            _userInputManager.Player.Movement.canceled += OnMoveInput;
            _userInputManager.Player.Jump.performed += OnJumpInput;
            _userInputManager.Player.Jump.canceled += OnJumpInput;
            _userInputManager.Player.Interact.performed += OnInteractInput;
            _userInputManager.Player.Interact.canceled += OnInteractInput;
            _userInputManager.Player.Sprint.performed += OnSprintInput;
            _userInputManager.Player.Sprint.canceled += OnSprintInput;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Debug.Log("Input move " + context.ReadValueAsObject());
            if (context.performed)
            {
                // Move object
            }

            if (context.canceled)
            {
                // Stop object
            }
        }

        private void OnJumpInput(InputAction.CallbackContext context)
        {
            Debug.Log("Input jump " + context.ReadValueAsObject());
            if (context.performed)
            {
                // Vertical acceleration
            }

            if (context.canceled)
            {
                // Stop accelerate
            }
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            Debug.Log("Input interact " + context.ReadValueAsObject());
            if (context.performed)
            {
                // Interact with object
            }
        }

        private void OnSprintInput(InputAction.CallbackContext context)
        {
            Debug.Log("Input sprint " + context.ReadValueAsObject());
            if (context.performed)
            {
                // Boost speed
            }

            if (context.canceled)
            {
                // Stop extra speed
            }
        }
    }
}