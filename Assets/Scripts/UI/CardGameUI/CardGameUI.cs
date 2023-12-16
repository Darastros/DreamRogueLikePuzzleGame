using System;
using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

namespace UI
{
    public class CardGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_cardObject;
        [SerializeField] private List<Transform> m_pos;
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
            CardGameController.OnReset += ResetHand;
        }

        private void OnDisable()
        {
            CardGameController.OnGettingCard -= AddCard;
            CardGameController.OnCraftSuccess -= CraftSuccess;
            CardGameController.OnCraftFailed -= CraftFailed;
            CardGameController.OnReset -= ResetHand;
            
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
        
        private void CraftSuccess(CraftCardResult _result)
        {
            m_animator.SetTrigger("Success");
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
        private void ResetHand()
        {
            m_animator.SetTrigger("Reset");
            m_cards.RemoveAll(x => x == null);
            foreach (var card in m_cards)
            {
                card.Reset();
            }
        }
    }
}