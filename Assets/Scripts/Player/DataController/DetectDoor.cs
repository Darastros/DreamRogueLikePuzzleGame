using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class DetectDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out IUsable _usable))
        {
            _usable.Hover();
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out IUsable _usable))
        {
            _usable.Exit();
        }
    }
}
