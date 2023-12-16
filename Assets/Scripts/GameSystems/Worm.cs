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
        
        [SerializeField] private int minRoomThresholdForWormToAppear = 30;
        [SerializeField] private float eatingEveryTime = 30f;
        [SerializeField] private float timeUntilRoomDestroyed = 20f;
        
        private bool m_wormAppeared = false;
        private Coroutine m_wormCoroutine = null;
        private Dictionary<Vector2Int, Coroutine> m_roomCoroutines = new ();
        
        public List<Vector2Int> RoomsAboutToBeDestoyed => m_roomCoroutines.Keys.ToList();

        private void Start()
        {
            Debug.Assert(GameManager.Instance.Worm == null, "MULTIPLE WORMS ON THE SCENE, IT'S NOT SUPPORTED");
            GameManager.Instance.Worm = this;
            DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
        }

        private void OnDestroy()
        {
            DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<OnRoomChanged>(this);
        }

        private void OnRoomChanged(OnRoomChanged _obj)
        {
            if (m_roomCoroutines.TryGetValue(_obj.m_to.Coordinate, out var coroutine))
            {
                StopCoroutine(coroutine);
                m_roomCoroutines.Remove(_obj.m_to.Coordinate);
                EventDispatcher.SendEvent<OnRoomSavedFromWorm>(_obj.m_to);
            }
            
            if (DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear)
            {
                if (!m_wormAppeared)
                {
                    m_wormCoroutine = StartCoroutine(EatingWormCoroutine());
                }
            }
        }

        private IEnumerator EatingWormCoroutine()
        {
            while (DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear || m_roomCoroutines.Count > 0)
            {
                m_wormAppeared = true;
                Room roomToDelete = GetRoomAtTheEdge().FirstOrDefault(_room => !m_roomCoroutines.ContainsKey(_room.Coordinate));

                if (roomToDelete != null)
                {
                    if (m_roomCoroutines.TryAdd(roomToDelete.Coordinate,
                            StartCoroutine(DestroyRoomCoroutine(roomToDelete))))
                    {
                        EventDispatcher.SendEvent<OnWormStartEatingRoom>(roomToDelete);
                        DungeonRoomSystem.Instance.GetEventDispatcher().SendEvent<ForceRefreshMap>();
                    }
                }
                yield return new WaitForSeconds(eatingEveryTime);
            }
            m_wormCoroutine = null;
            m_wormAppeared = false;
            EventDispatcher.SendEvent<OnWormLeft>();
        }
        
        private IEnumerator DestroyRoomCoroutine(Room _roomToDestroy)
        {
            yield return new WaitForSeconds(timeUntilRoomDestroyed);
            m_roomCoroutines.Remove(_roomToDestroy.Coordinate);
            DungeonRoomSystem.Instance.CloseRoom(_roomToDestroy.Coordinate);
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