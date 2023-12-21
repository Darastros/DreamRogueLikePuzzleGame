using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class RPGDetector : MonoBehaviour
    {
        private List<RPGObject> m_contactObjects;
        [SerializeField] private RPGObject m_currentObject;
        
        public delegate void RPGObjectUpdate(RPGObject _object);
        public RPGObjectUpdate OnRPGObjectEnter;
        public RPGObjectUpdate OnRPGObjectExit;
        private RPGObject currentObject
        {
            get => m_currentObject;
            set
            {
                if (m_currentObject) OnRPGObjectExit?.Invoke(m_currentObject);
                m_currentObject = value;
                if (m_currentObject) OnRPGObjectEnter?.Invoke(m_currentObject);
            }
        }
        
        private void Awake()
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
            m_contactObjects = new List<RPGObject>();
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out RPGObject _cardObject))
            {
                m_contactObjects.Add(_cardObject);
                currentObject = _cardObject;
            }
        }

        private void OnTriggerExit2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out RPGObject _cardObject))
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