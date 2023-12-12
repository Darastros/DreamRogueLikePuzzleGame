using System;
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
}