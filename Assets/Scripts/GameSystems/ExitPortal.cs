using System;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{

    public Animator _animator;
    public Animator _debugFeedBackRPG;
    public Animator _debugFeedBackCard;
    public Animator _debugFeedBackPlatformer;
    
    public delegate void VictoryEventDelegate();
    public static VictoryEventDelegate OnCrossPortal;
    
    private void OnEnable()
    {
        PlayerDataManager.OnActivateKeyPart += OnKeyPartCollected;
        if (_debugFeedBackCard != null)
        {
            _debugFeedBackCard.SetBool("active", PlayerDataManager.cardGameKeyPart);
        }
        
        if (_debugFeedBackRPG != null)
        {
            _debugFeedBackRPG.SetBool("active", PlayerDataManager.rpgGameKeyPart);
        }
        
        if (_debugFeedBackPlatformer != null)
        {
            _debugFeedBackPlatformer.SetBool("active", PlayerDataManager.platformerGameKeyPart);
        }
        _animator.SetBool("active", PlayerDataManager.platformerGameKeyPart && PlayerDataManager.rpgGameKeyPart && PlayerDataManager.cardGameKeyPart);
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
                if(_debugFeedBackPlatformer != null) _debugFeedBackPlatformer.SetBool("active", true);
                break;
            case GameRuleType.RPG:
                if (_debugFeedBackRPG != null) _debugFeedBackRPG.SetBool("active", true);
                break;
            case GameRuleType.CardGame:
                if (_debugFeedBackCard != null) _debugFeedBackCard.SetBool("active", true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_gameRuleType), _gameRuleType, null);
        }
        _animator.SetBool("active", PlayerDataManager.platformerGameKeyPart && PlayerDataManager.rpgGameKeyPart && PlayerDataManager.cardGameKeyPart);
    }
}
