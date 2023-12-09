using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Card/default")]
    public class Card : ScriptableObject
    {
        [SerializeField] private CardType m_type;
        public CardType Type => m_type;
    }
}