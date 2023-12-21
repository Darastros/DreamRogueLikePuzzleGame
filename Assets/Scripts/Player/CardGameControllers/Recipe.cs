using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private List<CardType> m_conditions;
        [SerializeField] private CraftCardResult m_result;

        public List<CardType> conditions => m_conditions;
        public CraftCardResult result => m_result;
    }

}
