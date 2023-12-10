using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("HurtfulObject"))
        {
            --PlayerDataManager.life;
        }
    }
}
