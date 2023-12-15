using GameSystems;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlatformerController : MonoBehaviour
    {
        [SerializeField] private int m_strawberryNeededToGetKeyPart = 10;
        [SerializeField] private PlatformerDetector m_detector;
        private int m_strawberries = 0;
        
        [SerializeField] public TopDownMovementController m_topDownController;
        [SerializeField] public PlatformerMovementController platformerMovementController;
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
            
            if(GameManager.Instance.PlatformerActivated) Activate();
            else Deactivate();
        }

        void OnDisable()
        {
            UnListenEvent();
        }

        void Start()
        {
            if(GameManager.Instance.PlatformerActivated) Activate();
            else Deactivate();
        }
        
        private void ListenEvent()
        {
            GameManager.OnActivatePlatformerGame += Activate;
            GameManager.OnDeactivatePlatformerGame += Deactivate;
            m_detector.OnPlatformerObjectEnter += EnterObject;
        }

        private void UnListenEvent()
        {
            GameManager.OnActivatePlatformerGame -= Activate;
            GameManager.OnDeactivatePlatformerGame -= Deactivate;
            m_detector.OnPlatformerObjectEnter -= EnterObject;
        }

        private void EnterObject(PlatformerObject _object)
        {
            m_strawberries += _object.strawberriesNumbers;
            if (_object.strawberriesNumbers > 0) OnGetStrawberries?.Invoke(m_strawberries, _object.strawberriesNumbers);
            if (_object.lifePoints > 0) PlayerDataManager.life += _object.lifePoints;
            if (_object.artifact) ++PlayerDataManager.artifact;
            
            if (m_strawberries >= m_strawberryNeededToGetKeyPart) PlayerDataManager.platformerGameKeyPart = true;
            _object.PickUp();
        }

        private void Activate()
        {
            platformerMovementController.enabled = true;
            m_topDownController.enabled = false;
            m_currentMovementController = platformerMovementController;
            m_playerInput.SwitchCurrentActionMap("Platformer");
            
            m_detector.gameObject.SetActive(true);
            OnActivate?.Invoke();
        }

        private void Deactivate()
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
        

        public bool IsOnGround()
        {
            return m_currentMovementController.IsOnGround();
        }
    }
}