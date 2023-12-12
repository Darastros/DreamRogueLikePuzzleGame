#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using Unity.Mathematics;
using UnityEngine;


public enum GameRuleType
{
    None,
    Platformer,
    RPG,
    CardGame
}

public class GameManager : MonoBehaviour
{
    
    //Singleton pattern
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            GameObject go = new GameObject();
            _instance = go.AddComponent<GameManager>();
            return _instance;
        }
        private set => _instance = value;
    }
    private static GameManager _instance;

    
    // Events
    public delegate void GameRuleActivation();
    
    public static GameRuleActivation OnActivateRPGGame;
    public static GameRuleActivation OnDeactivateRPGGame;
    
    public static GameRuleActivation OnActivatePlatformerGame;
    public static GameRuleActivation OnDeactivatePlatformerGame;
    
    public static GameRuleActivation OnActivateCardGame;
    public static GameRuleActivation OnDeactivateCardGame;
    
    // Game rule variables
    private uint m_platformerRuleStack = 0;
    public bool PlatformerActivated => m_platformerRuleStack > 0;
    
    
    private uint m_cardGameRuleStack = 0;
    public bool CardGameActivated => m_cardGameRuleStack > 0;
    
   
    private uint m_RPGRuleStack = 0;
    public bool RPGActivated => m_RPGRuleStack > 0;
    
    // Internal
    public delegate void Callback();

    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }
    
    #region GameRules
    public void AddRPGGameRuleToStack()
    {
        ++m_RPGRuleStack;
        if (m_RPGRuleStack == 1)
        {
            OnActivateRPGGame.Invoke();
        }
    }

    public void RemoveRPGGameRuleFromStack()
    {
        m_RPGRuleStack = math.max(0, m_RPGRuleStack - 1);
        if (m_RPGRuleStack == 0)
        {
            OnDeactivateRPGGame.Invoke();
        }
    }

    public void AddPlatformerGameToStack()
    {
        ++m_platformerRuleStack;
        if (m_platformerRuleStack == 1)
        {
            OnActivatePlatformerGame.Invoke();
        }
    }

    public void RemovePlatformerGameFromStack()
    {
        m_platformerRuleStack = math.max(0, m_platformerRuleStack - 1);
        if (m_platformerRuleStack == 0)
        {
            OnDeactivatePlatformerGame.Invoke();
        }
    }

    public void AddCardGameToStack()
    {
        ++m_cardGameRuleStack;
        if (m_cardGameRuleStack == 1)
        {
            OnActivateCardGame.Invoke();
        }
    }

    public void RemoveCardGameFromStack()
    {
        m_cardGameRuleStack = math.max(0, m_cardGameRuleStack - 1);
        if (m_cardGameRuleStack == 0)
        {
            OnDeactivateCardGame.Invoke();
        }
    }
    #endregion
    
    #region Game flow
    public delegate void GameFlow();

    public static GameFlow OnGamePause;
    public static GameFlow OnGameRestart;
    public static GameFlow OnGameResume;
    public static GameFlow OnGameWin;
    public static GameFlow OnGameLoose;

    private bool m_gameStart = true;
    private bool m_gamePaused = false;

    private void OnEnable()
    {
        PlayerDataManager.OnDeath += LooseGame;
        ExitPortal.OnCrossPortal += WinGame;
        m_gameStart = true;
    }


    private void OnDisable()
    {
        PlayerDataManager.OnDeath -= LooseGame;
        ExitPortal.OnCrossPortal -= WinGame;
    }

    private void WinGame()
    {
        OnGameWin?.Invoke();
        m_gameStart = false;
    }

    private void LooseGame()
    {
        OnGameLoose?.Invoke();
        m_gameStart = false;
    }

    public void Restart()
    {
        OnGameRestart?.Invoke();
        m_gameStart = true;
    }

    public void Pause()
    {
        if (m_gameStart && !m_gamePaused)
        {
            m_gamePaused = true;
            OnGamePause?.Invoke();
        }
    }

    public void Resume()
    {
        if (m_gameStart && m_gamePaused)
        {
            m_gamePaused = false;
            OnGameResume?.Invoke();
        }
    }

    private static float m_timeFactor = 1.0f;
    public static float timeFactor => m_timeFactor;
    public static float deltaTime => Time.deltaTime * m_timeFactor;
    public static float fixedDeltaTime => Time.fixedDeltaTime * m_timeFactor;
    
    #endregion

    #region DebugEditor
    #if UNITY_EDITOR

    private static void ExecuteLambdaIfApplicationPlaying(Callback _function, string _dialogTitle = "Are you ok?", string _errorMessage = "You're trying to switch game rules while not in Play mod, are you drunk?", string _validate = "Yes I am")
    {
        if (Application.isPlaying)
        {
            _function.Invoke();
        }
        else
        {
            EditorUtility.DisplayDialog(_dialogTitle,_errorMessage, _validate); 
        }
    }

    [MenuItem("GameManager/CardGame/Add Card Game to stack")]
    public static void ForceAddCardGameRuleToStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.AddCardGameToStack());
    
    [MenuItem("GameManager/CardGame/Remove Card Game from stack")]
    public static void ForceRemoveCardGameRuleFromStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.RemoveCardGameFromStack());
    
    [MenuItem("GameManager/Platformer/Add Platformer Rule to stack")]
    public static void ForceAddPlatformerGameRuleToStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.AddPlatformerGameToStack());
    
    [MenuItem("GameManager/Platformer/Remove Platformer Rule from stack")]
    public static void ForceRemovePlatformerGameRuleFromStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.RemovePlatformerGameFromStack());
    
    [MenuItem("GameManager/RPG/Add RPG System to stack")]
    public static void ForceAddRpgGameRuleToStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.AddRPGGameRuleToStack());
    
    [MenuItem("GameManager/CardGame/Remove Card Game from stack")]
    public static void ForceRemoveRpgGameRuleFromStack() => ExecuteLambdaIfApplicationPlaying(() => Instance.RemoveRPGGameRuleFromStack());
    
    [MenuItem("GameManager/Hit player")]
    public static void HitPlayer()
    {
        --PlayerDataManager.life;
    }
    #endif
    #endregion
}
