using System.Collections.Generic;
using Platformer;
using RPG;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    private class Effect
    {
        public int number;
        public ItemType type;

        public Effect(int _number, ItemType _type)
        {
            number = _number;
            type = _type;
        }
    }
    private enum ItemType
    {
        Coin,
        Key,
        Life,
        Artifact,
        Strawberry
    }

    private List<Effect> m_effects;
    private float m_cooldown;
    [SerializeField] private Color m_greenColor;
    [SerializeField] private Color m_redColor;
    
    [SerializeField] private GameObject m_effectObject;
    void OnEnable()
    {
        RPGController.OnGetCoins += GetCoins;
        RPGController.OnGetKeys += GetKey;
        PlayerDataManager.OnHeal += Heal;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnUseArtifact += UseArtifact;
        PlayerDataManager.OnCollectArtifact += CollectArtifact;
        PlatformerController.OnGetStrawberries += GetStrawberries;
    }

    void OnDisable()
    {
        RPGController.OnGetCoins -= GetCoins;
        RPGController.OnGetKeys -= GetKey;
        PlayerDataManager.OnHeal -= Heal;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnUseArtifact -= UseArtifact;
        PlayerDataManager.OnCollectArtifact -= CollectArtifact;
        PlatformerController.OnGetStrawberries -= GetStrawberries;   
    }

    void Start()
    {
        m_effects = new List<Effect>();
    }

    void Update()
    {
        m_cooldown -= Time.deltaTime;
        if (m_cooldown <= 0.0f && m_effects.Count > 0)
        {
            Spawn(m_effects[0].number, m_effects[0].type);
            m_effects.RemoveAt(0);
        }
    }
    
    private void GetCoins(int _total, int _number)
    {
        RequestSpawn(_number, ItemType.Coin);
    }

    private void GetKey(int _total, int _number)
    {
        RequestSpawn(_number, ItemType.Key);
    }

    private void Heal(int _newvalue, int _delta)
    {
        RequestSpawn(_delta, ItemType.Life);
        
    }

    private void Hit(int _newvalue, int _delta)
    {
        RequestSpawn(_delta, ItemType.Life);
    }


    private void UseArtifact(int _newvalue, int _delta)
    {
        RequestSpawn(_delta, ItemType.Artifact);
    }

    private void CollectArtifact(int _newvalue, int _delta)
    {
        RequestSpawn(_delta, ItemType.Artifact);
    }

    private void GetStrawberries(int _total, int _number)
    {
        RequestSpawn(_number, ItemType.Strawberry);
        
    }

    private void RequestSpawn(int _number, ItemType _type)
    {
        m_effects.Add(new Effect(_number, _type));
    }
    private void Spawn(int _number, ItemType _type)
    {
        m_cooldown = 0.5f;
        var instance = Instantiate(m_effectObject, transform);
        var effect = instance.GetComponent<CollectItemEffect>();
        effect.m_coinSprite.gameObject.SetActive(_type == ItemType.Coin);
        effect.m_keySprite.gameObject.SetActive(_type == ItemType.Key);
        effect.m_healSprite.gameObject.SetActive(_type == ItemType.Life);
        effect.m_artifactSprite.gameObject.SetActive(_type == ItemType.Artifact);
        effect.m_strawberrySprite.gameObject.SetActive(_type == ItemType.Strawberry);
        effect.m_text.text = (_number > 0? "+":"") + _number;
        effect.m_text.color = _number > 0 ? m_greenColor : m_redColor;
        effect.m_textOutline.text = (_number > 0? "+":"") + _number;
        Debug.Log(_type + " " + _number);
        effect.GetComponent<Animator>().SetTrigger("Collect");
    }

}
