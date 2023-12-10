using System.Collections;
using System.Collections.Generic;
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
            if(GameManager.RPGActivated) Activate();
            else Deactivate();
        }

        void OnDisable()
        {
            UnListenEvent();
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
            m_coins += _object.coinNumber;
            if (_object.coinNumber > 0) OnGetCoins?.Invoke(m_coins, _object.coinNumber);
            m_keys += _object.keyNumber;
            if (_object.keyNumber > 0) OnGetKeys?.Invoke(m_keys, _object.keyNumber);
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