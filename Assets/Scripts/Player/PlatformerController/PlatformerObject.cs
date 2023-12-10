using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformerObject : MonoBehaviour
    {
        [SerializeField] public int strawberriesNumbers = 0;
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
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}