using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameRuleActivation();

    public static GameRuleActivation OnActivateRPGGame;
    public static GameRuleActivation OnDeactivateRPGGame;
    public static GameRuleActivation OnActivatePlatformerGame;
    public static GameRuleActivation OnDeactivatePlatformerGame;
    public static GameRuleActivation OnActivateCardGame;
    public static GameRuleActivation OnDeactivateCardGame;

    private static bool m_cardGameActivated = false;
    public static bool CardGameActivated => m_cardGameActivated;
    [MenuItem("GameManager/Switch Card Game")]
    public static void SwitchCardGame()
    {
        m_cardGameActivated = !m_cardGameActivated;
        if(m_cardGameActivated) OnActivateCardGame?.Invoke();
        else OnDeactivateCardGame?.Invoke();
    }
    
    private static bool m_platformerActivated = false;
    public static bool PlatformerActivated => m_platformerActivated;
    [MenuItem("GameManager/Switch Platformer Game")]
    public static void SwitchPlatformerGame()
    {
        m_platformerActivated = !m_platformerActivated;
        if(m_platformerActivated) OnActivatePlatformerGame?.Invoke();
        else OnDeactivatePlatformerGame?.Invoke();
    }
    
    private static bool m_rpgActivated = false;
    public static bool RPGActivated => m_rpgActivated;
    [MenuItem("GameManager/Switch RPG Game")]
    public static void SwitchRPGGame()
    {
        m_rpgActivated = !m_rpgActivated;
        if(m_rpgActivated) OnActivateRPGGame?.Invoke();
        else OnDeactivateRPGGame?.Invoke();
    }
    
    
    public delegate void GameFlow();

    public static GameFlow OnGamePause;
    public static GameFlow OnGameRestart;
    public static GameFlow OnGameResume;

    private static float m_timeFactor = 1.0f;
    public static float deltaTime => Time.deltaTime * m_timeFactor;

}
