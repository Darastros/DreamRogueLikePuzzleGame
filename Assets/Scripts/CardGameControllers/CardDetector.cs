using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardDetector : MonoBehaviour
    {
        private List<CardObject> m_contactObjects;
        [SerializeField] private CardObject m_currentObject;
        
        public delegate void CardObjectUpdate(CardObject _card);
        public CardObjectUpdate OnCardObjectEnter;
        public CardObjectUpdate OnCardObjectExit;
        private CardObject currentObject
        {
            get => m_currentObject;
            set
            {
                if (m_currentObject) OnCardObjectExit?.Invoke(m_currentObject);
                m_currentObject = value;
                if (m_currentObject) OnCardObjectEnter?.Invoke(m_currentObject);
            }
        }
        
        private void Start()
        {
            ResetDetector();
        }
        private void OnEnable()
        {
            ResetDetector();
        }
        
        private void ResetDetector()
        {
            currentObject = null;
            m_contactObjects = new List<CardObject>();
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out CardObject _cardObject))
            {
                m_contactObjects.Add(_cardObject);
                if (!currentObject)
                {
                    currentObject = _cardObject;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out CardObject _cardObject))
            {
                m_contactObjects.Remove(_cardObject);
                if (currentObject == _cardObject)
                {
                    currentObject = m_contactObjects.Count > 0 ? m_contactObjects[0] : null;
                }
            }
        }
    }
}
