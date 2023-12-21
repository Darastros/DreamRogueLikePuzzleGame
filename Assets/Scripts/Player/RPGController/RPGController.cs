using UnityEngine;

namespace RPG
{
    public class RPGController : MonoBehaviour
    {
        [SerializeField] private RPGDetector m_detector;
        private int m_keys = 0;
        private int m_coins = 0;

        public delegate void Switch();
        public static Switch OnActivate;
        public static Switch OnDeactivate;

        public delegate void GetResources(int _total, int _number);
        public static GetResources OnGetCoins;
        public static GetResources OnGetKeys;

        void OnEnable()
        {
            ListenEvent();
            if(GameManager.Instance.RPGActivated) Activate();
            else Deactivate();
        }

        void OnDisable()
        {
            UnListenEvent();
        }

        public void Restart()
        {
            Deactivate();
        }

        void Start()
        {
            if(GameManager.Instance.RPGActivated) Activate();
            else Deactivate();
        }
        private void ListenEvent()
        {
            GameManager.OnActivateRPGGame += Activate;
            GameManager.OnDeactivateRPGGame += Deactivate;
            m_detector.OnRPGObjectEnter += EnterObject;
        }

        private void UnListenEvent()
        {
            GameManager.OnActivateRPGGame -= Activate;
            GameManager.OnDeactivateRPGGame -= Deactivate;
            m_detector.OnRPGObjectEnter += EnterObject;
        }
        
        private void EnterObject(RPGObject _object)
        {
            if ((_object.coinsNumbers < 0 && m_coins < -_object.coinsNumbers) || (_object.keysNumbers < 0 && m_keys < -_object.keysNumbers))
            {
                _object.Fail();
                return;
            }
            
            m_coins += _object.coinsNumbers;
            if (_object.coinsNumbers != 0) 
                OnGetCoins?.Invoke(m_coins, _object.coinsNumbers);
            
            m_keys += _object.keysNumbers;
            if (_object.keysNumbers != 0)
                OnGetKeys?.Invoke(m_keys, _object.keysNumbers);

            if (_object.lifePoints > 0) PlayerDataManager.life += _object.lifePoints;
            if (_object.artifact) ++PlayerDataManager.artifact;
            if (_object.portalPart) PlayerDataManager.rpgGameKeyPart = true;
            
            _object.PickUp();
            
        }

        private void Activate()
        {
            m_detector.gameObject.SetActive(true);
            OnActivate?.Invoke();
        }

        private void Deactivate()
        {
            m_detector.gameObject.SetActive(false);
            OnDeactivate?.Invoke();
            m_keys = 0;
            m_coins = 0;
        }
    }
}