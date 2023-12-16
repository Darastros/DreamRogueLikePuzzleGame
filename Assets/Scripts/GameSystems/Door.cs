using System;
using MovementControllers;
using Player;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameSystems
{
    public class Door : MonoBehaviour, IUsable
    {
        [SerializeField] private Collider2D m_collider2D;
        private Animator m_animator;

        public RoomEntrance whichEntrance;
        public Transform teleportPos;
        
        private void OnEnable()
        {
            if(m_collider2D == null)
                TryGetComponent(out m_collider2D);
            if(DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null)
                m_animator.SetTrigger("Open");
                m_animator.SetBool("hover", false);
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Update()
        {
            m_animator.SetBool("canSeal", DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbor(whichEntrance) != null && PlayerDataManager.artifact > 0);
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
                m_animator.SetTrigger("Seal");
            }
        }

        public void Hover()
        {
            m_animator.SetBool("hover", true);
        }

        public void Exit()
        {
            m_animator.SetBool("hover", false);
            
        }
    }
}