using System.Collections.Generic;
using System.Linq;
using GameSystems;

public static class Utils
{
    public static T GetRandomElem<T>(this ICollection<T> _collection)
    {
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
}