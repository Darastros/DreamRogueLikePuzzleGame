using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardGameController : MonoBehaviour
    {
        [SerializeField] private CardDetector m_detector;
        
        void Start()
        {
            m_detector.OnCardObjectEnter += EnterCard;
            m_detector.OnCardObjectExit += ExitCard;
            
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
        }
    }
}