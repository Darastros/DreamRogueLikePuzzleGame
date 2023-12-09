using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

namespace UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private Card m_data;
        private Animator m_animator;
        private int m_posInHand = 1;
        
        void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void InitDataCard(Card _card)
        {
            m_data = _card;
            m_posInHand = 1;
            m_animator.SetInteger("cardPos", m_posInHand);
        }

        public void MoveRight()
        {
            ++m_posInHand;
            m_animator.SetInteger("cardPos", m_posInHand);
        }
        
        public void CraftSuccess()
        {
            m_animator.SetTrigger("Success");
        }
        public void CraftFailed()
        {
            m_animator.SetTrigger("Fail");
        }

        public void Reset()
        {
            m_animator.SetTrigger("Reset");
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }

       
    }

}
