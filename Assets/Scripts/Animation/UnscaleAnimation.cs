using UnityEngine;

public class UnscaleAnimation : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

}
