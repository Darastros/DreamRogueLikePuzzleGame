using GameSystems;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Platformer
{
    public class PlatformerObject : MonoBehaviour, IEventListener
    {
        [SerializeField] public int strawberriesNumbers = 0;
        [SerializeField] public int lifePoints = 0;
        [SerializeField] public bool artifact = false;
        [SerializeField] public bool needNextRoomToBeActivated = true;
        
        [FormerlySerializedAs("maxDistancePerFixedTime")] [SerializeField] private float velocity = 5f;
        [FormerlySerializedAs("maxDistance")] [SerializeField] private float distanceWithPlayer = 1f;
        [SerializeField] private float distanceBetweenObjects = 0.75f;
        
        private Vector2 m_intitialPos = Vector2.zero;
        private bool pickedUp = false;

        
        private Animator m_animator;
        private static readonly int PickObjectUp = Animator.StringToHash("PickUp");

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            m_intitialPos = transform.position;
        }
        
        public void OnRoomChanged()
        {
            m_animator.SetTrigger(PickObjectUp);
        }

        public void PickUp(PlatformerController _who)
        {
            if(pickedUp)
                return;
            
            pickedUp = true;
            if(TryGetComponent(out Collider2D _collider))
            {
                _collider.enabled = false;
            }
            if (TryGetComponent(out Rigidbody2D _rigidbody))
            {
                _rigidbody.isKinematic = true;
            }

            if (needNextRoomToBeActivated)
            {
                if (_who.TryGetComponentInParent(out Rigidbody2D _rb))
                {
                    m_toFollow = _rb.transform;
                    offset = _who.ObjectsCollectedInTheRoom.Count - 1;
                }
            }
            else
            {
                m_animator.SetTrigger(PickObjectUp);
            }
        }

        private Vector2 _lastPos;
        private Transform m_toFollow = null;
        Vector2 _towards = Vector2.zero;
        private float offset = 0;
        private void FixedUpdate()
        {
            if (needNextRoomToBeActivated && m_toFollow != null && m_toFollow.TryGetComponent(out Rigidbody2D _rb))
            {
                if (_rb.velocity.sqrMagnitude > 5f)
                {
                    float dirX = _lastPos.x - _rb.position.x;
                    if (dirX != 0)
                    {
                        int y = _lastPos.x - _rb.position.x > 0 ? 1 : -1; 
                        _towards = new Vector2(y, 0);
                    }
                }
                var position = transform.position;
                position = Vector2.Lerp(position, _rb.position + _towards * (distanceWithPlayer + (offset * distanceBetweenObjects)), velocity * Time.fixedDeltaTime);
                transform.position = position;
                _lastPos = _rb.position;
            }
        }
        
        
        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void ResetPosition()
        {
            m_toFollow = null;
            pickedUp = false;
            transform.position = m_intitialPos;
            if(TryGetComponent(out Collider2D _collider))
            {
                _collider.enabled = true;
            }
        }
    }
}