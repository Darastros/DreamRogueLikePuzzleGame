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
        public int posInHand => m_posInHand;
        void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 0.1f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 0.1f);
        }
        
        public void InitDataCard(Card _card)
        {
            m_data = _card;
            m_posInHand = 1;
        }

        public void MoveRight()
        {
            ++m_posInHand;
            if(m_posInHand > 3) m_animator.SetTrigger("Reset");
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
