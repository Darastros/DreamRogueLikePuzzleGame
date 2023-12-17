using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float m_spawnFrequency = 1.0f;
    [SerializeField] private GameObject m_spawnObject;
    [SerializeField] private float m_speed = 10.0f;
    private Animator m_animator;
    private float m_timer;
    
    
    public delegate void SpawnDelegate();
    public static SpawnDelegate OnSpawn;
    
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
            if (m_animator)
            {
                m_animator.SetTrigger("Spawn");
            }
            else Spawn();
            m_timer = m_spawnFrequency;
        }
    }

    public void Spawn()
    {
        //OnSpawn?.Invoke();
        var instance = Instantiate(m_spawnObject, transform.position, transform.rotation);
        if (instance.TryGetComponent(out Rigidbody2D _rigidbody))
        {
            _rigidbody.velocity = transform.right * m_speed;
        }
    }
}
