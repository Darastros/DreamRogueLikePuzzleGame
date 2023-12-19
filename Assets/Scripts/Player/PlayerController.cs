using System.Collections;
using System.Collections.Generic;
using CardGame;
using DG.Tweening;
using GameSystems;
using MovementControllers;
using Platformer;
using Player;
using RPG;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    
    [SerializeField] private Transform m_spriteParent;
    [SerializeField] private PlayerDataManager m_dataManager;
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
        m_platformerController.platformerMovementController.Jumped += Jump;
        PlatformerController.OnActivate += SwitchPlatformer;
        PlatformerController.OnDeactivate += SwitchTopDown;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnDeath += Death;
        ExitPortal.OnCrossPortal += EnterPortal;
    }

    private void UnListenEvent()
    {
        GameManager.OnGameLoose -= Loose;
        GameManager.OnGameWin -= Win;
        GameManager.OnGameRestart -= Restart;
        m_platformerController.platformerMovementController.Jumped -= Jump;
        PlatformerController.OnActivate -= SwitchPlatformer;
        PlatformerController.OnDeactivate -= SwitchTopDown;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnDeath -= Death;
        ExitPortal.OnCrossPortal -= EnterPortal;
    }

    void Update()
    {
        UpdateMovement();
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
        m_platformerController.Restart();
        m_rpgController.Restart();
        m_cardGameController.Restart();
        m_dataManager.Restart();
        m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        m_rigidbody.velocity = Vector2.zero;
    }
    
    private void UpdateMovement()
    {
        if(m_rigidbody.velocity.x > 0.6f) m_spriteParent.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else if(m_rigidbody.velocity.x < -0.6f) m_spriteParent.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        
        m_animator.SetFloat("moveX", InputManager.instance.moveInput.x);
        m_animator.SetFloat("moveY", InputManager.instance.moveInput.y);
        m_animator.SetBool("isOnGround", m_platformerController.IsOnGround());
    }

    private void Jump()
    {
        m_animator.ResetTrigger("Jump");
        m_animator.ResetTrigger("JumpStretch");
        m_animator.SetTrigger("Jump");
        m_animator.SetTrigger("JumpStretch");
    }

    private void Hit(int _newValue, int _delta)
    {
        m_animator.ResetTrigger("Hit");
        m_animator.ResetTrigger("HitStretch");
        m_animator.SetTrigger("Hit");
        m_animator.SetTrigger("HitStretch");
    }

    private void Death()
    {
        m_animator.SetTrigger("Death");
    }

    private void SwitchPlatformer()
    {
        m_animator.ResetTrigger("Jump");
        m_animator.ResetTrigger("JumpStretch");
        m_animator.ResetTrigger("Hit");
        m_animator.ResetTrigger("HitStretch");
        m_animator.SetBool("platformer",true);
    }

    private void SwitchTopDown()
    {
        m_animator.ResetTrigger("Jump");
        m_animator.ResetTrigger("JumpStretch");
        m_animator.ResetTrigger("Hit");
        m_animator.ResetTrigger("HitStretch");
        m_animator.SetBool("platformer",false);
        
    }

    public void EndHit()
    {
        if (GameManager.Instance.PlatformerActivated)
        {
            GameManager.Instance.TeleportPlayerToRoomEntrance(DungeonRoomSystem.Instance.LastDoorOpened);
        }
    }
    
    private void EnterPortal(Vector3 _center)
    {
        transform.position = _center;
        m_animator.SetTrigger("EnterPortal");
    }

}
