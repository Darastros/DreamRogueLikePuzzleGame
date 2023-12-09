using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardObject : MonoBehaviour
    {
        [SerializeField] private Card m_card;
        private Animator m_animator;
        
        private Action<Card> m_callback;
        public CardType Type => m_card.Type;

        public void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void StartAbsorb(Action<Card> _callback)
        {
            m_callback = _callback;
            m_animator.SetBool("Absorb", true);
        }

        public void StopAbsorb()
        {
            m_callback = null;
            m_animator.SetBool("Absorb", false);
        }

        private void Absorb()
        {
            m_callback?.Invoke(m_card);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}