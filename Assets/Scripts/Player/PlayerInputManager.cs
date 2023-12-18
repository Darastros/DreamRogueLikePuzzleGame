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
        private static PlayerInputManager m_instance;
        public static PlayerInputManager instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = FindObjectOfType<PlayerInputManager>();
                }
                return m_instance;
            }
        }
    
        public delegate void SimpleEvent();
        public static SimpleEvent OnJump;
        public static SimpleEvent OnCraft;
        public static SimpleEvent OnUse;
        
        [SerializeField] private PlatformerController platformerController;
        [SerializeField] private CardGameController m_cardGameController;
        [SerializeField] private DetectDoor m_detectDoor;
        [SerializeField] public LayerMask PlayerLayer;
        [SerializeField] private float m_useRadius = 1f;

        public Vector2 moveInput;
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
            moveInput = _ctx.ReadValue<Vector2>();
        }
        
        public void OnJumpImput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                OnJump?.Invoke();
                platformerController.Jump(_ctx.ReadValueAsButton());
            }
        }
        public void OnCraftInput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                OnCraft?.Invoke();
                m_cardGameController.Craft();
            }
        }

        public void OnUseInput(InputAction.CallbackContext _ctx)
        {
            if (_ctx.performed)
            {
                OnUse?.Invoke();
                m_detectDoor.Use();
            }
        }
    }

    public interface IUsable
    {
        void Use(GameObject _user);
        void Hover();
        void Exit();
    }
}