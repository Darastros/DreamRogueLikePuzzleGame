using System;
using System.Collections;
using System.Collections.Generic;
using CardGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CardGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_cardObject;
        [SerializeField] private List<Transform> m_pos;
        [SerializeField] private Image m_successSprite;
        [SerializeField] private TextMeshProUGUI m_successText;
        private List<CardUI> m_cards;
        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void OnEnable()
        {
            m_cards = new List<CardUI>();
            CardGameController.OnGettingCard += AddCard;
            CardGameController.OnCraftSuccess += CraftSuccess;
            CardGameController.OnCraftFailed += CraftFailed;
            CardGameController.OnReset += Reset;
        }

        private void OnDisable()
        {
            CardGameController.OnGettingCard -= AddCard;
            CardGameController.OnCraftSuccess -= CraftSuccess;
            CardGameController.OnCraftFailed -= CraftFailed;
            CardGameController.OnReset -= Reset;
            
        }
        
        private void AddCard(Card _card)
        {
            m_cards.RemoveAll(x => x == null);
            foreach (var card in m_cards)
            {
                card.MoveRight();
                card.transform.SetParent(m_pos[card.posInHand]);
            }

            var cardObject = Instantiate(m_cardObject, m_pos[0]);
            cardObject.transform.SetParent(m_pos[1]);
            var cardUI = cardObject.GetComponent<CardUI>();
            cardUI.InitDataCard(_card);
            m_cards.Add(cardUI);
        }

        private CraftCardResult m_result;
        private Vector3 m_resultPos;
        private void CraftSuccess(CraftCardResult _result, Vector3 _position)
        {
            m_result = _result;
            m_resultPos = _position;
            m_animator.SetTrigger("Success");
            m_successSprite.sprite = _result.sprite;
            m_successText.text = _result.Name;
            m_cards.RemoveAll(x => x == null);
            foreach (var card in m_cards)
            {
                card.CraftSuccess();
            }
        }
        private void CraftFailed()
        {
            m_animator.SetTrigger("Fail");
            m_cards.RemoveAll(x => x == null);
            foreach (var card in m_cards)
            {
                card.CraftFailed();
            }
        } 
        private void Reset()
        {
            m_animator.SetTrigger("Reset");
        }
        private void ResetHand()
        {
            m_cards.RemoveAll(x => x == null);
            foreach (var card in m_cards)
            {
                card.Reset();
            }
        }

        public void Apply()
        {
            if(m_result) m_result.Apply(m_resultPos);
        }
    }
}