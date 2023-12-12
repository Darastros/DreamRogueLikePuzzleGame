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
        [SerializeField] public LayerMask PlayerLayer;
        [SerializeField] private float m_useRadius = 1f;

        private void OnValidate()
        {
            if (PlayerLayer == 0)
            {
                PlayerLayer = gameObject.layer;
            }
        }


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

        public void OnUseInput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                var hit = Physics2D.CircleCast(transform.position, m_useRadius, transform.forward, 0, ~PlayerLayer);
                if (hit.transform != null && hit.transform.TryGetComponent<IUsable>(out var usable))
                {
                    usable.Use(gameObject);
                }
            }
        }
    }

    public interface IUsable
    {
        void Use(GameObject _user);
    }
}