using System.Collections;
using System.Collections.Generic;
using GameSystems;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Result/default")]
    public class CraftCardResult : ScriptableObject
    {
        [SerializeField] private string m_name;
        public string name => m_name;
        [SerializeField] private Sprite m_sprite;
        public Sprite sprite => m_sprite;
        [SerializeField] private bool m_worldItem = false;
        [SerializeField] private GameObject m_object;

        public virtual void Apply(Vector3 _position)
        {
            if(m_worldItem && m_object)
            {
                
                var instance = Instantiate(m_object, _position, Quaternion.identity);
                instance.transform.SetParent(DungeonRoomSystem.Instance.CurrentRoom.m_runtimeGameScene.transform);
            }
        }
    }
}
