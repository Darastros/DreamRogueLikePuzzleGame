using System.Collections;
using System.Collections.Generic;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlatformerController : MonoBehaviour
    {
        [SerializeField] private PlatformerDetector m_detector;
        private int m_strawberries = 0;
        
        [SerializeField] private TopDownMovementController m_topDownController;
        [SerializeField] private PlatformerMovementController platformerMovementController;
        [SerializeField] private PlayerInput m_playerInput;

        public delegate void Switch();
        public static Switch OnActivate;
        public static Switch OnDeactivate;

        public delegate void GetResources(int _total, int _number);
        public static GetResources OnGetStrawberries;

        private IMovementController m_currentMovementController;
        void OnEnable()
        {
            ListenEvent();
            if(GameManager.PlatformerActivated) ActivatePlatformerGame();
            else DeactivatePlatformerGame();
        }

        void OnDisable()
        {
            UnListenEvent();
        }
        
        private void ListenEvent()
        {
            GameManager.OnActivatePlatformerGame += ActivatePlatformerGame;
            GameManager.OnDeactivatePlatformerGame += DeactivatePlatformerGame;
            m_detector.OnPlatformerObjectEnter += EnterObject;
        }

        private void UnListenEvent()
        {
            GameManager.OnActivatePlatformerGame -= ActivatePlatformerGame;
            GameManager.OnDeactivatePlatformerGame -= DeactivatePlatformerGame;
            m_detector.OnPlatformerObjectEnter -= EnterObject;
        }

        private void EnterObject(PlatformerObject _object)
        {
            m_strawberries += _object.strawberriesNumbers;
            if (_object.strawberriesNumbers > 0) OnGetStrawberries?.Invoke(m_strawberries, _object.strawberriesNumbers);
            _object.PickUp();
        }

        private void ActivatePlatformerGame()
        {
            platformerMovementController.enabled = true;
            m_topDownController.enabled = false;
            m_currentMovementController = platformerMovementController;
            m_playerInput.SwitchCurrentActionMap("Platformer");
            
            m_detector.gameObject.SetActive(true);
            OnActivate?.Invoke();
        }

        private void DeactivatePlatformerGame()
        {
            platformerMovementController.enabled = false;
            m_topDownController.enabled = true;
            m_currentMovementController = m_topDownController;
            m_playerInput.SwitchCurrentActionMap("TopDown");
            
            m_detector.gameObject.SetActive(false);
            OnDeactivate?.Invoke();
            m_strawberries = 0;
        }
        
        public void Move(Vector2 _value)
        {
            m_currentMovementController.Move(_value);
        }
        
        public void Jump(bool _value)
        {
            m_currentMovementController.Jump(_value);
        }
    }
}