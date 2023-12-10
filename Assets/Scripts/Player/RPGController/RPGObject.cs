using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class RPGObject : MonoBehaviour
    {
        [SerializeField] public int keysNumbers = 0;
        [SerializeField] public int coinsNumbers = 0;
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
            if (TryGetComponent(out Rigidbody2D _rigidbody))
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}