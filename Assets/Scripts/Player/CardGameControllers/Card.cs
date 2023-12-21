using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Card/default")]
    public class Card : ScriptableObject
    {
        [SerializeField] private CardType m_type;
        public CardType type => m_type;
        [SerializeField] private Sprite m_sprite;
        public Sprite sprite => m_sprite;
    }
}