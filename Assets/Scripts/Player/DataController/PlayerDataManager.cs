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
    [SerializeField] private int m_artifactAtStart = 1;
    private int m_life;

    public static int life
    {
        get => instance.m_life;
        set
        {
            if (isDead) return;
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
        if (artifact > 0)
        {
            --artifact;
            return true;
        }

        return false;
    }
    
    public static ValueChangeEventDelegate OnUseArtifact;
    public static ValueChangeEventDelegate OnCollectArtifact;
    #endregion // Artifact
    
    #region KeyPart

    public delegate void ActivateKeyPartEventDelegate(GameRuleType _part);
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
            OnActivateKeyPart?.Invoke(GameRuleType.RPG);
            instance.m_RPGGameKeyPart = value;
        }
    }
    public static bool platformerGameKeyPart
    {
        get => instance.m_platformerGameKeyPart;
        set
        {
            if (!value || instance.m_platformerGameKeyPart) return;
            OnActivateKeyPart?.Invoke(GameRuleType.Platformer);
            instance.m_platformerGameKeyPart = value;
        }
    }
    public static bool cardGameKeyPart
    {
        get => instance.m_cardGameKeyPart;
        set
        {
            if (!value || instance.m_cardGameKeyPart) return;
            OnActivateKeyPart?.Invoke(GameRuleType.CardGame);
            instance.m_cardGameKeyPart = value;
        }
    }
    #endregion


    public void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        m_life = m_maxLife;
        m_artifact = m_artifactAtStart;
        m_cardGameKeyPart = false;
        m_platformerGameKeyPart = false;
        m_RPGGameKeyPart = false;
    }

    public void Restart()
    {
        Reset();
    }
}
