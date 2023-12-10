using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField] private List<Image> m_hearts;
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
    }

    private void OnDisable()
    {
        PlayerDataManager.OnHeal -= Heal;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnActivateKeyPart -= ActivateKeyPart;
    }

    private void ActivateKeyPart(string _part)
    {
        if(_part == "RPG") m_RPGKeyPart.SetTrigger("Activate");
        else if(_part == "Card") m_cardGameKeyPart.SetTrigger("Activate");
        else m_platformerKeyPart.SetTrigger("Activate");
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
}
