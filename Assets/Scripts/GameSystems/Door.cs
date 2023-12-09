using System;
using MovementControllers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameSystems
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Collider2D m_collider2D;

        public RoomEntrance whichEntrance;
        public Transform teleportPos;
        
        private void OnEnable()
        {
            if(m_collider2D == null)
                TryGetComponent(out m_collider2D);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IMovementController>(out var _player))
            {
                DungeonRoomSystem.Instance.GetEventDispatcher().SendEvent<OnPlayerOpenDoor>(whichEntrance);
            }
        }

        private void OnValidate()
        {
            if (whichEntrance != 0 && (whichEntrance & (whichEntrance - 1)) != 0)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("ERROR", "RoomEntrance should not contain multiple flags.",
                    "OK I'LL REMOVE IT NOW");
#endif
                Debug.LogError("RoomEntrance should not contain multiple flags.", this);
            }
        }
    }
}