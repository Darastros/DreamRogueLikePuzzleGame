using GameSystems;
using UnityEngine;

namespace ScriptableObjects
{
    public class RoomUIRuntimeData : MonoBehaviour
    {
        public Room AssociatedRoom { get; set; }
        public Vector2Int Coordinate { get; set; }
    }
}