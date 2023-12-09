using System;
using CardGame;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        private MovementManager m_movementManager;
        private CardGameController m_cardGameController;
        private void OnEnable()
        {
            TryGetComponent(out m_movementManager);
            TryGetComponent(out m_cardGameController);
        }

        private void OnDisable()
        {
            m_movementManager = null;
        }

        public void OnMoveInput(InputAction.CallbackContext _ctx)
        {
            m_movementManager.Move(_ctx.ReadValue<Vector2>());
        }
        
        public void OnJumpImput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                m_movementManager.Jump(_ctx.ReadValueAsButton());
            }
        }
        
        public void OnCraftInput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                m_cardGameController.Craft();
            }
        }
    }
}