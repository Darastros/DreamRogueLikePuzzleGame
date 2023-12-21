using System.Collections.Generic;
using GameSystems;
using UnityEngine;
using Utils;

public class TutorialManager : MonoBehaviour, IEventListener
{
    [SerializeField] private TutorialAnimator m_artifactTutorial;
    [SerializeField] private TutorialAnimator m_platformerTutorial;
    [SerializeField] private TutorialAnimator m_rpgTutorial;
    [SerializeField] private TutorialAnimator m_cardGameTutorial;
    [SerializeField] private TutorialAnimator m_wormTutorial;
    [SerializeField] private TutorialAnimator m_portalTutorial;

    private List<TutorialAnimator> m_tutorialsToDraw;
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
        Restart();
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
        m_enablePortal = false;
        m_nbRoom = 0;
        m_tutorialsToDraw = new List<TutorialAnimator>();
    }

    private bool m_enableRPG = false;
    private void OnActivateRPGGame()
    {
        if (!m_enableRPG)
        {
            m_tutorialsToDraw.Add(m_rpgTutorial);
            m_enableRPG = true;
        }
    }

    private bool m_enableCardGame = false;
    private void OnActivateCardGame()
    {
        if (!m_enableCardGame)
        {
            m_tutorialsToDraw.Add(m_cardGameTutorial);
            m_enableCardGame = true;
        }
    }

    private bool m_enablePlatformer = false;
    private void OnActivatePlatformerGame()
    {
        if (!m_enablePlatformer)
        {
            m_tutorialsToDraw.Add(m_platformerTutorial);
            m_enablePlatformer = true;
        }
    }
    
    private int m_nbRoom = 0;
    private bool m_enableArtifact = false;
    private bool m_enablePortal = false;
    private void OnRoomChanged(OnRoomChanged obj)
    {
        ++m_nbRoom;
        if (m_nbRoom >= 5 && !m_enableArtifact)
        {
            m_tutorialsToDraw.Add(m_artifactTutorial);
            m_enableArtifact = true;
        }
        if(m_nbRoom == 2 && !m_enablePortal)
        {
            m_tutorialsToDraw.Add(m_portalTutorial);
            m_enablePortal = true;
            
        }
    }
    
    private void OnCollectArtifact(int _newvalue, int _delta)
    {
        if (!m_enableArtifact)
        {
            m_tutorialsToDraw.Add(m_artifactTutorial);
            m_enableArtifact = true;
        }
    }
    

    private bool m_enableWorm = false;
    private void OnWormStartEatingRoom()
    {
        Debug.Log("Worm start eating");
        if (!m_enableWorm)
        {
            m_tutorialsToDraw.Add(m_wormTutorial);
            m_enableWorm = true;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.gamePaused && m_tutorialsToDraw.Count > 0)
        {
            m_tutorialsToDraw[0].gameObject.SetActive(true);
            m_tutorialsToDraw[0].Appear();
            m_tutorialsToDraw.RemoveAt(0);
        }
    }

}
