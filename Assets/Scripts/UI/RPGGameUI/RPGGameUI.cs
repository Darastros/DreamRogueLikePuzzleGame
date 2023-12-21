using RPG;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RPGGameUI : MonoBehaviour
    {
        private Animator m_animator;
        [SerializeField] private TextMeshProUGUI m_keysText;
        [SerializeField] private TextMeshProUGUI m_coinsText;

        void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        void OnEnable()
        {
            ListenEvent();
            Deactivate();
        }

        void OnDisable()
        {
            UnListenEvent();
        }

        private void ListenEvent()
        {
            RPGController.OnActivate += Activate;
            RPGController.OnDeactivate += Deactivate;
            RPGController.OnGetCoins += GetCoins;
            RPGController.OnGetKeys += GetKeys;
        }

        private void UnListenEvent()
        {
            RPGController.OnActivate -= Activate;
            RPGController.OnDeactivate -= Deactivate;
            RPGController.OnGetCoins -= GetCoins;
            RPGController.OnGetKeys -= GetKeys;
        }

        private void GetCoins(int _total, int _number)
        {
            m_coinsText.text = (_total <= 9 ? "0" : "") + _total;
        }

        private void GetKeys(int _total, int _number)
        {
            m_keysText.text = (_total <= 9 ? "0" : "") + _total;
        }

        private void Activate()
        {
            m_keysText.text = "00";
            m_coinsText.text = "00";
            m_animator.SetBool("activate", true);
        }

        private void Deactivate()
        {
            m_animator.SetBool("activate", false);
        }
    }
}