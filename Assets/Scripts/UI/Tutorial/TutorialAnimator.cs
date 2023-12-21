using UnityEngine;
using Player;

public class TutorialAnimator : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void OnDisable()
    {
        InputManager.OnCraft -= Disappear;
    }

    public void Appear()
    {
        m_animator.SetTrigger("Appear");
        InputManager.OnCraft += Disappear;
    }

    public void Disappear()
    {
        m_animator.SetTrigger("Disappear");
        InputManager.OnCraft -= Disappear;
    }
    

    public void GamePause()
    {
        GameManager.Instance.Pause();
    }

    public void GameResume()
    {
        GameManager.Instance.Resume();
        gameObject.SetActive(false);
    }
    
    
}
