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
    
    void OnEnable()
    {
        DungeonRoomSystem.Instance.GetEventDispatcher()
            .RegisterEvent<OnRoomChanged>(this, OnRoomChanged);

        GameManager.OnGameRestart += Reset;

        PlayerDataManager.OnCollectArtifact += OnCollectArtifact;
    }

    private void OnDisable()
    {
        DungeonRoomSystem.Instance.GetEventDispatcher()
            .RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
        
        GameManager.OnGameRestart -= Reset;
        
        PlayerDataManager.OnCollectArtifact -= OnCollectArtifact;
    }

    private void Reset()
    {
        m_nbRoom = 0;
    }

    public int m_nbRoom = 0;
    public bool m_enableArtifact = false;
    private void OnRoomChanged(OnRoomChanged obj)
    {
        ++m_nbRoom;
        if (m_nbRoom >= 5 && !m_enableArtifact)
        {
            m_artifactTutorial.Appear();
            m_enableArtifact = true;
        }
    }
    
    private void OnCollectArtifact(int _newvalue, int _delta)
    {
        if (!m_enableArtifact)
        {
            m_artifactTutorial.Appear();
            m_enableArtifact = true;
        }
    }
}
