using System;
using System.Collections.Generic;
using GameSystems;
using ScriptableObjects;
using UnityEngine;
using Utils;

namespace UI
{
    public class MapBuilder : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private UDictionary<RoomTypeUI, GameObject> m_roomUIPrefab = new UDictionary<RoomTypeUI, GameObject>();
        [SerializeField] private bool m_useCurrentRoomAsCenter = true;
        [SerializeField] private float m_offsetBetweenRoom = 0.1f;

        private void Start()
        {
            DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
            DungeonRoomSystem.Instance.GetEventDispatcher().RegisterEvent<ForceRefreshMap>(this, ForceRefreshMap);
        }

        private void OnDestroy()
        {
            DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<OnRoomChanged>(this);
            DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<ForceRefreshMap>(this);
        }

        private void OnRoomChanged(OnRoomChanged _obj)
        {
            RefreshMap();
        }
        void ForceRefreshMap(ForceRefreshMap _obj)
        {
            RefreshMap();
        }

        [ContextMenu("RefreshMap")]
        private void RefreshMap()
        {
            gameObject.DestroyChildren();
            
            var currentRoom = DungeonRoomSystem.Instance.CurrentRoom;
            var currentRooms = DungeonRoomSystem.Instance.CurrentRooms;

            HashSet<Vector2Int> neighborsAlreadyTreated = new HashSet<Vector2Int>();
            foreach (var (coordinate, room) in currentRooms)
            {
                GameObject prefabToUse = GetPrefabToUse(coordinate, currentRoom);
                Vector2Int offsetedCoordinate = m_useCurrentRoomAsCenter ? coordinate - currentRoom.Coordinate : coordinate;
                var instanciated = InstanciateRoom(prefabToUse, offsetedCoordinate);
                if(instanciated.TryGetComponent( out RoomUIRuntimeData runtimeData))
                {
                    runtimeData.Coordinate = coordinate;
                    runtimeData.AssociatedRoom = room;
                }
                
                foreach (Vector2Int roomNeighborsCoordinate in room.m_neighborsCoordinates)
                {
                    if (!currentRooms.ContainsKey(roomNeighborsCoordinate) && neighborsAlreadyTreated.Add(roomNeighborsCoordinate))
                    {
                        prefabToUse = GetPrefabToUse(roomNeighborsCoordinate, currentRoom);
                        offsetedCoordinate = m_useCurrentRoomAsCenter ? roomNeighborsCoordinate - currentRoom.Coordinate : roomNeighborsCoordinate;
                        instanciated = InstanciateRoom(prefabToUse, offsetedCoordinate);
                        if(instanciated.TryGetComponent( out runtimeData))
                        {
                            runtimeData.Coordinate = coordinate;
                            runtimeData.AssociatedRoom = null;
                        }
                    }
                }
            }
        }

        private GameObject InstanciateRoom(GameObject prefabToUse, Vector2Int coordinate)
        {
            var instanciated = Instantiate(prefabToUse, transform, false);
            if (instanciated.TryGetComponent(out RectTransform rectTransform))
            {
                instanciated.transform.localPosition = new Vector2(coordinate.x * rectTransform.rect.width + (coordinate.x * m_offsetBetweenRoom), coordinate.y * rectTransform.rect.height + (coordinate.y * m_offsetBetweenRoom));
            }
            instanciated.name = $"{instanciated.name}_{coordinate.x}_{coordinate.y}";
            return instanciated;
        }

        private GameObject GetPrefabToUse(Vector2Int _coordinate, Room _currentRoom)
        {
            GameObject prefabToUse = null;
            DungeonRoomSystem.Instance.CurrentRooms.TryGetValue(_coordinate, out Room room);
            if (_coordinate == _currentRoom.Coordinate)
            {
                m_roomUIPrefab.TryGetValue(RoomTypeUI.CurrentRoom, out prefabToUse);
            }
            else if (room != null)
            {
                if (room.m_roomDescriptor.m_registerToStartPool)
                {
                    m_roomUIPrefab.TryGetValue(RoomTypeUI.StartRoom, out prefabToUse);
                }
                else if (room.m_roomDescriptor.m_registerToExitPool)
                {
                    m_roomUIPrefab.TryGetValue(RoomTypeUI.ExitRoom, out prefabToUse);
                }
                else if (room.m_roomDescriptor.m_gameRuleType == GameRuleType.Platformer)
                {
                    m_roomUIPrefab.TryGetValue(RoomTypeUI.PlatformerRoom, out prefabToUse);
                }
                else if (room.m_roomDescriptor.m_gameRuleType == GameRuleType.RPG)
                {
                    m_roomUIPrefab.TryGetValue(RoomTypeUI.RPGRoom, out prefabToUse);
                }
                else if (room.m_roomDescriptor.m_gameRuleType == GameRuleType.CardGame)
                {
                    m_roomUIPrefab.TryGetValue(RoomTypeUI.CardGameRoom, out prefabToUse);
                } 
            }
            else
            {
                 m_roomUIPrefab.TryGetValue(RoomTypeUI.NotDiscoveredRoom, out prefabToUse);
            }
            if(prefabToUse == null)
            {
                m_roomUIPrefab.TryGetValue(RoomTypeUI.NeutralRoom, out prefabToUse);
            }
            return prefabToUse;
        }

        [Serializable]
        public enum RoomTypeUI : int
        {
            CurrentRoom,
            NotDiscoveredRoom,
            NeutralRoom,
            StartRoom,
            ExitRoom,
            RPGRoom,
            PlatformerRoom,
            CardGameRoom
        }
    }
}