using System;
using DG.Tweening;
using MovementControllers;
using Player;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utils;

namespace GameSystems
{
    public class Door : MonoBehaviour, IUsable, IEventListener
    {
        [SerializeField] private Collider2D m_collider2D;
        private Animator m_animator;

        public float timeBeforeAllowingPlayerToOpenTheDoor = 1f;
        public RoomEntrance whichEntrance;
        public Transform teleportPos;
        private bool m_isColliding = false;
        
        private static readonly int CanSealAnimHash = Animator.StringToHash("canSeal");
        private static readonly int HoverAnimHash = Animator.StringToHash("hover");
        private static readonly int OpenAnimHash = Animator.StringToHash("Open");
        private static readonly int SealAnimHash = Animator.StringToHash("Seal");
        
        //TODO MATHIEU CLOSE DOOR HERE WHEN TO CLOSE
        private static readonly int CloseDoorAnimHash = Animator.StringToHash("Close");

        private void OnEnable()
        {
            DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<EventPlayerEnteredRoom>(this, OnPlayerEnterRoom);
            if(m_collider2D == null)
                TryGetComponent(out m_collider2D);
            m_animator.SetBool(CanSealAnimHash, DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null && PlayerDataManager.artifact > 0);
            m_animator.SetBool(HoverAnimHash, false);

            if(DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null)
            {
                m_animator.SetTrigger(OpenAnimHash);
            }

            Sequence enableSeq = DOTween.Sequence().AppendInterval(timeBeforeAllowingPlayerToOpenTheDoor);
            enableSeq.onComplete = ActivateDoorTrigger;
            enableSeq.Play();
        }

        private void OnPlayerEnterRoom(EventPlayerEnteredRoom _obj)
        {
            if (_obj.door == whichEntrance)
            {
                m_animator.SetTrigger(CloseDoorAnimHash);
            }
        }

        private void OnDisable()
        {
            if (TryGetComponent(out Collider2D _collider))
            {
                _collider.isTrigger = false;
            }
            DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<EventPlayerEnteredRoom>(this);
        }
        
        private void ActivateDoorTrigger()
        {
            if (TryGetComponent(out Collider2D _collider) && !_collider.isTrigger && !m_isColliding)
            {
                if(DungeonRoomSystem.Instance.LastDoorOpened == whichEntrance)
                    m_animator.SetTrigger(OpenAnimHash);
                _collider.isTrigger = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            m_isColliding = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            m_isColliding = false;
            ActivateDoorTrigger();
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Update()
        {
            m_animator.SetBool(CanSealAnimHash, DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null && PlayerDataManager.artifact > 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DungeonRoomSystem.Instance.GetEventDispatcher().SendEvent<OnPlayerOpenDoor>(whichEntrance);
            }
        }

        public void Use(GameObject _user)
        {
            if (DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null && PlayerDataManager.TryUseArtifact())
            {
                DungeonRoomSystem.Instance.CloseRoom(whichEntrance);
                m_animator.SetTrigger(SealAnimHash);
            }
        }

        public void Hover()
        {
            //m_animator.SetLayerWeight(m_animator.GetLayerIndex("Hover"), 1.0f);
            m_animator.SetBool(HoverAnimHash, true);
        }

        public void Exit()
        {
            //m_animator.SetLayerWeight(m_animator.GetLayerIndex("Hover"), 1.0f);
            m_animator.SetBool(HoverAnimHash, false);
            
        }
    }
}