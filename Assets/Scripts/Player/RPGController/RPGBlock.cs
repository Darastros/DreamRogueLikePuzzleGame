using UnityEngine;

public class RPGBlock : MonoBehaviour
{
    private Collider2D m_collider2D;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Sprite outlineSprite;
    private Sprite activateSprite;

    private void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
        if (!outlineSprite) return;
        activateSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = outlineSprite;
    }

    public void OnEnable()
    { 
        GameManager.OnActivateRPGGame += ActivateRPG;
        GameManager.OnDeactivateRPGGame += DeactivateRPG;
        
        if(GameManager.Instance.RPGActivated) ActivateRPG();
        else DeactivateRPG();
    }

    public void OnDisable()
    {
        GameManager.OnActivateRPGGame -= ActivateRPG;
        GameManager.OnDeactivateRPGGame -= DeactivateRPG;
    }

    private void ActivateRPG()
    {
        m_collider2D.enabled = true;
        spriteRenderer.sprite = activateSprite;
    }
    private void DeactivateRPG()
    {
        m_collider2D.enabled = false;
        spriteRenderer.sprite = outlineSprite;
    }
}
