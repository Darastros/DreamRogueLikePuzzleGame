using System;
using GameSystems;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomDescriptor", menuName = "Create Room descriptor", order = 0)]
    public class RoomDescriptor : ScriptableObject
    {
        public RoomEntrance m_entrances;
        public GameObject m_prefab;

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

        [ContextMenu("ComputeEntrances")]
        private void ComputeEntrances()
        {
            RoomEntrance type = RoomEntrance.None;
            foreach (Door door in m_prefab.GetComponentsInChildren<Door>())
            {
                type |= door.whichEntrance;
            }
            m_entrances = type;
        }

        [MenuItem("RoomSystem/RecomputeAllEntrancesForRooms")]
        private static void RecomputeAllEntrancesForRooms()
        {
            foreach (RoomDescriptor roomDescriptor in Resources.FindObjectsOfTypeAll<RoomDescriptor>())
            {
                roomDescriptor.ComputeEntrances();
            }
        }
    }
}