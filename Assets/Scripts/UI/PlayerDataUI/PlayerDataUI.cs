using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField] private List<Image> m_hearts;
    [SerializeField] private TextMeshProUGUI m_artifactNumberText;
    [SerializeField] private Sprite m_heartSpriteFull;
    [SerializeField] private Sprite m_heartSpriteEmpty;
    [SerializeField] private Animator m_RPGKeyPart;
    [SerializeField] private Animator m_platformerKeyPart;
    [SerializeField] private Animator m_cardGameKeyPart;
    private void OnEnable()
    {
        PlayerDataManager.OnHeal += Heal;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnActivateKeyPart += ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact += CollectArtifact;
        PlayerDataManager.OnUseArtifact += UseArtifact;
    }
    
    private void OnDisable()
    {
        PlayerDataManager.OnHeal -= Heal;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnActivateKeyPart -= ActivateKeyPart;
    }

    private void ActivateKeyPart(GameRuleType _part)
    {
        switch (_part)
        {
            case GameRuleType.RPG:
                m_RPGKeyPart.SetTrigger("Activate");
                break;
            case GameRuleType.CardGame:
                m_cardGameKeyPart.SetTrigger("Activate");
                break;
            case GameRuleType.Platformer:
                m_platformerKeyPart.SetTrigger("Activate");
                break;
            default:
                Debug.LogError($"Cannot activate Key part {_part}");
                break;
        }
    }
    
    private void Hit(int _newValue, int _delta)
    {
        UpdateLifeBar(_newValue);
    }

    private void Heal(int _newValue, int _delta)
    {
        UpdateLifeBar(_newValue);
    }

    private void UpdateLifeBar(int _newValue)
    {
        for (int i = 0; i < m_hearts.Count; ++i)
        {
            if (i < _newValue)
            {
                m_hearts[i].sprite = m_heartSpriteFull;
            }
            else m_hearts[i].sprite = m_heartSpriteEmpty;
        }
    }
    

    private void CollectArtifact(int _newValue, int _delta)
    {
        m_artifactNumberText.text = (_newValue <= 9 ? "0" : "") + _newValue;
    }

    private void UseArtifact(int _newValue, int _delta)
    {
        m_artifactNumberText.text = (_newValue <= 9 ? "0" : "") + _newValue;
    }

}
