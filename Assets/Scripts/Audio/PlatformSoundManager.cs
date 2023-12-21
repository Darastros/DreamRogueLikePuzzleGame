using Platformer;
using UnityEngine;

public class PlatformSoundManager : SoundManager
{
    [Header("Controllers")]
    [SerializeField] private PlatformerController m_platformerController;
    [Header("Audio clips")]
    [SerializeField] private AudioClip activatePlatformerClip;
    [SerializeField] private AudioClip deactivatePlatformerClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip collectStrawberryClip;

    protected override void ListenEvent()
    {
        GameManager.OnActivatePlatformerGame += ActivatePlatformer;
        GameManager.OnDeactivatePlatformerGame += DeactivatePlatformer;
        PlatformerController.OnGetStrawberries += OnGetStrawberries;
        m_platformerController.platformerMovementController.Jumped += Jump;
        m_platformerController.platformerMovementController.GroundedChanged += GroundedChanged;
    }

    protected override void UnListenEvent()
    {
        GameManager.OnActivatePlatformerGame -= ActivatePlatformer;
        GameManager.OnDeactivatePlatformerGame -= DeactivatePlatformer;
        PlatformerController.OnGetStrawberries -= OnGetStrawberries;
        m_platformerController.platformerMovementController.Jumped -= Jump;
        m_platformerController.platformerMovementController.GroundedChanged -= GroundedChanged;
    }

    public void ActivatePlatformer()
    {
        PlayJingle(activatePlatformerClip, 1f, activationJingleVolume);
    }

    public void DeactivatePlatformer()
    {
        PlayJingle(deactivatePlatformerClip, 1f, activationJingleVolume);
    }

    private void Jump()
    {
        PlaySfx(jumpClip, Random.Range(minRandomPitch, maxRandomPitch));
    }

    private void GroundedChanged(bool grounded, float velocity)
    {
        if (grounded)
            PlaySfx(landingClip, Random.Range(minRandomPitch, maxRandomPitch), 0.45f);
    }

    private void OnGetStrawberries(int total, int number)
    {
        PlaySfx(collectStrawberryClip);
    }
}
