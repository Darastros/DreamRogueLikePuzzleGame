using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace GameSystems
{
    public class Room
    {
        public Vector2Int Coordinate { get; }
        public readonly RoomDescriptor m_roomDescriptor;
        public GameObject m_runtimeGameScene = null;
        public HashSet<Vector2Int> m_neighborsCoordinates = new();

        public Room(int _x, int _y, RoomDescriptor _roomDescriptor) : this(new Vector2Int(_x, _y), _roomDescriptor)
        {
        }

        public Room(Vector2Int _coordinate, RoomDescriptor _roomDescriptor)
        {
            Coordinate = _coordinate;
            m_roomDescriptor = _roomDescriptor;
            
            if (m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.South))
            {
                m_neighborsCoordinates.Add(Coordinate+ Vector2Int.down);
            }
            
            if (m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.North))
            {
                m_neighborsCoordinates.Add(Coordinate + Vector2Int.up);
            }
            
            if (m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.East))
            {
                m_neighborsCoordinates.Add(Coordinate + Vector2Int.right);
            }
            
            if (m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.West))
            {
                m_neighborsCoordinates.Add(Coordinate + Vector2Int.left);
            }
            
            Debug.Log($"Creating room at coordinate {_coordinate}, with this descriptor {_roomDescriptor}");
            Debug.Assert(IsValid(), "Invalid room");
        }

        public void RemoveRuntimeObject()
        {
            Object.Destroy(m_runtimeGameScene);
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