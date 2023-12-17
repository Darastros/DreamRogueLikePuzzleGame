using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class RPGObject : MonoBehaviour
    {
        
        // Events
        public delegate void PickUpItem();
    
        public static PickUpItem OnBuyItem;
        public static PickUpItem OnFailBuyItem;
        public static PickUpItem OnOpenChest;
        public static PickUpItem OnFailOpenChest;

        
        [SerializeField] public int keysNumbers = 0;
        [SerializeField] public int coinsNumbers = 0;
        [SerializeField] public int lifePoints = 0;
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

        public void Fail()
        {
            m_animator.SetTrigger("Fail");
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void OpenChest()
        {
            OnOpenChest?.Invoke();
        }

        public void FailOpenChest()
        {
            OnFailOpenChest?.Invoke();
        }
        
        public void BuyItem()
        {
            OnBuyItem?.Invoke();
        }

        public void FailBuyItem()
        {
            OnFailBuyItem?.Invoke();
        }
    }
}