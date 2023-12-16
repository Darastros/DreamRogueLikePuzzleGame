using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class DetectDoor : MonoBehaviour
{
    private IUsable m_usable;
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out IUsable _usable))
        {
            _usable.Hover();
            m_usable = _usable;
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out IUsable _usable))
        {
            _usable.Exit();
            if(m_usable == _usable) m_usable = null;
        }
    }

    public void Use()
    {
        if (m_usable != null)
        {
            m_usable.Use(gameObject);
        }
    }
}
