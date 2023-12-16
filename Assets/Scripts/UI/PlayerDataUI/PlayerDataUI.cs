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
        PlayerDataManager.OnDeath += Death;
        PlayerDataManager.OnActivateKeyPart += ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact += CollectArtifact;
        PlayerDataManager.OnUseArtifact += UseArtifact;

        GameManager.OnActivateCardGame += AddCardGameRules;
        GameManager.OnDeactivateCardGame += RemoveCardGameRules;
        GameManager.OnActivatePlatformerGame += AddPlatformerGameRules;
        GameManager.OnDeactivatePlatformerGame += RemovePlatformerGameRules;
        GameManager.OnActivateRPGGame += AddRPGGameRules;
        GameManager.OnDeactivateRPGGame += RemoveRPGGameRules;

        m_activeRules = new List<Animator>();
        AddStartGameRules();
    }

    private void OnDisable()
    {
        PlayerDataManager.OnHeal -= Heal;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnDeath -= Death;
        PlayerDataManager.OnActivateKeyPart -= ActivateKeyPart;
        
        GameManager.OnActivateCardGame -= AddCardGameRules;
        GameManager.OnDeactivateCardGame -= RemoveCardGameRules;
        GameManager.OnActivatePlatformerGame -= AddPlatformerGameRules;
        GameManager.OnDeactivatePlatformerGame -= RemovePlatformerGameRules;
        GameManager.OnActivateRPGGame -= AddRPGGameRules;
        GameManager.OnDeactivateRPGGame -= RemoveRPGGameRules;
    }

    private void FixedUpdate()
    {
        UpdateRulesPos();
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

    private void Death()
    {
        UpdateLifeBar(0);
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
    
    [Header("Game Rule")]
    [SerializeField] private List<Transform> m_rulesPos;
    
    private void UpdateRulesPos()
    {
        int pos = 0;
        for (int i = m_activeRules.Count - 1; i >= 0; --i)
        {
            Vector3 requirePos = GetRuleRequirePos(m_activeRules[i].transform, pos);
            m_activeRules[i].transform.position = Vector3.Lerp(m_activeRules[i].transform.position, requirePos, 0.1f);
            ++pos;
        }
    }

    private Vector3 GetRuleRequirePos(Transform _rule, int _pos)
    {
        Vector3 requirePos = _rule.position;
        requirePos.y = m_rulesPos[_pos].position.y;
        return requirePos;
    }

    private void ActivateRule(Animator _rule)
    {
        _rule.SetTrigger("Activate");
        m_activeRules.Add(_rule);
        _rule.transform.position = GetRuleRequirePos(_rule.transform, 0);
    }
    
    private void DeactivateRule(Animator _rule)
    {
        _rule.SetTrigger("Deactivate");
        m_activeRules.Remove(_rule);
    }

    
    [SerializeField] private Animator m_startGameRulesExplain;
    private List<Animator> m_activeRules;
    private void AddStartGameRules()
    {
        ActivateRule(m_startGameRulesExplain);
    }

    private void RemoveStartGameRules()
    {
        DeactivateRule(m_startGameRulesExplain);
    }

    [SerializeField] private Animator m_cardGameRulesExplain;
    private void AddCardGameRules()
    {
        ActivateRule(m_cardGameRulesExplain);
    }

    private void RemoveCardGameRules()
    {
        DeactivateRule(m_cardGameRulesExplain);
    }

    [SerializeField] private Animator m_paltformerRulesExplain;
    private void AddPlatformerGameRules()
    {
        ActivateRule(m_paltformerRulesExplain);
    }

    private void RemovePlatformerGameRules()
    {
        DeactivateRule(m_paltformerRulesExplain);
    }
    
    [SerializeField] private Animator m_rpgRulesExplain;
    private void AddRPGGameRules()
    {
        ActivateRule(m_rpgRulesExplain);
    }

    private void RemoveRPGGameRules()
    {
        DeactivateRule(m_rpgRulesExplain);
    }

}
