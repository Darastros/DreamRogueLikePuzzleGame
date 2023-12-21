using System;
using GameSystems;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GlobalSoundManager : SoundManager, IEventListener
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip unlockDoorClip;
    [SerializeField] private AudioClip enteringDoorClip;
    [SerializeField] private AudioClip collectArtifactClip;
    [SerializeField] private AudioClip sealRoomClip;
    [SerializeField] private AudioClip deathJingleClip;
    [SerializeField] private AudioClip portalJingleClip;
    [SerializeField] private AudioClip projectileSpawnClip;
    [SerializeField] private AudioClip projectileHitClip;
    [SerializeField] private AudioClip activateKeyPartClip;
    [SerializeField] private bool manualAudioDuckingEnabled;

    private AudioSource musicSource;

    void Start()
    {
        DungeonRoomSystem.EventDispatcher?
            .RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
        musicSource = GetComponent<AudioSource>();
        StartMusic();
    }

    protected override void ListenEvent()
    {
        // Player interactions
        PlayerDataManager.OnActivateKeyPart += ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact += OnCollectArtifact;
        PlayerDataManager.OnUseArtifact += OnSealedRoom;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnHeal += Heal;
        Spawner.OnSpawn += OnProjectileSpawn;
        Projectile.OnHit += OnProjectileHit;

        // Game interactions
        GameManager.OnGameLoose += OnDeath;
        GameManager.OnGameWin += StopMusic;
        GameManager.OnGameRestart += StartMusic;
        ExitPortal.OnCrossPortal += CrossPortal;
    }

    protected override void UnListenEvent()
    {
        PlayerDataManager.OnActivateKeyPart -= ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact -= OnCollectArtifact;
        PlayerDataManager.OnUseArtifact -= OnSealedRoom;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnHeal -= Heal;
        DungeonRoomSystem.EventDispatcher?.UnregisterEvent<OnRoomChanged>(this);
        Spawner.OnSpawn -= OnProjectileSpawn;
        Projectile.OnHit -= OnProjectileHit;

        GameManager.OnGameLoose -= OnDeath;
        GameManager.OnGameWin -= StopMusic;
        GameManager.OnGameRestart -= StartMusic;
        ExitPortal.OnCrossPortal -= CrossPortal;
    }

    private void OnProjectileHit()
    {
        PlaySfx(projectileHitClip, Random.Range(0.5f, 1.5f), 1.0f);
    }

    private void StartMusic()
    {
        lastJingleTime = 0f;
        musicSource.Play();
    }

    private void StopMusic()
    {
        musicSource.Stop();
    }

    private void Hit(int _newValue, int _delta)
    {
        PlaySfx(damageClip, volume: 0.6f);
    }

    private void Heal(int _newValue, int _delta)
    {
        PlaySfx(healClip, volume: 0.3f);
    }

    private void OnRoomChanged(OnRoomChanged _obj)
    {
        if (_obj.m_from != null)
        {
            PlaySfx(enteringDoorClip, Random.Range(minRandomPitch, maxRandomPitch));

        }
    }

    private void OnCollectArtifact(int newValue, int delta)
    {
        PlaySfx(collectArtifactClip, Random.Range(minRandomPitch, maxRandomPitch), volume: 0.4f, delay: 0.2f);
    }

    private void OnSealedRoom(int newValue, int delta)
    {
        PlaySfx(sealRoomClip, Random.Range(minRandomPitch, maxRandomPitch), 0.7f);
    }

    private void OnProjectileSpawn()
    {
        PlaySfx(projectileSpawnClip, Random.Range(minRandomPitch, maxRandomPitch));
    }

    private void OnDeath()
    {
        StopMusic();
        PlayJingle(deathJingleClip);
    }

    private void CrossPortal(Vector3 _center)
    {
        PlayJingle(portalJingleClip, 1f, 0.5f);
    }

    private void ActivateKeyPart(GameRuleType _part)
    {
        PlayJingle(activateKeyPartClip);
    }

    void Update()
    {
        if (manualAudioDuckingEnabled)
        {
            lastJingleTime = Math.Max(0, lastJingleTime - Time.unscaledDeltaTime);
            musicSource.volume = lastJingleTime <= 0 ? 1.0f : 0f;
        }
    }
}
