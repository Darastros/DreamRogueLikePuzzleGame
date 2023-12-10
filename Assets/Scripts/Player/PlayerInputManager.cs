using System;
using CardGame;
using MovementControllers;
using Platformer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlatformerController platformerController;
        [SerializeField] private CardGameController m_cardGameController;
        
        public void OnMoveInput(InputAction.CallbackContext _ctx)
        {
            platformerController.Move(_ctx.ReadValue<Vector2>());
        }
        
        public void OnJumpImput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                platformerController.Jump(_ctx.ReadValueAsButton());
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