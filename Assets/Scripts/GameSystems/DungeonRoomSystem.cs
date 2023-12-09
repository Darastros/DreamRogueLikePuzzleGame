using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovementControllers;
using Player;
using ScriptableObjects;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using EventDispatcher = Utils.EventDispatcher;

namespace GameSystems
{
    
    [Flags, Serializable]
    public enum RoomEntrance : int
    {
        None = 0,
        North = 1,
        South = 1 << 2,
        East = 1 << 3,
        West = 1 << 4,
        
        Invalid = Int32.MaxValue,
        Everything = None | North | South | East | West
    }
    public class DungeonRoomSystem : MonoBehaviour, IEventListener
    {
        public static DungeonRoomSystem Instance { get; private set; }

        //Events
        private EventDispatcher m_eventDispatcher = new();
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

        private static Vector2Int SouthEast = South + East;
        private static Vector2Int SouthWest = South + West;
        private static Vector2Int NorthEast = North + East;
        private static Vector2Int NorthWest = North + West;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        [ContextMenu("FetchAllRooms")]
        public void RetrieveAllRoomDescriptors()
        {
            roomPool.Clear();
            exitRoom.Clear();
            startRoom.Clear();

            foreach (RoomDescriptor roomDescriptor in Resources.FindObjectsOfTypeAll<RoomDescriptor>())
            {
                if (roomDescriptor.m_registerToRoomPool)
                    roomPool.Add(roomDescriptor);
                else if (roomDescriptor.m_registerToStartPool)
                {
                    startRoom.Add(roomDescriptor);
                }
                else if (roomDescriptor.m_registerToExitPool)
                {
                    exitRoom.Add(roomDescriptor);
                }
            }
        }

        private void Start()
        {
            if (startRoom.Count > 0)
            {
                Room newRoom = new Room(0, 0, startRoom.GetRandomElem());
                m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                OnNewRoomAppear(newRoom, RoomEntrance.None);
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

            if (m_runtimeRooms.TryGetValue(where, out Room newRoom))
            {
                OnNewRoomAppear(newRoom, _obj.entrance);
            }
            else
            {
                newRoom = GenerateRoom(where.x, where.y);
                if (newRoom != null)
                {
                    Debug.Assert(newRoom.IsValid(), "Generated room is invalid");
                    m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                    OnNewRoomAppear(newRoom, _obj.entrance);
                }
                else
                {
                    Debug.Assert(false, "Failed to generate the ROOM!");
                }
            }
        }

        private void OnNewRoomAppear(Room _newRoom, RoomEntrance _from)
        {
            if (m_currentRoom != null)
            {
                m_currentRoom.m_runtimeGameScene.SetActive(false);
            }
                
            m_currentRoom = _newRoom;
            
            var player = FindObjectOfType<PlayerInputManager>().GetComponent<Rigidbody2D>(); // TODO CLEAN THIS SHIT
            
            if (m_currentRoom.m_runtimeGameScene != null)
            {
                m_currentRoom.m_runtimeGameScene.SetActive(true);
            }
            else
            {
                m_currentRoom.m_runtimeGameScene = Instantiate(m_currentRoom.m_roomDescriptor.m_prefab);
            }

            #if UNITY_EDITOR
            ValidateCurrentRoom();
            #endif
            
            //TODO: Clean this shit
            Door[] doors = m_currentRoom.m_runtimeGameScene.GetComponentsInChildren<Door>();
            foreach (var door in doors)
            {
                if (door.whichEntrance.GetOpposite() == _from)
                {
                    player.position = door.teleportPos.position;
                    break;
                }
            }
        }

        private bool ValidateCurrentRoom()
        {
            bool mustHaveNorth = m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.North);
            bool mustHaveSouth = m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.South);
            bool mustHaveEast = m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.East);
            bool mustHaveWest = m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.West);

            bool haveNorth = false;
            bool haveSouth = false;
            bool haveEast = false;
            bool haveWest = false;
            
            Door[] doors = m_currentRoom.m_runtimeGameScene.GetComponentsInChildren<Door>();
            foreach (Door door in doors)
            {
                switch (door.whichEntrance)
                {
                    case RoomEntrance.North:
                        haveNorth = true;
                        break;
                    case RoomEntrance.South:
                        haveSouth = true;
                        break;
                    case RoomEntrance.East:
                        haveEast = true;
                        break;
                    case RoomEntrance.West:
                        haveWest = true;
                        break;
                    default:
                        Debug.Assert(false, $"Data not setuped correctly {m_currentRoom.m_roomDescriptor}");
                        break;
                }
            }

            bool res = mustHaveNorth == haveNorth && mustHaveEast == haveEast && mustHaveSouth == haveSouth &&
                       mustHaveWest == haveWest;
            if(!res)
                Debug.LogError( $"DATA ERROR - Missmatch between Room Description and runtime room {m_currentRoom.m_roomDescriptor}" );
            return res;
        }

        private void OnDestroy()
        {
            m_eventDispatcher.UnregisterAllEvents(this);
        }

        private Room GenerateRoom(int _x, int _y)
        {
            GetNeededEntranceConstraints(out var neededEntrances, out var forbiddenEntrances, _x, _y);
            List<RoomDescriptor> availableRooms = roomPool.FindAll(_room =>
                DoesRoomMeetRequirement(_room, neededEntrances, forbiddenEntrances));

            if (availableRooms.Count > 0)
            {
                var selectedRoom = availableRooms.GetRandomElem();
                var newRoom = new Room(_x, _y, selectedRoom);
                return newRoom;
            }

            Debug.AssertFormat(false, "Cannot find any room that meets the {0} with those forbidden entrances {1}",
                neededEntrances, forbiddenEntrances);
            return null;
        }

        private bool DoesRoomMeetRequirement(
            RoomDescriptor _room,
            RoomEntrance _neededEntrances,
            RoomEntrance _forbiddenEntrances)
        {

            return _room.m_entrances.HasFlag(_neededEntrances) && (_room.m_entrances & _forbiddenEntrances) == RoomEntrance.None; 
        }

        bool CheckIfRoomHasNeededEntrance(Vector2Int _coordinate, RoomEntrance _neededEntrance)
        {
            if (m_runtimeRooms.TryGetValue(_coordinate, out Room _room))
            {
                return _room.m_roomDescriptor.m_entrances.HasFlag(_neededEntrance);
            }

            return true;
        }

        [ContextMenu("DebugLogRecomputeCurrentConstraint")]
        private void DebugLogRecomputeCurrentConstraint()
        {
            GetNeededEntranceConstraints(out RoomEntrance _neededEntance, out RoomEntrance _forbiddenEntrances,
                m_currentRoom.Coordinate.x, m_currentRoom.Coordinate.y);

            Debug.Log($"Needed Entrances {_neededEntance} - ForbiddenEntrances {_forbiddenEntrances}");
            Debug.Log($"Current Entrances {m_currentRoom.m_roomDescriptor.m_entrances}");
        }
        
        private void GetNeededEntranceConstraints(out RoomEntrance _neededRoomEntrance, out RoomEntrance _forbiddenEntrances,
            int _x, int _y)
        {
            Vector2Int coordinate = new(_x, _y);
            _neededRoomEntrance = RoomEntrance.None;
            _forbiddenEntrances = RoomEntrance.None;

            if (m_runtimeRooms.TryGetValue(coordinate, out Room _room))
            {
                Debug.LogWarning(
                    "Retrieving Entrances Constraints for an existing room, are you sure you want to do that?...");
            }

            // North
            if (m_runtimeRooms.TryGetValue(coordinate + North, out _room))
            {
                if (_room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.South))
                {
                    _neededRoomEntrance |= RoomEntrance.North;
                }
                else
                {
                    _forbiddenEntrances |= RoomEntrance.North;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + North)))
            {
                _neededRoomEntrance |= RoomEntrance.North;
            }


            // South
            if (m_runtimeRooms.TryGetValue(coordinate + South, out _room)) // If room instantiated next to the coordinate
            {
                if (_room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.North)) 
                {
                    _neededRoomEntrance |= RoomEntrance.South;
                }
                else
                {
                    _forbiddenEntrances |= RoomEntrance.South;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + South))) // If room not yet instantiated (door but not open) next to the coordinate but supposed to be there
            {
                _neededRoomEntrance |= RoomEntrance.South;
            }

            // East
            if (m_runtimeRooms.TryGetValue(coordinate + East, out _room))
            {
                if (_room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.West))
                {
                    _neededRoomEntrance |= RoomEntrance.East;
                }
                else
                {
                    _forbiddenEntrances |= RoomEntrance.East;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + East)))
            {
                _neededRoomEntrance |= RoomEntrance.East;
            }

            // West
            if (m_runtimeRooms.TryGetValue(coordinate + West, out _room))
            {
                if (_room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.East))
                {
                    _neededRoomEntrance |= RoomEntrance.West;
                }
                else
                {
                    _forbiddenEntrances |= RoomEntrance.West;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + West)))
            {
                _neededRoomEntrance |= RoomEntrance.West;
            }


            // Check if room can appear at position
            //Can appear south
            if (!CheckIfRoomHasNeededEntrance(coordinate + SouthWest, RoomEntrance.East) ||
                !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.West))
            {
                _forbiddenEntrances |= RoomEntrance.South;
            }

            //Can appear north
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.West) ||
                !CheckIfRoomHasNeededEntrance(coordinate + NorthWest, RoomEntrance.East))
            {
                _forbiddenEntrances |= RoomEntrance.North;
            }

            //Can appear east
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.South) ||
                !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.North))
            {
                _forbiddenEntrances |= RoomEntrance.East;
            }

            //Can appear west
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthWest, RoomEntrance.South) ||
                !CheckIfRoomHasNeededEntrance(coordinate + SouthWest, RoomEntrance.North))
            {
                _forbiddenEntrances |= RoomEntrance.West;
            }

            Debug.Assert(_neededRoomEntrance != RoomEntrance.None && _neededRoomEntrance != RoomEntrance.Invalid,
                $"Cannot find room around the targeted position, should never happen => {_neededRoomEntrance}");
            if(_neededRoomEntrance != RoomEntrance.None && _neededRoomEntrance != RoomEntrance.Invalid)
                Debug.DebugBreak();
            Debug.Assert(_forbiddenEntrances != RoomEntrance.Everything,
                $"This room can't have doors?! => {_forbiddenEntrances}");
        }
    }
}

