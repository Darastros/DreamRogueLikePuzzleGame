﻿using UnityEngine;

namespace GameSystems
{
    [CreateAssetMenu(fileName = "RoomDescriptor", menuName = "Create Room descriptor", order = 0)]
    public class RoomDescriptor : ScriptableObject
    {
        public RoomEntrance m_entrances;
        public GameObject m_prefab;
    }
}