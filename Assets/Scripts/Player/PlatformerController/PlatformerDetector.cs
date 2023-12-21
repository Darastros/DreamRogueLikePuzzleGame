using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformerDetector : MonoBehaviour
    {
        private List<PlatformerObject> m_contactObjects;
        [SerializeField] private PlatformerObject m_currentObject;
        
        public delegate void PlatformerObjectUpdate(PlatformerObject _object);
        public PlatformerObjectUpdate OnPlatformerObjectEnter;
        public PlatformerObjectUpdate OnPlatformerObjectExit;
        private PlatformerObject currentObject
        {
            get => m_currentObject;
            set
            {
                if (m_currentObject) OnPlatformerObjectExit?.Invoke(m_currentObject);
                m_currentObject = value;
                if (m_currentObject) OnPlatformerObjectEnter?.Invoke(m_currentObject);
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
            m_contactObjects = new List<PlatformerObject>();
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out PlatformerObject _cardObject))
            {
                m_contactObjects.Add(_cardObject);
                currentObject = _cardObject;
            }
        }

        private void OnTriggerExit2D(Collider2D _other)
        {
            if (_other.TryGetComponent(out PlatformerObject _cardObject))
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