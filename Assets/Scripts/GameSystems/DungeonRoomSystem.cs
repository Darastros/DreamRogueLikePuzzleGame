using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriorityQueue;
using ScriptableObjects;
using TMPro;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utils;
using EventDispatcher = Utils.EventDispatcher;
using Random = System.Random;

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
        public static DungeonRoomSystem Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<DungeonRoomSystem>();
                return _instance;
            }
            private set => _instance = value;
        }
        private static DungeonRoomSystem _instance;

        //Events
        private readonly EventDispatcher m_eventDispatcher = new();
        private EventDispatcher GetEventDispatcher() => m_eventDispatcher;
        public static EventDispatcher EventDispatcher => Instance != null ? Instance.GetEventDispatcher() : null;

        //Settings
        public List<RoomDescriptor> roomPool;
        public List<RoomDescriptor> startRoom;
        public List<RoomDescriptor> exitRoom;

        private Random m_randomRoom = new Random();
        
        public bool forceExitRoomToBeFirstRoom = false;

        public TextMeshProUGUI debugMap;
        public TextMeshProUGUI debugCoordinate;

        // Runtime variables
        private Room m_currentRoom;
        public Room CurrentRoom => m_currentRoom;

        private Dictionary<Vector2Int, Room> m_runtimeRooms = new();
        public Dictionary<Vector2Int, Room> CurrentRooms => m_runtimeRooms;
        private RoomEntrance m_lastDoorOpened;
        public RoomEntrance LastDoorOpened
        {
            private set
            {
                m_lastDoorOpened = value;
                GetEventDispatcher().SendEvent<EventPlayerEnteredRoom>(value);
            }
            get => m_lastDoorOpened;
        }
        // Directions
        private static readonly Vector2Int South = Vector2Int.down;
        private static readonly Vector2Int North = Vector2Int.up;
        private static readonly Vector2Int East = Vector2Int.right;
        private static readonly Vector2Int West = Vector2Int.left;

        private static readonly Vector2Int[] Direction = { North, East, South, West };

        private static readonly Vector2Int SouthEast = South + East;
        private static readonly Vector2Int SouthWest = South + West;
        private static readonly Vector2Int NorthEast = North + East;
        private static readonly Vector2Int NorthWest = North + West;
        
        [ContextMenu("FetchAllRooms")]
        public void RetrieveAllRoomDescriptors()
        {
            Resources.LoadAll<RoomDescriptor>("Rooms");
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
            Resources.LoadAll<RoomDescriptor>("Rooms");

            if (startRoom.Count > 0)
            {
                Room newRoom = new Room(0, 0, startRoom.GetRandomElem());
                m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                OnNewRoomAppear(newRoom, RoomEntrance.None);
            }

            if (!forceExitRoomToBeFirstRoom && exitRoom.Count > 0)
            {
                var exitRoomCoordinate = Direction.GetRandomElem();
                Debug.Log($"SPOILER: EXIT ROOM IS AT POS {exitRoomCoordinate}");
                var exitRoom = GenerateRoom(exitRoomCoordinate.x, exitRoomCoordinate.y, this.exitRoom);
                if (exitRoom != null)
                {
                    m_runtimeRooms.Add(exitRoomCoordinate, exitRoom);
                    GetEventDispatcher().SendEvent<ForceRefreshMap>();
                }
            }

            m_eventDispatcher.RegisterEvent<OnPlayerOpenDoor>(this, OnPlayerWalkDoor);
            GameManager.OnGameRestart += ResetMap;
        }

        private WeightedList<RoomDescriptor> ComputeWeightedList(List<RoomDescriptor> _pool)
        {
            WeightedList<RoomDescriptor> list = new WeightedList<RoomDescriptor>(m_randomRoom);
            foreach (RoomDescriptor roomDescriptor in _pool)
            {
                list.Add(roomDescriptor, Math.Max(1, roomDescriptor.m_weight));
            }
            return list;
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
                return;
            }
            else if(!m_runtimeRooms.Any(_pair => _pair.Value.m_roomDescriptor.m_registerToExitPool)) // If no exit room, first next room to be exit room
            {
                newRoom = GenerateRoom(where.x, where.y, exitRoom);
                if (newRoom != null)
                {
                    Debug.Assert(newRoom.IsValid(), "Generated room is invalid");
                    m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                    OnNewRoomAppear(newRoom, _obj.entrance);
                    return;
                }
            }
            
            {
                newRoom = GenerateRoom(where.x, where.y, roomPool);
                if (newRoom != null)
                {
                    Debug.Assert(newRoom.IsValid(), "Generated room is invalid");
                    m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
                    OnNewRoomAppear(newRoom, _obj.entrance);
                }
                else
                {
                    Debug.LogError($"Failed to generate the ROOM! at position: {where}"+
                                   $"INFO Room in exit pool =: {exitRoom.Count}\n" +
                                   $"INFO Room in start pool =: {startRoom.Count}\n" +
                                   $"INFO Room in Usual pool =: {roomPool.Count}\n" );
                }
            }
        }

        private void OnNewRoomAppear(Room _newRoom, RoomEntrance _from)
        {
            if (m_currentRoom != null)
            {
                GameManager.Instance.HidePlayer();
                m_currentRoom.HideRoom();
            }

            Room oldRoom = m_currentRoom;
            m_currentRoom = _newRoom;
            GetEventDispatcher().SendEvent<OnRoomChanged>(oldRoom, m_currentRoom);
            UpdateDebugMap();
            
            m_currentRoom.ActivateRoom();

            #if UNITY_EDITOR
            ValidateCurrentRoom();
            #endif
            
            GameManager.Instance.TeleportPlayerToRoomEntrance(_from.GetOpposite());
            LastDoorOpened = _from.GetOpposite();
            GameManager.Instance.ShowPlayer();
        }

        private void ValidateCurrentRoom()
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
                        Debug.Assert(false, $"Data not configured correctly {m_currentRoom.m_roomDescriptor}");
                        break;
                }
            }

            bool res = mustHaveNorth == haveNorth && mustHaveEast == haveEast && mustHaveSouth == haveSouth &&
                       mustHaveWest == haveWest;
            if(!res)
                Debug.LogError( $"DATA ERROR - Mismatch between Room Description and runtime room {m_currentRoom.m_roomDescriptor}" );
        }

        private void OnDestroy()
        {
            GameManager.OnGameRestart -= ResetMap;
            m_eventDispatcher.UnregisterAllEvents(this);
        }

        private Room GenerateRoom(int _x, int _y, List<RoomDescriptor> _pool)
        {
            GetNeededEntranceConstraints(out var neededEntrances, out var forbiddenEntrances, _x, _y);
            List<RoomDescriptor> availableRooms = new List<RoomDescriptor>();
                
            availableRooms = _pool.FindAll(_room => DoesRoomMeetRequirement(_room, neededEntrances, forbiddenEntrances));

            { // Remove 4 doors
                List<RoomDescriptor> availableRoomsExceptFour = availableRooms.FindAll(_room => _room.m_entrances != RoomEntrance.Everything);
                if (availableRoomsExceptFour.Count > 0) availableRooms = availableRoomsExceptFour;
            }
            
            
            { // Remove current
                List<RoomDescriptor> availableRoomsExceptCurrent = availableRooms.FindAll(_room => CurrentRoom.m_roomDescriptor != _room);
                if (availableRoomsExceptCurrent.Count > 0) availableRooms = availableRoomsExceptCurrent;
            }

            var weightedList = ComputeWeightedList(availableRooms);

            if (availableRooms.Count > 0)
            {
                var selectedRoom = weightedList.Next();
                var newRoom = new Room(_x, _y, selectedRoom);
                return newRoom;
            }

            Debug.LogErrorFormat("Cannot find any room that meets the {0} with those forbidden entrances {1}", neededEntrances, forbiddenEntrances);
            return null;
        }


        #if UNITY_EDITOR
        [MenuItem("RoomSystem/ResetMap")]
        public static void DebugResetMap()
        {
            Instance.ResetMap();
        }
        #endif
        
        public void ResetMap()
        {
            m_currentRoom = null;
            foreach (var roomCoordinate in m_runtimeRooms.Keys.ToList())
            {
                if (m_runtimeRooms.Remove(roomCoordinate, out Room room))
                {
                    room.Destroy();
                }
            }

            Room newRoom = new Room(0, 0, startRoom.GetRandomElem());
            m_runtimeRooms.Add(newRoom.Coordinate, newRoom);
            OnNewRoomAppear(newRoom, RoomEntrance.None);
            GameManager.Instance.PlayerController.transform.position = Vector2.zero;
        }
        
        public void CloseRoom(RoomEntrance _whichEntrance)
        {
            // check if the current room has a north entrance
            if (m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(_whichEntrance)) 
            {   
                // get the coordinate of the room to the north from the current room
                Vector2Int coordinate = m_currentRoom.Coordinate + _whichEntrance.GetOffset();
                
                // then destroy the room in the north
                CloseRoom(coordinate);
            }
            else
            {
                Debug.LogError($"Trying to close door {_whichEntrance} but this room is suppose to only have {m_currentRoom.m_roomDescriptor.m_entrances}");
            }
        }
        
        public void CloseRoom(Vector2Int _coordinates)
        {
            // check if the current room has a north entrance
            if (m_runtimeRooms.TryGetValue(_coordinates, out Room room)) 
            {
                DestroyRoom(_coordinates);
                GetEventDispatcher().SendEvent<ForceRefreshMap>();
            }
        }
        
        [ContextMenu("Destroy room north")]
        public void DebugDestroyRoomNorth()
        {
            // check if the current room has a north entrance
            if (m_currentRoom.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.North)) 
            {
                // then destroy the room in the north
                CloseRoom(RoomEntrance.North);
            }
            else 
            {
                Debug.Log("The current room does not have a north entrance.");
            }
        }
        private void DestroyRoom(Vector2Int _roomCoordinate)
        {
            if(m_runtimeRooms.Remove(_roomCoordinate, out Room room))
            {
                room.Destroy();
                Debug.Log($"Removed room at coordinate {_roomCoordinate}");
                foreach (Vector2Int valueNeighborsCoordinate in room.m_neighborsCoordinates)
                {
                    if(m_runtimeRooms.TryGetValue(valueNeighborsCoordinate, out Room neighbours))
                    {
                        if (!IsThereAPathLeadingTo(m_currentRoom.Coordinate, neighbours))
                        {
                            DestroyRoom(neighbours.Coordinate);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Trying to remove a non-existing room at {_roomCoordinate}");
            }
            UpdateDebugMap();
        }

        private bool DoesRoomMeetRequirement(
            RoomDescriptor _room,
            RoomEntrance _neededEntrances,
            RoomEntrance _forbiddenEntrances)
        {

            return _room.m_entrances.HasFlag(_neededEntrances) && (_room.m_entrances & _forbiddenEntrances) == RoomEntrance.None; 
        }

        private bool CheckIfRoomHasNeededEntrance(Vector2Int _coordinate, RoomEntrance _neededEntrance)
        {
            if (m_runtimeRooms.TryGetValue(_coordinate, out Room room))
            {
                return room.m_roomDescriptor.m_entrances.HasFlag(_neededEntrance);
            }

            return true;
        }

        [ContextMenu("DebugLogRecomputeCurrentConstraint")]
        private void DebugLogRecomputeCurrentConstraint()
        {
            GetNeededEntranceConstraints(out RoomEntrance neededEntrance, out RoomEntrance forbiddenEntrances,
                m_currentRoom.Coordinate.x, m_currentRoom.Coordinate.y);

            Debug.Log($"Needed Entrances {neededEntrance} - ForbiddenEntrances {forbiddenEntrances}");
            Debug.Log($"Current Entrances {m_currentRoom.m_roomDescriptor.m_entrances}");
        }
        
        private void GetNeededEntranceConstraints(out RoomEntrance _neededRoomEntrance, out RoomEntrance _forbiddenEntrances,
            int _x, int _y)
        {
            Vector2Int coordinate = new(_x, _y);
            _neededRoomEntrance = RoomEntrance.None;
            _forbiddenEntrances = RoomEntrance.None;

            if (m_runtimeRooms.TryGetValue(coordinate, out Room room))
            {
                Debug.LogWarning(
                    "Retrieving Entrances Constraints for an existing room, are you sure you want to do that?...");
            }

            // North
            if (m_runtimeRooms.TryGetValue(coordinate + North, out room))
            {
                if (room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.South))
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
            if (m_runtimeRooms.TryGetValue(coordinate + South, out room)) // If room instantiated next to the coordinate
            {
                bool roomHasNorthDoor = room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.North);
                _neededRoomEntrance |= RoomEntrance.South;
                if(!roomHasNorthDoor)
                {
                    _forbiddenEntrances |= RoomEntrance.South;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + South))) // If room not yet instantiated (door but not open) next to the coordinate but supposed to be there
            {
                _neededRoomEntrance |= RoomEntrance.South;
            }

            // East
            if (m_runtimeRooms.TryGetValue(coordinate + East, out room))
            {
                bool roomHasWestDoor = room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.West);
               _neededRoomEntrance |= RoomEntrance.East;
                
                if(!roomHasWestDoor)
                {
                    _forbiddenEntrances |= RoomEntrance.East;
                }
            }
            else if(m_runtimeRooms.Values.Any(_r => _r.m_neighborsCoordinates.Contains(coordinate + East)))
            {
                _neededRoomEntrance |= RoomEntrance.East;
            }

            // West
            if (m_runtimeRooms.TryGetValue(coordinate + West, out room))
            {
                bool roomHaveEastDoor = room.m_roomDescriptor.m_entrances.HasFlag(RoomEntrance.East);
                _neededRoomEntrance |= RoomEntrance.West;
                if (!roomHaveEastDoor)
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
                !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.West) ||
                !CheckIfRoomHasNeededEntrance(coordinate + South + South, RoomEntrance.North))
            {
                _forbiddenEntrances |= RoomEntrance.South;
            }

            //Can appear north
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.West) ||
                !CheckIfRoomHasNeededEntrance(coordinate + NorthWest, RoomEntrance.East) ||
                !CheckIfRoomHasNeededEntrance(coordinate + North + North, RoomEntrance.South))
            {
                _forbiddenEntrances |= RoomEntrance.North;
            }

            //Can appear east
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthEast, RoomEntrance.South) ||
                !CheckIfRoomHasNeededEntrance(coordinate + SouthEast, RoomEntrance.North) ||
                !CheckIfRoomHasNeededEntrance(coordinate + East + East, RoomEntrance.West))
            {
                _forbiddenEntrances |= RoomEntrance.East;
            }

            //Can appear west
            if (!CheckIfRoomHasNeededEntrance(coordinate + NorthWest, RoomEntrance.South) ||
                !CheckIfRoomHasNeededEntrance(coordinate + SouthWest, RoomEntrance.North)||
                !CheckIfRoomHasNeededEntrance(coordinate + West + West, RoomEntrance.East))
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

        public List<Room> GetInstanciatedNeighbors(Room _room)
        {
            List<Room> result = new List<Room>();
            foreach (Vector2Int roomNeighborsCoordinate in _room.m_neighborsCoordinates)
            {
                if (CurrentRooms.TryGetValue(roomNeighborsCoordinate, out var neighbor))
                {
                    result.Add(neighbor);
                }
            }
            return result;
        }

        public List<Room> ComputePathLeadingTo(Room _target, Room _from, List<Room> _blackList = null)
        {
            
            //Variables
            Dictionary<Room, Room> cameFrom = new();
            Dictionary<Room, int> costSoFar = new();

            List<Room> path = new List<Room>();
            SimplePriorityQueue<Room> frontiers = new SimplePriorityQueue<Room>();
            bool found = false;

            cameFrom.Add(_from, _from);
            costSoFar.Add(_from, 0);
            Room current;
            frontiers.Enqueue(_from, 0);

            while (frontiers.TryDequeue(out current))
            {
                if(current.Coordinate == _target.Coordinate)
                {
                    found = true;
                    break;
                }

                foreach (Room neighbor in current.GetInstanciatedNeighbors())
                {
                    if(_blackList?.Contains(neighbor) == true)
                        continue;
                    
                    int cost = costSoFar[current] + 1;
                    if (!costSoFar.ContainsKey(neighbor) || cost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = cost;
                        cameFrom[neighbor] = current;
                        int priority = cost + computeHeuristic(neighbor, _target);
                        frontiers.Enqueue(neighbor, priority);
                    }
                }
            }

            if (!found) return path;
            while (current != _from)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            return path;
        }

        private int computeHeuristic(Room _a, Room _b)
        {
            return (int)(_b.Coordinate - _a.Coordinate).magnitude;
        }
        
        public bool IsThereAPathLeadingTo(Vector2Int _targetCoordinates, Room _startingRoom, Room _blackListRoom = null)
        {
            var visitedRooms = new HashSet<Room>(); 
            var roomsToVisit = new Stack<Room>();

            if (_blackListRoom != null)
                visitedRooms.Add(_blackListRoom);

            roomsToVisit.Push(_startingRoom);
            void AddAllUnvisitedNeighborsToVisitStack(Room _currentRoom)
            {
                foreach (var neighborCoordinate in _currentRoom.m_neighborsCoordinates)
                {
                    if (m_runtimeRooms.TryGetValue(neighborCoordinate, out Room neighborRoom) && !visitedRooms.Contains(neighborRoom))
                    {
                        roomsToVisit.Push(neighborRoom);
                    }
                }
            }

            // Initial population from the starting room
            AddAllUnvisitedNeighborsToVisitStack(_startingRoom);
    
            while (roomsToVisit.TryPop(out Room currentRoom))
            {
                // Mark room as visited when it's popped (ensures it won't be visited again)
                visitedRooms.Add(currentRoom);

                if (currentRoom.Coordinate == _targetCoordinates)
                {
                    return true;
                }

                // Visit all unvisited neighbor rooms
                AddAllUnvisitedNeighborsToVisitStack(currentRoom);
            }
            return false;
        }

        private void UpdateDebugMap()
        {
            Vector2Int mapSizeMax = Vector2Int.zero;
            Vector2Int mapSizeMin = Vector2Int.zero;

            HashSet<Vector2Int> existingRooms = new HashSet<Vector2Int>();
            foreach (var (key, room) in m_runtimeRooms)
            {
                foreach (Vector2Int roomNeighborsCoordinate in room.m_neighborsCoordinates)
                {
                    mapSizeMax.x = math.max(roomNeighborsCoordinate.x, mapSizeMax.x);
                    mapSizeMax.y = math.max(roomNeighborsCoordinate.y, mapSizeMax.y);
                    
                    mapSizeMin.x = math.min(roomNeighborsCoordinate.x, mapSizeMin.x);
                    mapSizeMin.y = math.min(roomNeighborsCoordinate.y, mapSizeMin.y);
                    existingRooms.Add(roomNeighborsCoordinate);
                }
            }

            StringBuilder builder = new StringBuilder();
            for (int y = mapSizeMax.y; y >= mapSizeMin.y; y--)
            {
                for (int x = mapSizeMin.x; x <= mapSizeMax.x; x++)
                {
                    Vector2Int coordinate = new Vector2Int(x, y);
                    if (coordinate == Vector2Int.zero)
                    {
                        if (coordinate == Vector2Int.zero)
                        {
                            builder.Append("[S]");
                        }
                    }
                    else if (m_runtimeRooms.TryGetValue(coordinate, out Room room))
                    {
                        builder.Append(room == m_currentRoom ? "[\u00a4]" : "[*]");
                    }
                    else if(existingRooms.Contains(coordinate))
                    {
                        builder.Append("[?]");
                    }
                    else
                    {
                        builder.Append("[ ]");
                    }
                }
                builder.AppendLine();
            }
            if(debugMap)
                debugMap.text = builder.ToString();
            if (debugCoordinate)
                debugCoordinate.text = m_currentRoom.Coordinate.ToString();
        }
    }

    internal struct Node : IComparable<Node>
    {
        public int priority;
        public Room Room;

        public Node(int _priority, Room _room)
        {
            priority = _priority;
            Room = _room;
        }

        public int CompareTo(Node obj)
        {
            return priority.CompareTo(obj.priority);
        }
    }
}

