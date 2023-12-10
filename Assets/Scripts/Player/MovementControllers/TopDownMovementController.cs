using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace MovementControllers
{
    public class TopDownMovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private float m_baseSpeed = 20f;
        [SerializeField] private float m_speedMultiplier = 1f;
        [SerializeField] private Rigidbody2D m_rigidbody2D;
     
        private Vector2 m_wantedDirection = Vector2.zero;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if(m_rigidbody2D == null)
                TryGetComponent(out m_rigidbody2D);
        }
        #endif
        
        private void Awake()
        {
            if(m_rigidbody2D == null)
                TryGetComponent(out m_rigidbody2D);
        }
        
        public void Move(Vector2 _wantedDirection)
        {
            m_wantedDirection = _wantedDirection;
        }

        public void Jump(bool _buttonPressed)
        {
            // Do nothing
        }

        private void FixedUpdate()
        {
            m_rigidbody2D.AddForce(m_wantedDirection * (m_baseSpeed * m_speedMultiplier));
        }
    }
}