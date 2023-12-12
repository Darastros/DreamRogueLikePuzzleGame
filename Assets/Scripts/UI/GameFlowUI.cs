using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowUI : MonoBehaviour
{
    private Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        GameManager.OnGameLoose += GameLoose;
        GameManager.OnGameWin += GameWin;
        GameManager.OnGameRestart += GameRestart;
    }
    
    private void OnDisable()
    {
        GameManager.OnGameLoose -= GameLoose;
        GameManager.OnGameWin -= GameWin;
        GameManager.OnGameRestart -= GameRestart;
    }

    private void GameRestart()
    {
        m_animator.SetTrigger("Reset");   
    }

    private void GameWin()
    {
        m_animator.SetTrigger("Win");
    }

    private void GameLoose()
    {
        m_animator.SetTrigger("Loose");
    }
}
