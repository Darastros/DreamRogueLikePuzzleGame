using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ExtensionMethods
{
    public static T GetRandomElem<T>(this ICollection<T> _collection)
    {
        if (_collection == null || _collection.Count == 0)
            return default;
        
        var index = UnityEngine.Random.Range(0, _collection.Count);
        var element = _collection.ElementAt(index);
        return element;
    }
    
    public static IList<T> Shuffle<T>(this IList<T> _list)
    {
        int n = _list.Count;
        while (n > 1)
        {
            --n;
            int k = UnityEngine.Random.Range(0, n + 1);
            (_list[k], _list[n]) = (_list[n], _list[k]);
        }
        return _list;
    }
    
    
    public static RoomEntrance GetOpposite(this RoomEntrance _entrance)
    {
        return _entrance switch
        {
            RoomEntrance.North => RoomEntrance.South,
            RoomEntrance.South => RoomEntrance.North,
            RoomEntrance.East => RoomEntrance.West,
            RoomEntrance.West => RoomEntrance.East,
            _ => RoomEntrance.Invalid
        };
    }
    
    public static Vector2Int GetOffset(this RoomEntrance _entrance)
    {
        return _entrance switch
        {
            RoomEntrance.North => Vector2Int.up,
            RoomEntrance.South => Vector2Int.down,
            RoomEntrance.East => Vector2Int.right,
            RoomEntrance.West => Vector2Int.left,
            _ => Vector2Int.zero
        };
    }
    
    public static IEnumerable<Enum> GetFlags(this Enum e)
    {
        return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
    }

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> _dict, TKey _key) 
        where TValue : new()
    {
        if (_dict.TryGetValue(_key, out TValue val)) return val;
        val = new TValue();
        _dict.Add(_key, val);
        return val;
    }
    
    public static TValue GetOrCreate<TValue>(this GameObject _go) where TValue : Component
    {
        if (_go.TryGetComponent(out TValue component))
        {
            return component;
        }
        return _go.AddComponent<TValue>();
    }
    
    
    public static void DestroyChildren(this GameObject _go)
    {
        Transform transform = _go.transform;
        int i = 0;
    
        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            Object.Destroy(child);
        }
    }
    
    public static List<Room> GetInstanciatedNeighbors(this Room _room)
    {
        return DungeonRoomSystem.Instance.GetInstanciatedNeighbors(_room);
    }
    
    public static Room GetInstanciatedNeighbor(this Room _room, RoomEntrance _entrance)
    {
        return DungeonRoomSystem.Instance.CurrentRooms.GetValueOrDefault(_room.Coordinate + _entrance.GetOffset());
    }
    
    public static Vector2 ToVec2(this Vector3 _vector3)
    {
        return new Vector2(_vector3.x, _vector3.y);
    }

    public static bool TryGetComponentInParent<T>(this Component _component, out T _found)
    {
        _found = _component.GetComponentInParent<T>();
        return _found != null;
    }
}