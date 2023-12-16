﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        [SerializeField] private float timeUntilRoomDestroyed = 30f;
        [SerializeField] private float newTimerIfRoomSaved = 120f;
        
        private bool m_wormAppeared = false;
        private Coroutine m_wormCoroutine = null;

        public Room RoomAboutToBeDestroyed { get; private set; } = null;

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
            if (m_wormCoroutine != null && RoomAboutToBeDestroyed.Coordinate == _obj.m_to.Coordinate)
            {
                StopCoroutine(m_wormCoroutine);
                m_wormCoroutine = null;
                RoomAboutToBeDestroyed = null;
                EventDispatcher.SendEvent<OnRoomSavedFromWorm>(_obj.m_to);
                StartCoroutine(Reset());
            }
            
            if (DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear)
            {
                if (!m_wormAppeared)
                {
                    m_wormAppeared = true;
                    m_wormCoroutine = StartCoroutine(DestroyRoomCoroutine(GetRoomAtTheEdge()[0]));
                }
            }
        }
        
        private IEnumerator DestroyRoomCoroutine(Room _roomToDestroy)
        {
            RoomAboutToBeDestroyed = _roomToDestroy;
            DungeonRoomSystem.Instance.GetEventDispatcher().SendEvent<ForceRefreshMap>();
            yield return new WaitForSeconds(timeUntilRoomDestroyed);
            EventDispatcher.SendEvent<OnWormStartEatingRoom>(_roomToDestroy);
            DungeonRoomSystem.Instance.CloseRoom(_roomToDestroy.Coordinate);
            m_wormCoroutine = null;
            RoomAboutToBeDestroyed = null;
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(newTimerIfRoomSaved);
            if (DungeonRoomSystem.Instance.CurrentRooms.Count >= minRoomThresholdForWormToAppear)
            {
                m_wormCoroutine = StartCoroutine(DestroyRoomCoroutine(GetRoomAtTheEdge()[0]));
            }
            else
            {
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

        private List<Room> GetRoomAtTheEdgeV2()
        {
            return null;
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