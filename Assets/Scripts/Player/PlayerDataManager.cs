using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public delegate void SimpleEventDelegate();
    public delegate void ValueChangeEventDelegate(int _newValue, int _delta);
    
    #region Singleton
    private static PlayerDataManager m_instance;
    public static PlayerDataManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<PlayerDataManager>();
            }
            return m_instance;
        }
    }
    #endregion
    
    #region Life
    [SerializeField] private int m_maxLife = 5;
    private int m_life;

    public static int life
    {
        get => instance.m_life;
        set
        {
            int clampValue = math.clamp(value, 0, instance.m_maxLife);
            int delta = value - instance.m_life;
            
            if (delta == 0) return;
             if (delta > 0)
             {
                 OnHeal?.Invoke(clampValue, delta);
             }
             else
             {
                 if (clampValue == 0) OnDeath?.Invoke();
                 else OnHit?.Invoke(clampValue, delta);
             }
             instance.m_life = clampValue;
        }
    }

    public static bool isDead => life <= 0;
    public static ValueChangeEventDelegate OnHit;
    public static ValueChangeEventDelegate OnHeal;
    public static SimpleEventDelegate OnDeath;
    #endregion // Life

    #region Artifact
    private int m_artifact = 1;

    public static int artifact
    {
        get => instance.m_artifact;
        set
        {;
            int delta = value - instance.m_artifact;
            
            if (delta == 0) return;
            if (delta > 0)
            {
                OnCollectArtifact?.Invoke(value, delta);
            }
            else
            {
                OnUseArtifact?.Invoke(value, delta);
            }
            instance.m_artifact = value;
        }
    }

    public static bool TryUseArtifact()
    {
        if (instance.m_artifact > 0)
        {
            --instance.m_artifact;
            return true;
        }

        return false;
    }
    
    public static ValueChangeEventDelegate OnUseArtifact;
    public static ValueChangeEventDelegate OnCollectArtifact;
    #endregion // Artifact
    
    #region KeyPart

    public delegate void ActivateKeyPartEventDelegate(string _part);
    public static ActivateKeyPartEventDelegate OnActivateKeyPart;
    private bool m_RPGGameKeyPart = false;
    private bool m_platformerGameKeyPart = false;
    private bool m_cardGameKeyPart = false;

    public static bool rpgGameKeyPart
    {
        get => instance.m_RPGGameKeyPart;
        set
        {
            if (!value || instance.m_RPGGameKeyPart) return;
            OnActivateKeyPart?.Invoke("RPG");
            instance.m_RPGGameKeyPart = value;
        }
    }
    public static bool platformerGameKeyPart
    {
        get => instance.m_platformerGameKeyPart;
        set
        {
            if (!value || instance.m_platformerGameKeyPart) return;
            OnActivateKeyPart?.Invoke("Platformer");
            instance.m_platformerGameKeyPart = value;
        }
    }
    public static bool cardGameKeyPart
    {
        get => instance.m_cardGameKeyPart;
        set
        {
            if (!value || instance.m_cardGameKeyPart) return;
            OnActivateKeyPart?.Invoke("Card");
            instance.m_cardGameKeyPart = value;
        }
    }
    #endregion


    public void Awake()
    {
        m_life = m_maxLife;
    }
}
