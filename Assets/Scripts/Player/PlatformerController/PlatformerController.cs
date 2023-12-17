using System.Collections.Generic;
using GameSystems;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Platformer
{
    public class PlatformerController : MonoBehaviour, IEventListener
    {
        [SerializeField] private int m_strawberryNeededToGetKeyPart = 10;
        [SerializeField] private PlatformerDetector m_detector;
        private int m_strawberries = 0;
        public HashSet<PlatformerObject> ObjectsCollectedInTheRoom { get; set; } = new();
        
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

        public void Restart()
        {
            Deactivate();
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
            PlayerDataManager.OnHit += OnHit;
            DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
        }
        private void UnListenEvent()
        {
            GameManager.OnActivatePlatformerGame -= Activate;
            GameManager.OnDeactivatePlatformerGame -= Deactivate;
            m_detector.OnPlatformerObjectEnter -= EnterObject;
            PlayerDataManager.OnHit -= OnHit;
            DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<OnRoomChanged>(this);
        }

        private void EnterObject(PlatformerObject _object)
        {
            if (_object.needNextRoomToBeActivated)
            {
                ObjectsCollectedInTheRoom.Add(_object);
            }
            else
            {
                m_strawberries += _object.strawberriesNumbers;
                if (_object.strawberriesNumbers > 0) OnGetStrawberries?.Invoke(m_strawberries, _object.strawberriesNumbers);
                if (m_strawberries >= m_strawberryNeededToGetKeyPart) PlayerDataManager.platformerGameKeyPart = true;
                if (_object.lifePoints > 0) PlayerDataManager.life += _object.lifePoints;
                if (_object.artifact) ++PlayerDataManager.artifact;
            }
            _object.PickUp(this);
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
        
        private void OnHit(int _newvalue, int _delta)
        {
            foreach (PlatformerObject platformerObject in ObjectsCollectedInTheRoom)
            {
                platformerObject.ResetPosition();
            }
            ObjectsCollectedInTheRoom.Clear();
        }

        private void OnRoomChanged(OnRoomChanged _obj)
        {
            int strawBerriesToAdd = 0;
            foreach (PlatformerObject platformerObject in ObjectsCollectedInTheRoom)
            {
                strawBerriesToAdd += platformerObject.strawberriesNumbers;
                if (platformerObject.lifePoints > 0) PlayerDataManager.life += platformerObject.lifePoints;
                if (platformerObject.artifact) ++PlayerDataManager.artifact;
                platformerObject.OnRoomChanged();
            }
            m_strawberries += strawBerriesToAdd;
            if (strawBerriesToAdd > 0) OnGetStrawberries?.Invoke(m_strawberries, strawBerriesToAdd);
            if (m_strawberries >= m_strawberryNeededToGetKeyPart) PlayerDataManager.platformerGameKeyPart = true;
            
            ObjectsCollectedInTheRoom.Clear();
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