using System.Collections;
using System.Collections.Generic;
using Platformer;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlatformerGameUI : MonoBehaviour
    {
        private Animator m_animator;
        [SerializeField] private TextMeshProUGUI m_strawberriesText;

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
            PlatformerController.OnActivate += Activate;
            PlatformerController.OnDeactivate += Deactivate;
            PlatformerController.OnGetStrawberries += GetStrawberries;
        }

        private void UnListenEvent()
        {
            PlatformerController.OnActivate -= Activate;
            PlatformerController.OnDeactivate -= Deactivate;
            PlatformerController.OnGetStrawberries -= GetStrawberries;
        }

        private void GetStrawberries(int _total, int _number)
        {
            m_strawberriesText.text = (_total <= 9 ? "0" : "") + _total + " / 10";
        }
        private void Activate()
        {
            m_strawberriesText.text = "00 / 10";
            m_animator.SetBool("activate", true);
        }

        private void Deactivate()
        {
            m_animator.SetBool("activate", false);
        }
    }
}