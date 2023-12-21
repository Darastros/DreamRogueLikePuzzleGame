using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private int m_contactNumbers = 0;
    [SerializeField] private float m_invulnerabilityDuration = 0.2f;
    private float m_invulnerabilityTimer = 0.0f;
    
    private int contactNumbers
    {
        get => m_contactNumbers;
        set
        {
            if(m_contactNumbers == 0 && value > 0 && m_invulnerabilityTimer <= 0.0f)
                --PlayerDataManager.life;
            if (value == 0) m_invulnerabilityTimer = m_invulnerabilityDuration;
            
            m_contactNumbers = value;
        }
    }

    private void Update()
    {
        m_invulnerabilityTimer -= GameManager.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("HurtfulObject"))
        {
            contactNumbers++;
        }
    }
    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("HurtfulObject"))
        {
            contactNumbers--;
        }
    }

}
