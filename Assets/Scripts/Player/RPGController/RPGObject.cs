using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("renderer")] [SerializeField] public SpriteRenderer spriteRenderer;
        [SerializeField] public Sprite outlineSprite;
        private Sprite activateSprite;
        [SerializeField] public int keysNumbers = 0;
        [SerializeField] public int coinsNumbers = 0;
        [SerializeField] public int lifePoints = 0;
        [SerializeField] public bool artifact = false;
        [SerializeField] public bool portalPart = false;

        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            if (!outlineSprite) return;
            activateSprite = spriteRenderer.sprite;
            spriteRenderer.sprite = outlineSprite;
        }
        
        public void PickUp()
        {
            m_animator.SetTrigger("PickUp");
            if (TryGetComponent(out Rigidbody2D _rigidbody))
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void OnEnable()
        { 
            if (!outlineSprite) return;
            GameManager.OnActivateRPGGame += ActivateRPG;
            GameManager.OnDeactivateRPGGame += DeactivateRPG;
            if(GameManager.Instance.RPGActivated) ActivateRPG();
            else DeactivateRPG();
        }

        public void OnDisable()
        {
            if (!outlineSprite) return;
            GameManager.OnActivateRPGGame -= ActivateRPG;
            GameManager.OnDeactivateRPGGame -= DeactivateRPG;
        }

        private void ActivateRPG()
        {
            spriteRenderer.sprite = activateSprite;
        }

        private void DeactivateRPG()
        {
            spriteRenderer.sprite = outlineSprite;
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