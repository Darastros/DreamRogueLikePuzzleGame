using System;
using CardGame;
using Platformer;
using RPG;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private GameObject _gameObject;

    [Header("Controllers")]
    [SerializeField] private PlatformerController m_platformerController;

    [Header("Audio settings")]
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [Header("Audio clips")]
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip unlockDoorClip;
    [SerializeField] private AudioClip enteringDoorClip;

    [SerializeField] private AudioClip activatePlatformerClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;

    [SerializeField] private AudioClip activateRpgClip;
    [SerializeField] private float coinPitchModifier;
    [SerializeField] private AudioClip rpgCoinClip;
    [SerializeField] private AudioClip rpgKeyClip;

    void Awake()
    {
        _gameObject = gameObject;
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
        // Player interactions
        m_platformerController.platformerMovementController.Jumped += Jump;
        m_platformerController.platformerMovementController.GroundedChanged += GroundedChanged;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnHeal += Heal;
        RPGController.OnGetCoins += GetRpgCoin;
        RPGController.OnGetKeys += GetRpgKey;

        // Game interactions
        // GameManager.OnGameRestart += Restart;
        // GameManager.OnGameLoose += Loose;
        // GameManager.OnGameWin += Win;
        // ExitPortal.OnCrossPortal += CrossPortal;

        PlatformerController.OnActivate += ActivatePlatformer;
        PlatformerController.OnDeactivate += DeactivatePlatformer;
        RPGController.OnActivate += ActivateRpg;
        RPGController.OnDeactivate += DeactivateRpg;
        // CardGameController.OnActivate += ActivateCG;
        // CardGameController.OnDeactivate += DeactivateCG;
    }

    private void UnListenEvent()
    {
        m_platformerController.platformerMovementController.Jumped -= Jump;
        m_platformerController.platformerMovementController.GroundedChanged -= GroundedChanged;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnHeal -= Heal;
        RPGController.OnGetCoins -= GetRpgCoin;
        RPGController.OnGetKeys -= GetRpgKey;

        PlatformerController.OnActivate -= ActivatePlatformer;
        PlatformerController.OnDeactivate -= DeactivatePlatformer;
        RPGController.OnActivate -= ActivateRpg;
        RPGController.OnDeactivate -= DeactivateRpg;
    }

    public void PlayAudio(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        AudioSource audioSource = _gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length / Math.Abs(audioSource.pitch));
    }

    private void Hit(int _newValue, int _delta)
    {
        PlayAudio(damageClip);
    }

    private void Heal(int _newValue, int _delta)
    {
        PlayAudio(healClip);
    }

    private void Jump()
    {
        PlayAudio(jumpClip, Random.Range(minPitch, maxPitch));
    }

    private void GroundedChanged(bool grounded, float velocity)
    {
        if (grounded)
            PlayAudio(landingClip, Random.Range(minPitch, maxPitch));
    }

    private void GetRpgCoin(int total, int number)
    {
        PlayAudio(rpgCoinClip, 1f + total * coinPitchModifier);
    }

    private void GetRpgKey(int total, int number)
    {
        PlayAudio(rpgKeyClip);
    }

    public void ActivatePlatformer()
    {
        PlayAudio(activatePlatformerClip);
    }

    public void DeactivatePlatformer()
    {
        PlayAudio(activatePlatformerClip, -1f);
    }

    public void ActivateRpg()
    {
        PlayAudio(activateRpgClip);
    }

    public void DeactivateRpg()
    {
        PlayAudio(activateRpgClip, -1f);
    }

    void Update()
    {
        
    }
}
