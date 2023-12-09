using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Card/default")]
    public class CraftCardResult : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_sprite;
        [SerializeField] private bool m_worldItem = false;
        [SerializeField] private GameObject m_object;
    }
}
