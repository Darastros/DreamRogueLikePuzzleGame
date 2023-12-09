using System.Collections;
using System.Collections.Generic;
using MovementControllers;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private TopDownMovementController m_topDownController;
    [SerializeField] private PlatformerController m_platformerController;
    [SerializeField] private Rigidbody2D m_rigidbody2D;
    [SerializeField] private PlayerInput m_playerInput;

    private IMovementController m_currentMovementController;
    void OnEnable()
    {
        ListenEvent();
        DeactivatePlatformerGame();
    }

    void OnDisable()
    {
        UnListenEvent();
    }
    
    private void ListenEvent()
    {
        GameManager.OnActivatePlatformerGame += ActivatePlatformerGame;
        GameManager.OnDeactivatePlatformerGame += DeactivatePlatformerGame;
    }

    private void UnListenEvent()
    {
        GameManager.OnActivatePlatformerGame -= ActivatePlatformerGame;
        GameManager.OnDeactivatePlatformerGame -= DeactivatePlatformerGame;
    }
    private void ActivatePlatformerGame()
    {
        m_platformerController.enabled = true;
        m_topDownController.enabled = false;
        m_currentMovementController = m_platformerController;
        m_playerInput.SwitchCurrentActionMap("Platformer");
    }

    private void DeactivatePlatformerGame()
    {
        m_platformerController.enabled = false;
        m_topDownController.enabled = true;
        m_currentMovementController = m_topDownController;
        m_playerInput.SwitchCurrentActionMap("TopDown");
        
    }
    
    public void Move(Vector2 _value)
    {
        m_currentMovementController.Move(_value);
    }
    
    public void Jump(bool _value)
    {
        m_currentMovementController.Jump(_value);
    }

}
