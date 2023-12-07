using System;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(IMovementController))]
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
    }
}