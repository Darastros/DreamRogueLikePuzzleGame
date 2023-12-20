using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class TutorialManager : MonoBehaviour, IEventListener
{
    [SerializeField] private TutorialAnimator m_artifactTutorial;
    [SerializeField] private TutorialAnimator m_platformerTutorial;
    [SerializeField] private TutorialAnimator m_rpgTutorial;
    [SerializeField] private TutorialAnimator m_cardGameTutorial;
    [SerializeField] private TutorialAnimator m_wormTutorial;
    
    void OnEnable()
    {
        GameManager.OnActivateRPGGame += OnActivateRPGGame;
        GameManager.OnActivateCardGame += OnActivateCardGame;
        GameManager.OnActivatePlatformerGame += OnActivatePlatformerGame;
        GameManager.OnGameRestart += Restart;
        PlayerDataManager.OnCollectArtifact += OnCollectArtifact;
        Worm.OnWormStartEatingRoom += OnWormStartEatingRoom;
        
        m_artifactTutorial.gameObject.SetActive(false);
        m_platformerTutorial.gameObject.SetActive(false);
        m_rpgTutorial.gameObject.SetActive(false);
        m_cardGameTutorial.gameObject.SetActive(false);
        m_wormTutorial.gameObject.SetActive(false);
        
        DungeonRoomSystem.EventDispatcher?.RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
    }
    private void OnDisable()
    {
        GameManager.OnActivateRPGGame -= OnActivateRPGGame;
        GameManager.OnActivateCardGame -= OnActivateCardGame;
        GameManager.OnActivatePlatformerGame -= OnActivatePlatformerGame;
        GameManager.OnGameRestart -= Restart;
        PlayerDataManager.OnCollectArtifact -= OnCollectArtifact;
        Worm.OnWormStartEatingRoom -= OnWormStartEatingRoom;
        
        DungeonRoomSystem.EventDispatcher?.RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
    }

    private void Restart()
    {
        m_artifactTutorial.gameObject.SetActive(false);
        m_platformerTutorial.gameObject.SetActive(false);
        m_rpgTutorial.gameObject.SetActive(false);
        m_cardGameTutorial.gameObject.SetActive(false);
        m_wormTutorial.gameObject.SetActive(false);
        
        m_enableRPG = false;
        m_enableCardGame = false;
        m_enablePlatformer = false;
        m_enableArtifact = false;
        m_enableWorm = false;
        m_nbRoom = 0;
        
    }

    private bool m_enableRPG = false;
    private void OnActivateRPGGame()
    {
        if (!m_enableRPG)
        {
            m_rpgTutorial.gameObject.SetActive(true);
            m_enableRPG = true;
            m_rpgTutorial.Appear();
        }
    }

    private bool m_enableCardGame = false;
    private void OnActivateCardGame()
    {
        if (!m_enableCardGame)
        {
            m_cardGameTutorial.gameObject.SetActive(true);
            m_enableCardGame = true;
            m_cardGameTutorial.Appear();
        }
    }

    private bool m_enablePlatformer = false;
    private void OnActivatePlatformerGame()
    {
        if (!m_enablePlatformer)
        {
            m_platformerTutorial.gameObject.SetActive(true);
            m_enablePlatformer = true;
            m_platformerTutorial.Appear();
        }
    }
    
    private int m_nbRoom = 0;
    private bool m_enableArtifact = false;
    private void OnRoomChanged(OnRoomChanged obj)
    {
        ++m_nbRoom;
        if (m_nbRoom >= 5 && !m_enableArtifact)
        {
            m_artifactTutorial.gameObject.SetActive(true);
            m_artifactTutorial.Appear();
            m_enableArtifact = true;
        }
    }
    
    private void OnCollectArtifact(int _newvalue, int _delta)
    {
        if (!m_enableArtifact)
        {
            m_artifactTutorial.gameObject.SetActive(true);
            m_artifactTutorial.Appear();
            m_enableArtifact = true;
        }
    }
    

    private bool m_enableWorm = false;
    private void OnWormStartEatingRoom()
    {
        Debug.Log("Worm start eating");
        if (!m_enableWorm)
        {
            m_wormTutorial.gameObject.SetActive(true);
            m_wormTutorial.Appear();
            m_enableWorm = true;
        }
    }

}
