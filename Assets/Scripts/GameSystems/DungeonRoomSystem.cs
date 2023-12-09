using System;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using EventDispatcher = Utils.EventDispatcher;

namespace GameSystems
{
    public class DungeonRoomSystem : MonoBehaviour, IEventListener
    {
        public static DungeonRoomSystem Instance { get; private set; }
        
        //Events
        private EventDispatcher m_eventDispatcher = new EventDispatcher();
        public EventDispatcher GetEventDispatcher() => m_eventDispatcher;

        //Settings
        public List<RoomDescriptor> roomPool;
        public List<RoomDescriptor> startRoom;
        public List<RoomDescriptor> exitRoom;
        
        
        // Runtime variables
        private Room m_currentRoom;
        private Dictionary<Vector2Int, Room> m_runtimeRooms = new();

        // Directions
        private static Vector2Int South = Vector2Int.down;
        private static Vector2Int North = Vector2Int.up;
        private static Vector2Int East = Vector2Int.right;
        private static Vector2Int West = Vector2Int.left;
        
        private static Vector2Int SouthEast = South + East ;
        private static Vector2Int SouthWest = South + West ;
        private static Vector2Int NorthEast = North + East ;
        private static Vector2Int NorthWest = North + West ;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            if (startRoom.Count > 0)
            {
                m_currentRoom = new Room(0, 0, startRoom.GetRandomElem());
                m_runtimeRooms.Add(m_currentRoom.Coordinate, m_currentRoom);
            }
            m_eventDispatcher.RegisterEvent<OnPlayerOpenDoor>(this, OnPlayerWalkDoor);
        }

        Vector2Int GetRoomCoordinate(RoomEntrance _entrance)
        {
            return m_currentRoom.Coordinate + _entrance.GetOffset();
        }
        
        private void OnPlayerWalkDoor(OnPlayerOpenDoor _obj)
        {
            Vector2Int where = GetRoomCoordinate(_obj.entrance);
            Room newRoom = GenerateRoom(where.x, where.y);
            if (newRoom != null)
            {
                m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                OnNewRoomGenerated(newRoom);
            }
        }

        private void OnNewRoomGenerated(Room _newRoom)
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            m_eventDispatcher.UnregisterAllEvents(this);
        }
        
        private Room GenerateRoom(int _x, int _y)
        {
            GetNeededEntranceConstraints(out var neededEntrances, out var forbiddenEntrances, _x, _y);
            List<RoomDescriptor> availableRooms = roomPool.FindAll(_room => _room.m_entrances.HasFlag(neededEntrances) && !_room.m_entrances.HasFlag(forbiddenEntrances));
            if (availableRooms.Count > 0)
            {
                var selectedRoom = availableRooms.GetRandomElem();
                var newRoom = new Room(_x, _y, selectedRoom);
                return newRoom;
            }
            Debug.AssertFormat(false, "Cannot find any room that meets the {0} with those forbidden entrances {1}", neededEntrances, forbiddenEntrances);
            return null;
        }
        
        private void ApplyNeededEntranceConstraintIfRoomAtCoordinate(ref RoomEntrance _constraint, Vector2Int _coordinate, RoomEntrance _constraintToAdd)
        {
            if (m_runtimeRooms.TryGetValue(_coordinate, out Room _room))
            {
                if (_room.roomDescriptor.m_entrances.HasFlag(_constraintToAdd.GetOpposite()))
                {
                    _constraint |= _constraintToAdd; 
                }
            }
        }

        bool CheckIfRoomHasNeededEntrance(Vector2Int _coordinate, RoomEntrance _neededEntrance)
        {
            if (m_runtimeRooms.TryGetValue(_coordinate, out Room _room))
            {
                return _room.roomDescriptor.m_entrances.HasFlag(_neededEntrance);
            }
            return true;
        }

        private void GetNeededEntranceConstraints(out RoomEntrance _neededEntance, out RoomEntrance _forbiddenEntrances, int _x, int _y)
        {
            Vector2Int coordinate = new(_x, _y);
            _neededEntance = RoomEntrance.Invalid;
            _forbiddenEntrances = RoomEntrance.Invalid; // using invalid as "None"
            
            if (m_runtimeRooms.TryGetValue(coordinate, out Room _room))
            {   
                Debug.LogWarning("Retrieving Entrances Constraints for an existing room, are you sure you want to do that?...");
            }
          
            // North
            ApplyNeededEntranceConstraintIfRoomAtCoordinate(ref _neededEntance, coordinate + North, RoomEntrance.North);
            
            // South
            ApplyNeededEntranceConstraintIfRoomAtCoordinate(ref _neededEntance, coordinate + South, RoomEntrance.South);
            
            // East
            ApplyNeededEntranceConstraintIfRoomAtCoordinate(ref _neededEntance, coordinate + East, RoomEntrance.East);
            
            // West
            ApplyNeededEntranceConstraintIfRoomAtCoordinate(ref _neededEntance, coordinate + West, RoomEntrance.West);

            
            // Check if room can appear at position
            //Can appear south
            if (!CheckIfRoomHasNeededEntrance(coordinate + SouthWest, RoomEntrance.East) || !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.West))
            {
                _forbiddenEntrances |= RoomEntrance.South;
            }
            
            //Can appear north
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.West) || !CheckIfRoomHasNeededEntrance(NorthWest + coordinate, RoomEntrance.East))
            {
                _forbiddenEntrances |= RoomEntrance.North;
            }
            
            //Can appear east
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.South) || !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.North))
            {
                _forbiddenEntrances |= RoomEntrance.East;
            }
            
            //Can appear west
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthWest, RoomEntrance.South) || !CheckIfRoomHasNeededEntrance(coordinate + SouthWest, RoomEntrance.North))
            {
                _forbiddenEntrances |= RoomEntrance.West;
            }
            
            Debug.Assert(_neededEntance != RoomEntrance.Invalid, "Cannot find room around the targeted position, should never happen" );
            Debug.Assert(_forbiddenEntrances == RoomEntrance.Everything, "This room can't have doors?!" );
            
        }
    }
    
    [Flags, Serializable]
    public enum RoomEntrance : int
    {
        Invalid = 0,
        North = 1,
        South = 1 << 2,
        East = 1 << 3,
        West = 1 << 4,
            
        Everything = Invalid | North | South | East | West
    }
    
    public class Room
    {
        public Vector2Int Coordinate { get; }
        public RoomDescriptor roomDescriptor;

        public Room(int _x, int _y, RoomDescriptor _roomDescriptor) : this(new Vector2Int(_x,_y), _roomDescriptor)
        {}
        
        public Room(Vector2Int _coordinate, RoomDescriptor _roomDescriptor)
        {
            Coordinate = _coordinate;
            roomDescriptor = _roomDescriptor;
        }
    }
}