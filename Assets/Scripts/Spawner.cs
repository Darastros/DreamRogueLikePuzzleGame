using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float m_spawnFrequency = 1.0f;
    [SerializeField] private GameObject m_spawnObject;
    private Animator m_animator;
    private float m_timer;
    
    void Awake()
    {
        m_timer = m_spawnFrequency;
        TryGetComponent(out m_animator);
    }

    void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0.0f)
        {
            
        }
    }

    public void Spawn()
    {
        
    }
}
