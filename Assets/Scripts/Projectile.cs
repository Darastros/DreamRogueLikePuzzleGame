using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        GetComponent<Animator>().SetTrigger("Destroy");
        if(TryGetComponent(out Rigidbody2D _rigidbody)) _rigidbody.velocity = Vector2.zero;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
