using DG.Tweening;
using Player;

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

        private Sequence m_initSequence = null;

        private void OnEnable()
        {
            DungeonRoomSystem.EventDispatcher?.RegisterEvent<EventPlayerEnteredRoom>(this, OnPlayerEnterRoom);
            if(m_collider2D == null)
                TryGetComponent(out m_collider2D);
            m_animator.SetBool(CanSealAnimHash, DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null && PlayerDataManager.artifact > 0);
            m_animator.SetBool(HoverAnimHash, false);

            if(DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null)
            {
                m_animator.SetTrigger(OpenAnimHash);
            }

            m_initSequence = DOTween.Sequence().AppendInterval(timeBeforeAllowingPlayerToOpenTheDoor);
            m_initSequence.onComplete = ActivateDoorTrigger;
            m_initSequence.Play();
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
            if(m_initSequence != null && m_initSequence.active && m_initSequence.IsPlaying()) m_initSequence.Kill();
            m_collider2D.isTrigger = false;
            GameManager.OnTeleportPlayer -= OnPlayerTeleported;
            if(DungeonRoomSystem.Instance != null)
            {
                DungeonRoomSystem.EventDispatcher?.UnregisterEvent<EventPlayerEnteredRoom>(this);
            }
        }
        
        private void ActivateDoorTrigger()
        {
            if (TryGetComponent(out Collider2D collide) && !collide.isTrigger && !m_isColliding)
            {
                if(DungeonRoomSystem.Instance.LastDoorOpened == whichEntrance)
                    m_animator.SetTrigger(OpenAnimHash);
                collide.isTrigger = true;
                GameManager.OnTeleportPlayer += OnPlayerTeleported;
            }
        }

        private void OnPlayerTeleported(Vector3 _pos)
        {
            if (!this) return;
            if ((_pos - teleportPos.position).sqrMagnitude < 1)
            {
                m_collider2D.isTrigger = false;
                ForceClose();
                Sequence enableSeq = DOTween.Sequence().AppendInterval(timeBeforeAllowingPlayerToOpenTheDoor);
                enableSeq.onComplete = ActivateDoorTrigger;
                enableSeq.Play();
            }
        }

        private void OnCollisionEnter2D(Collision2D _other)
        {
            m_isColliding = true;
        }

        private void OnCollisionExit2D(Collision2D _other)
        {
            if(m_initSequence != null && m_initSequence.active && m_initSequence.IsPlaying())
                return;
            
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

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.CompareTag("Player"))
            {
                DungeonRoomSystem.EventDispatcher?.SendEvent<OnPlayerOpenDoor>(whichEntrance);
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

        public void ForceClose()
        {
            m_animator.SetTrigger(CloseDoorAnimHash);
        }
    }
}