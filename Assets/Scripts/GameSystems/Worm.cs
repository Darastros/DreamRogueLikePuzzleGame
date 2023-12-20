using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Event = Utils.Event;

namespace GameSystems
{
    public class Worm : MonoBehaviour, IEventListener
    {
        public EventDispatcher EventDispatcher { get; private set; } = new();
        // Events
        public delegate void WormEvent();
    
        public static WormEvent OnWormStartEatingRoom;
        
        [SerializeField] private int minRoomThresholdForWormToAppear = 30;
        [SerializeField] private float timeUntilRoomDestroyed = 30f;
        [SerializeField] private float newTimerIfRoomSaved = 120f;
        [SerializeField] private float newTimerIfRoomLost = 100f;
        [SerializeField] private bool forceMinRoomToDestroyOthers = false;

        private bool m_wormAppeared = false;
        private Coroutine m_wormCoroutine = null;

        [SerializeField] private GameObject m_wormIndicatorPrefab;
        private GameObject m_wormIndicatorRuntime;

        public Room RoomAboutToBeDestroyed { get; private set; } = null;

        private void Start()
        {
            Debug.Assert(GameManager.Instance.Worm == null, "MULTIPLE WORMS ON THE SCENE, IT'S NOT SUPPORTED");
            GameManager.Instance.Worm = this;
            DungeonRoomSystem.EventDispatcher?.RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
            DungeonRoomSystem.EventDispatcher?.RegisterEvent<EventPlayerEnteredRoom>(this, OnPlayerEnteredRoom);
        }

        private void OnDestroy()
        {
            DungeonRoomSystem.EventDispatcher?.UnregisterAllEvents(this);
        }

        private void OnRoomChanged(OnRoomChanged _obj)
        {
            if (m_wormCoroutine != null && RoomAboutToBeDestroyed.Coordinate == _obj.m_to.Coordinate)
            {
                StopCoroutine(m_wormCoroutine);
                m_wormCoroutine = null;
                RoomAboutToBeDestroyed = null;
                EventDispatcher.SendEvent<OnRoomSavedFromWorm>(_obj.m_to);
                StartCoroutine(Reset(newTimerIfRoomSaved));
            }
            
            if (DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear)
            {
                if (!m_wormAppeared)
                {
                    m_wormAppeared = true;
                    m_wormCoroutine = StartCoroutine(DestroyRoomCoroutine(GetRoomAtTheEdgeV2()));
                }
            }
        }
        
        private void OnPlayerEnteredRoom(EventPlayerEnteredRoom _obj)
        {
            UpdateWormIndicator();
        }

        private void UpdateWormIndicator()
        {
            if(m_wormIndicatorPrefab == null || DungeonRoomSystem.Instance.CurrentRoom.m_runtimeGameScene == null)
                return;
            
            if (m_wormIndicatorRuntime != null)
            {
                Destroy(m_wormIndicatorRuntime);
            }
            
            if (RoomAboutToBeDestroyed != null)
            {
                var neighbors = DungeonRoomSystem.Instance.CurrentRoom.GetInstanciatedNeighbors();
                List<Room> leading = DungeonRoomSystem.Instance.ComputePathLeadingTo(RoomAboutToBeDestroyed, DungeonRoomSystem.Instance.CurrentRoom);
                var candidates = leading.Where((_room, _i) => neighbors.Contains(_room)).ToList();

                if (candidates.Count > 0)
                {
                    Room random = candidates.GetRandomElem();
                    var door = DungeonRoomSystem.Instance.CurrentRoom.GetEntrance(
                        (random.Coordinate - DungeonRoomSystem.Instance.CurrentRoom.Coordinate)
                        .ConvertToRoomEntrance());
                    m_wormIndicatorRuntime = Instantiate(m_wormIndicatorPrefab, door.transform.position, Quaternion.identity);
                    m_wormIndicatorRuntime.transform.SetParent(door.transform, true);
                }
            }
        }

        private IEnumerator DestroyRoomCoroutine(Room _roomToDestroy)
        {
            RoomAboutToBeDestroyed = _roomToDestroy;
            DungeonRoomSystem.EventDispatcher?.SendEvent<ForceRefreshMap>();
            EventDispatcher.SendEvent<OnWormStartEatingRoom>(_roomToDestroy);
            OnWormStartEatingRoom?.Invoke();
            UpdateWormIndicator();
            yield return new WaitForSeconds(timeUntilRoomDestroyed);
            DungeonRoomSystem.Instance.CloseRoom(_roomToDestroy.Coordinate);
            UpdateWormIndicator();
            m_wormCoroutine = null;
            RoomAboutToBeDestroyed = null;
            StartCoroutine(Reset(newTimerIfRoomLost));
        }

        private IEnumerator Reset(float _newTimer)
        {
            yield return new WaitForSeconds(_newTimer);
            if (!forceMinRoomToDestroyOthers ||(forceMinRoomToDestroyOthers && DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear))
            {
                m_wormCoroutine = StartCoroutine(DestroyRoomCoroutine(GetRoomAtTheEdgeV2()));
            }
            else if(forceMinRoomToDestroyOthers)
            {
                EventDispatcher.SendEvent<OnWormLeft>();
                m_wormAppeared = false;
            }
        }

        /// <summary>
        /// Get room at the edge of the map, sorted by distance of the player
        /// </summary>
        /// <returns></returns>
        private List<Room> GetRoomAtTheEdge()
        {
            Vector2Int curCoordinate = DungeonRoomSystem.Instance.CurrentRoom.Coordinate;
            
            Vector2Int minBox = Vector2Int.zero;
            Vector2Int maxBox = Vector2Int.zero;
            foreach (var (coordinates, value) in DungeonRoomSystem.Instance.CurrentRooms)
            {
                minBox.x = math.min(coordinates.x, minBox.x);
                minBox.y = math.min(coordinates.y, minBox.y);
                
                maxBox.x = math.max(coordinates.x, maxBox.x);
                maxBox.y = math.max(coordinates.y, maxBox.y);
            }

            List<Room> candidates = new ();
            foreach (var (coordinates, value) in DungeonRoomSystem.Instance.CurrentRooms)
            {
                if (coordinates.x == minBox.x || coordinates.x == maxBox.x || coordinates.y == minBox.y ||
                    coordinates.y == maxBox.y)
                {
                    candidates.Add(value);
                }
            }
            var sortedRoom = candidates.OrderByDescending((_room) => (_room.Coordinate - curCoordinate).sqrMagnitude).ToList();
            return sortedRoom;
        }

        private Room GetRoomAtTheEdgeV2(bool _ignore4NeighborsConstraint = false)
        {
            Vector2Int curCoordinate = DungeonRoomSystem.Instance.CurrentRoom.Coordinate;
            List<Room> candidates = new List<Room>();
            foreach (var (coordinate, room) in DungeonRoomSystem.Instance.CurrentRooms)
            {
                if (coordinate == Vector2Int.zero && coordinate != curCoordinate)
                {
                    if (room.GetInstanciatedNeighbors().Count == 1)
                    {
                        candidates.Add(room);
                    }
                }
                
                bool neighborHaveLinkToStartRoom = true;
                var neighbors = room.GetInstanciatedNeighbors();
                
                if (neighbors.Count == 4 && !_ignore4NeighborsConstraint)
                    continue;
                
                foreach (Room instanciatedNeighbor in neighbors)
                {
                    neighborHaveLinkToStartRoom = neighborHaveLinkToStartRoom && DungeonRoomSystem.Instance.IsThereAPathLeadingTo(Vector2Int.zero, instanciatedNeighbor, room);
                }
                if(neighborHaveLinkToStartRoom && coordinate != curCoordinate)
                    candidates.Add(room);
            }

            Room result = candidates.GetRandomElem();
            if (result == null && !_ignore4NeighborsConstraint)
                result = GetRoomAtTheEdgeV2(true);
            return result;
        }

        [ContextMenu("Log candidates")]
        void DebugLogRooms()
        {
            Debug.Log(GetRoomAtTheEdgeV2().Coordinate);
        }
    }

    public class OnWormStartEatingRoom : Event
    {
        public Room m_roomAboutToBeDestroyed;

        public OnWormStartEatingRoom(){}
        
        public OnWormStartEatingRoom(Room _roomAboutToBeDestroyed)
        {
            m_roomAboutToBeDestroyed = _roomAboutToBeDestroyed;
        }
    }
    
    public class OnWormLeft : Event
    {
    }
    
    public class OnRoomSavedFromWorm : Event
    {
        public Room m_roomSaved;
        public OnRoomSavedFromWorm(){}
        public OnRoomSavedFromWorm(Room _roomSaved)
        {
            m_roomSaved = _roomSaved;
        }
    }
}