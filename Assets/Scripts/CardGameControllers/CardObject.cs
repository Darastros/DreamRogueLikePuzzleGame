using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardObject : MonoBehaviour
    {
        [SerializeField] private Card m_card;
        
        private Action<Card> m_callback;
        public CardType Type => m_card.Type;

        public void StartAbsorb(Action<Card> _callback)
        {
            m_callback = _callback;
        }

        public void StopAbsorb()
        {
            m_callback = null;
        }

        private void Absorb()
        {
            m_callback?.Invoke(m_card);
        }
    }
}