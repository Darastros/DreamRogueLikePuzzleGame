﻿using System;
using Utils;

namespace GameSystems
{
    [Serializable]
    public class OnPlayerOpenDoor : Event
    {
        public RoomEntrance entrance;

        public OnPlayerOpenDoor(RoomEntrance _entrance)
        {
            entrance = _entrance;
        }
    }
    
    
    [Serializable]
    public class OnRoomChanged : Event
    {
        public Room m_from;
        public Room m_to;
        public OnRoomChanged(Room _from, Room _to)
        {
            m_from = _from;
            m_to = _to;
        }
        public OnRoomChanged()
        {}
    }
    
    [Serializable]
    public class ForceRefreshMap : Event
    {
        public ForceRefreshMap()
        {}
    }
}