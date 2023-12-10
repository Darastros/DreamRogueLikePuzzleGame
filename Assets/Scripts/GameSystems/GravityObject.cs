using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        if(GameManager.PlatformerActivated) ActivateGravity();
        else DeactivateGravity();
    }
    
    void OnEnable()
    {
        ListenEvent();
    }

    void OnDisable()
    {
        UnListenEvent();
    }
    
    private void ListenEvent()
    {
        GameManager.OnActivatePlatformerGame += ActivateGravity;
        GameManager.OnDeactivatePlatformerGame += DeactivateGravity;
    }

    private void UnListenEvent()
    {
        GameManager.OnActivatePlatformerGame -= ActivateGravity;
        GameManager.OnDeactivatePlatformerGame -= DeactivateGravity;
    }
    
    private void ActivateGravity()
    {
        m_rigidbody.isKinematic = false;
    }
    
    private void DeactivateGravity()
    {
        m_rigidbody.isKinematic = true;
        m_rigidbody.velocity = Vector2.zero;
    }
}
