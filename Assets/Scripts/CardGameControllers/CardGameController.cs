using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardGameController : MonoBehaviour
    {
        [SerializeField] private CardDetector m_detector;
        [SerializeField] private int m_maxCardsInHand = 5;
        [SerializeField] private List<Card> m_hand;
        
        public delegate void GetCard(Card _card);
        public static GetCard OnGettingCard;

        public delegate void Reset();
        public static Reset OnReset;
        public static Reset OnFailCraft;

        public delegate void Result(CraftCardResult _result);
        public static Result OnCraftSuccess;
        
        void OnEnable()
        {
            ListenEvent();
            m_hand = new List<Card>();
        }

        void OnDisable()
        {
            UnListenEvent();
        }

        private void ListenEvent()
        {
            m_detector.OnCardObjectEnter += EnterCard;
            m_detector.OnCardObjectExit += ExitCard;
        }
        
        private void UnListenEvent()
        {
            m_detector.OnCardObjectEnter -= EnterCard;
            m_detector.OnCardObjectExit -= ExitCard;
        }
        
        private void EnterCard(CardObject _card)
        {
            Debug.Log("Enter " + _card.Type);
            _card.StartAbsorb(AbsorbCard);
        }
        
        private void ExitCard(CardObject _card)
        {
            Debug.Log("Exit " + _card.Type);
            _card.StopAbsorb();
        }
        
        void AbsorbCard(Card _card)
        {
            Debug.Log("Absorb card " + _card.Type);
            OnGettingCard?.Invoke(_card);
            m_hand.Add(_card);
            if(m_hand.Count > m_maxCardsInHand) m_hand.RemoveAt(0);
        }

        public void Craft()
        {
            if (m_hand.Count == m_maxCardsInHand)
            {
                Debug.Log("Try Craft");
                var result = FindCraftPlan();
                if(result) OnCraftSuccess?.Invoke(result);
                else OnFailCraft?.Invoke();
                
                m_hand = new List<Card>();
            }
        }

        public CraftCardResult FindCraftPlan()
        {
            return null;
        }
    }
}