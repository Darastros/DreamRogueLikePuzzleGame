using GameSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomDescriptor", menuName = "Create Room descriptor", order = 0)]
    public class RoomDescriptor : ScriptableObject
    {
        public RoomEntrance m_entrances;
        public GameObject m_prefab;

        /// <summary>
        /// Probability of this room to spawn is influenced by this weight (1 is minimum)
        /// </summary>
        public int m_weight = 1;

        public GameRuleType m_gameRuleType = GameRuleType.None;

        public bool m_registerToRoomPool = true;
        public bool m_registerToStartPool;
        public bool m_registerToExitPool;

        private void OnValidate()
        {
            if (m_prefab != null && m_entrances == RoomEntrance.None)
            {
                ComputeEntrances();
            }
        }
#if UNITY_EDITOR
        private void Reset()
        {
            if (m_prefab == null && Selection.activeGameObject)
            {
                m_prefab = Selection.activeGameObject;
                ComputeEntrances();
            }
        }

        [ContextMenu("ComputeEntrances")]
#endif
        private void ComputeEntrances()
        {
            if (m_prefab == null)
                return;

            RoomEntrance type = RoomEntrance.None;
            foreach (Door door in m_prefab.GetComponentsInChildren<Door>())
            {
                type |= door.whichEntrance;
            }

            m_entrances = type;
        }

#if UNITY_EDITOR
        [MenuItem("RoomSystem/RecomputeAllEntrancesForRooms")]
        private static void RecomputeAllEntrancesForRooms()
        {
            foreach (RoomDescriptor roomDescriptor in Resources.FindObjectsOfTypeAll<RoomDescriptor>())
            {
                roomDescriptor.ComputeEntrances();
            }
        }
#endif
    }
}