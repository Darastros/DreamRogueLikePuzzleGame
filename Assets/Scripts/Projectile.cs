using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        GetComponent<Animator>().SetTrigger("Destroy");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
