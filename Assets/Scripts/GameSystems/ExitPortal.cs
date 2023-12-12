using System;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{

    public SpriteRenderer _debugFeedBackRPG;
    public SpriteRenderer _debugFeedBackCard;
    public SpriteRenderer _debugFeedBackPlatformer;
    
    public delegate void VictoryEventDelegate();
    public static VictoryEventDelegate OnCrossPortal;
    
    private void OnEnable()
    {
        PlayerDataManager.OnActivateKeyPart += OnKeyPartCollected;
        if (_debugFeedBackCard != null)
        {
            _debugFeedBackCard.color = PlayerDataManager.cardGameKeyPart ? _debugFeedBackCard.color = Color.green : Color.red;
        }
        
        if (_debugFeedBackRPG != null)
        {
            _debugFeedBackRPG.color = PlayerDataManager.rpgGameKeyPart ? _debugFeedBackRPG.color = Color.green : Color.red;
        }
        
        if (_debugFeedBackPlatformer != null)
        {
            _debugFeedBackPlatformer.color = PlayerDataManager.platformerGameKeyPart ? _debugFeedBackPlatformer.color = Color.green : Color.red;
        }
    }

    private void OnDisable()
    {
        PlayerDataManager.OnActivateKeyPart -= OnKeyPartCollected;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerDataManager.cardGameKeyPart && PlayerDataManager.platformerGameKeyPart &&
                PlayerDataManager.rpgGameKeyPart)
            {
                OnCrossPortal?.Invoke();
            }
        }
    }
    
    [ContextMenu("GIVE RPG")]
    public void GiveRPGKey()
    {
        PlayerDataManager.rpgGameKeyPart = true;
    }
    [ContextMenu("GIVE Card")]
    public void GiveCardKey()
    {
        PlayerDataManager.cardGameKeyPart = true;
    }
    
    [ContextMenu("GIVE Platformer")]
    public void GivePlatformerKey()
    {
        PlayerDataManager.platformerGameKeyPart = true;
    }
    private void OnKeyPartCollected(GameRuleType _gameRuleType)
    {
        switch (_gameRuleType)
        {
            case GameRuleType.Platformer:
                if(_debugFeedBackPlatformer != null) _debugFeedBackPlatformer.color = Color.green;
                break;
            case GameRuleType.RPG:
                if (_debugFeedBackRPG != null) _debugFeedBackRPG.color = Color.green;
                break;
            case GameRuleType.CardGame:
                if (_debugFeedBackCard != null) _debugFeedBackCard.color = Color.green;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_gameRuleType), _gameRuleType, null);
        }
    }
}
