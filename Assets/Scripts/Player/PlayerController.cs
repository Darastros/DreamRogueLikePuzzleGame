using System.Collections;
using System.Collections.Generic;
using CardGame;
using MovementControllers;
using Platformer;
using RPG;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    [SerializeField] private PlatformerController m_platformerController;
    [SerializeField] private RPGController m_rpgController;
    [SerializeField] private CardGameController m_cardGameController;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        ListenEvent();
    }

    void OnDisable()
    {
        UnListenEvent();
    }

    private void ListenEvent()
    {
        GameManager.OnGameLoose += Loose;
        GameManager.OnGameWin += Win;
        GameManager.OnGameRestart += Restart;
    }

    private void UnListenEvent()
    {
        GameManager.OnGameLoose -= Loose;
        GameManager.OnGameWin -= Win;
        GameManager.OnGameRestart -= Restart;
    }

    private void Loose()
    {
        m_rigidbody.bodyType = RigidbodyType2D.Static;
    }

    private void Win()
    {
        m_rigidbody.bodyType = RigidbodyType2D.Static;
    }

    private void Restart()
    {
        m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        m_rigidbody.velocity = Vector2.zero;
    }
}
