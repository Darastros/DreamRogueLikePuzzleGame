using ScriptableObjects;
using UnityEngine;

namespace GameSystems
{
    public class Room
    {
        public Vector2Int Coordinate { get; }
        public readonly RoomDescriptor m_roomDescriptor;
        public GameObject m_runtimeGameScene = null;

        public Room(int _x, int _y, RoomDescriptor _roomDescriptor) : this(new Vector2Int(_x, _y), _roomDescriptor)
        {
        }

        public Room(Vector2Int _coordinate, RoomDescriptor _roomDescriptor)
        {
            Coordinate = _coordinate;
            m_roomDescriptor = _roomDescriptor;
            
            Debug.Log($"Creating room at coordinate {_coordinate}, with this descriptor {_roomDescriptor}");
            Debug.Assert(IsValid(), "Invalid room");
        }

        public bool IsValid()
        {
            if (m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.Invalid) ||
                m_roomDescriptor.m_entrances == RoomEntrance.None)
            {
                Debug.Log($"DATA ERROR on room descriptor, incorrect flags {m_roomDescriptor.m_entrances}");
                return false;
            }

            return true;
        }
    }
}