using System;
using GameSystems;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.Worm
{
    public class WormTimerIndicator : MonoBehaviour, IEventListener
    {
        [SerializeField] private TextMeshPro[] m_text;
        private float m_endTimerTimeStamp = 0;

        private void OnEnable()
        {
            GameManager.Instance.Worm.EventDispatcher.RegisterEvent<OnWormStartEatingRoom>(this, OnWormStartEatingRoom);
            if (GameManager.Instance.Worm.RoomAboutToBeDestroyed != null)
            {
                OnWormStartEatingRoom(new OnWormStartEatingRoom(GameManager.Instance.Worm.RoomAboutToBeDestroyed));
            }
        }
        private void OnWormStartEatingRoom(OnWormStartEatingRoom _obj)
        {
            var worm = GameManager.Instance.Worm;
            m_endTimerTimeStamp = worm.TimeStampWormStartEatRoom + worm.timeUntilRoomDestroyed;
        }

        private void Update()
        {
            if (m_endTimerTimeStamp >= Time.time)
            {
                float timeLeft = m_endTimerTimeStamp - Time.time;
                foreach (var text in m_text)
                {
                    text.text = $"{timeLeft:F0}s";
                }
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.Worm != null)
            {
                GameManager.Instance.Worm.EventDispatcher.UnregisterAllEvents(this);
            }
        }
    }
}