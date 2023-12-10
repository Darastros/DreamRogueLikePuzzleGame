#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

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

    
    // Events (static to please Becher)
    public delegate void GameRuleActivation();
    
    public static GameRuleActivation OnActivateRPGGame;
    public static GameRuleActivation OnDeactivateRPGGame;
    public static GameRuleActivation OnActivatePlatformerGame;
    public static GameRuleActivation OnDeactivatePlatformerGame;
    public static GameRuleActivation OnActivateCardGame;
    public static GameRuleActivation OnDeactivateCardGame;
    
    // Game rule variables
    private bool m_platformerActivated = false;
    public bool PlatformerActivated => m_platformerActivated;
    
    private bool m_cardGameActivated = false;
    public bool CardGameActivated => m_cardGameActivated;
    
    private bool m_rpgActivated = false;
    public bool RPGActivated => m_rpgActivated;
    
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }


    #region DebugEditor
    #if UNITY_EDITOR
    [MenuItem("GameManager/Switch Card Game")]
    public static void SwitchCardGame()
    {
        if (Application.isPlaying)
        {
            Instance.m_cardGameActivated = !Instance.m_cardGameActivated;
            if(Instance.m_cardGameActivated) OnActivateCardGame?.Invoke();
            else OnDeactivateCardGame?.Invoke();
        }
        else
        {
            EditorUtility.DisplayDialog("Are you ok?","You're trying to switch game rules while not in Play mod, are you drunk?", "Yes I am"); 
        }
    }
    #endif
    
   
    
    
#if UNITY_EDITOR
    [MenuItem("GameManager/Switch Platformer Game")]
    public static void SwitchPlatformerGame()
    {
        if (Application.isPlaying)
        {
            Instance.m_platformerActivated = !Instance.m_platformerActivated;
            if (Instance.m_platformerActivated) OnActivatePlatformerGame?.Invoke();
            else OnDeactivatePlatformerGame?.Invoke();
        }
        else
        {
            EditorUtility.DisplayDialog("Are you ok?","You're trying to switch game rules while not in Play mod, are you drunk?", "Yes I am"); 
        }
    }
#endif
    
#if UNITY_EDITOR
    [MenuItem("GameManager/Switch RPG Game")]
    public static void SwitchRPGGame()
    {
        if (Application.isPlaying)
        {
            Instance.m_rpgActivated = !Instance.m_rpgActivated;
            if (Instance.m_rpgActivated) OnActivateRPGGame?.Invoke();
            else OnDeactivateRPGGame?.Invoke();
        }
        else
        {
            EditorUtility.DisplayDialog("Are you ok?",
                "You're trying to switch game rules while not in Play mod, are you drunk?", "Yes I am");
        }
    }
#endif
    #endregion
    public delegate void GameFlow();

    public static GameFlow OnGamePause;
    public static GameFlow OnGameRestart;
    public static GameFlow OnGameResume;

    private static float m_timeFactor = 1.0f;
    public static float deltaTime => Time.deltaTime * m_timeFactor;

}
