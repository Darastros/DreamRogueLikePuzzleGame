using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class RPGObject : MonoBehaviour
    {
        [SerializeField] public int keyNumber = 0;
        [SerializeField] public int coinNumber = 0;
        [SerializeField] public bool artifact = false;
        [SerializeField] public bool portalPart = false;

        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }
        
        public void PickUp()
        {
            m_animator.SetTrigger("PickUp");
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}