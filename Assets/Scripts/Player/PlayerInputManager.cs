using System;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        private IMovementController m_movementController;
        private void OnEnable()
        {
            TryGetComponent(out m_movementController);
        }

        private void OnDisable()
        {
            m_movementController = null;
        }

        public void OnMoveInput(InputAction.CallbackContext _ctx)
        {
            m_movementController.Move(_ctx.ReadValue<Vector2>());
        }
        
        public void OnJumpImput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                m_movementController.Jump(_ctx.ReadValueAsButton());
            }
        }
    }
}