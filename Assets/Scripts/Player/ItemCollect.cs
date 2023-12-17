using System.Collections;
using System.Collections.Generic;
using Platformer;
using RPG;
using TMPro;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_text;
    [SerializeField] private TextMeshPro m_textOutline;
    [SerializeField] private Animator m_animator;
    void OnEnable()
    {
        RPGController.OnGetCoins += GetCoins;
        RPGController.OnGetKeys += GetKey;
        PlayerDataManager.OnHeal += Heal;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnUseArtifact += UseArtifact;
        PlayerDataManager.OnCollectArtifact += CollectArtifact;
        PlatformerController.OnGetStrawberries += GetStrawberries;
    }

    void OnDisable()
    {
        RPGController.OnGetCoins -= GetCoins;
        RPGController.OnGetKeys -= GetKey;
        PlayerDataManager.OnHeal -= Heal;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnUseArtifact -= UseArtifact;
        PlayerDataManager.OnCollectArtifact -= CollectArtifact;
        PlatformerController.OnGetStrawberries -= GetStrawberries;   
    }

    void DisableSpriteRenderer()
    {
        m_coinSprite.gameObject.SetActive(false);
        m_keySprite.gameObject.SetActive(false);
        m_healSprite.gameObject.SetActive(false);
        m_artifactSprite.gameObject.SetActive(false);
        m_strawberrySprite.gameObject.SetActive(false);

    }
    
    [SerializeField] private SpriteRenderer m_coinSprite;
    private void GetCoins(int _total, int _number)
    {
        
        m_text.text = (_number > 0? "+":"") + _number;
        m_textOutline.text = (_number > 0? "+":"") + _number;
        DisableSpriteRenderer();
        m_coinSprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
    }

    [SerializeField] private SpriteRenderer m_keySprite;
    private void GetKey(int _total, int _number)
    {
        m_text.text = (_number > 0? "+":"") + _number;
        m_textOutline.text = (_number > 0? "+":"") + _number;
        DisableSpriteRenderer();
        m_keySprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
    }

    [SerializeField] private SpriteRenderer m_healSprite;
    private void Heal(int _newvalue, int _delta)
    {
        m_text.text = (_delta > 0? "+":"") + _delta;
        m_textOutline.text = (_delta > 0? "+":"") + _delta;
        DisableSpriteRenderer();
        m_healSprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
        
    }

    private void Hit(int _newvalue, int _delta)
    {
        m_text.text = (_delta > 0? "+":"") + _delta;
        m_textOutline.text = (_delta > 0? "+":"") + _delta;
        DisableSpriteRenderer();
        m_healSprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
    }

    [SerializeField] private SpriteRenderer m_artifactSprite;
    private void UseArtifact(int _newvalue, int _delta)
    {
        m_text.text = (_delta > 0 ? "+" : "") + _delta;
        m_textOutline.text = (_delta > 0 ? "+" : "") + _delta;
        DisableSpriteRenderer();
        m_artifactSprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
    }

    private void CollectArtifact(int _newvalue, int _delta)
    {
        m_text.text = (_delta > 0? "+":"") + _delta;
        m_textOutline.text = (_delta > 0? "+":"") + _delta;
        DisableSpriteRenderer();
        m_artifactSprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
    }

    [SerializeField] private SpriteRenderer m_strawberrySprite;
    private void GetStrawberries(int _total, int _number)
    {
        m_text.text = (_number > 0? "+":"") + _number;
        m_textOutline.text = (_number > 0? "+":"") + _number;
        DisableSpriteRenderer();
        m_strawberrySprite.gameObject.SetActive(true);
        m_animator.SetTrigger("Collect");
        
    }

}
