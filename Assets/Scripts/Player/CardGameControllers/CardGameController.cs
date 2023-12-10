using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardGameController : MonoBehaviour
    {
        [SerializeField] private CardDetector m_detector;
        [SerializeField] private int m_maxCardsInHand = 3;
        [SerializeField] private List<Recipe> m_recipes;
        [SerializeField] private List<Card> m_hand;
        
        private List<long> m_masks;
        
        public delegate void GetCard(Card _card);
        public static GetCard OnGettingCard;

        public delegate void Reset();
        public static Reset OnReset;
        public static Reset OnCraftFailed;

        public delegate void Result(CraftCardResult _result);
        public static Result OnCraftSuccess;

        void Awake()
        {
            m_masks = new(){0b100010001, 0b100001010, 0b010100001, 0b001100010, 0b001010100, 0b010001100};
        }
        
        void OnEnable()
        {
            ListenEvent();
            m_hand = new List<Card>();
            if(GameManager.CardGameActivated) Activate();
            else Deactivate();
        }

        void OnDisable()
        {
            UnListenEvent();
        }

        void Start()
        {
            if(GameManager.CardGameActivated) Activate();
            else Deactivate();
        }

        private void ListenEvent()
        {
            m_detector.OnCardObjectEnter += EnterCard;
            m_detector.OnCardObjectExit += ExitCard;
            GameManager.OnActivateCardGame += Activate;
            GameManager.OnDeactivateCardGame += Deactivate;
        }

        private void UnListenEvent()
        {
            m_detector.OnCardObjectEnter -= EnterCard;
            m_detector.OnCardObjectExit -= ExitCard;
            GameManager.OnActivateCardGame -= Activate;
            GameManager.OnDeactivateCardGame -= Deactivate;
        }
        
        private void Activate()
        {
            m_detector.gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            m_detector.gameObject.SetActive(false);
            OnReset?.Invoke();
            m_hand = new List<Card>();
        }
        
        private void EnterCard(CardObject _card)
        {
            //Debug.Log("Enter " + _card.Type);
            _card.StartAbsorb(AbsorbCard);
        }
        
        private void ExitCard(CardObject _card)
        {
            //Debug.Log("Exit " + _card.Type);
            _card.StopAbsorb();
        }
        
        void AbsorbCard(Card _card)
        {
            //Debug.Log("Absorb card " + _card.Type);
            OnGettingCard?.Invoke(_card);
            m_hand.Add(_card);
            if(m_hand.Count > m_maxCardsInHand) m_hand.RemoveAt(0);
        }

        public void Craft()
        {
            if (m_hand.Count == m_maxCardsInHand)
            {
                //Debug.Log("Try Craft");
                var result = FindCraftPlan();
                if(result)
                {
                    //Debug.Log("Craft : " + result.name);
                    OnCraftSuccess?.Invoke(result);
                    result.Spawn(transform.position);
                }
                else OnCraftFailed?.Invoke();
                
                m_hand = new List<Card>();
            }
        }

        public CraftCardResult FindCraftPlan()
        {
            foreach (var recipe in m_recipes)
            {
                long result = 0;
                for (int y = 0; y < m_maxCardsInHand; ++y)
                {
                    for (int x = 0; x < m_maxCardsInHand; ++x)
                    {
                        if((m_hand[x].Type & recipe.conditions[y]) == m_hand[x].Type) result += 1 << y * m_maxCardsInHand + x;
                    }
                }
                foreach (long mask in m_masks)
                {
                    if((mask & result) == mask)
                        return recipe.result;
                }
            }
            return null;
        }
    }
}