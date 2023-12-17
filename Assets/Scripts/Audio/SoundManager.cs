using System;
using CardGame;
using GameSystems;
using Platformer;
using RPG;
using UnityEngine;
using UnityEngine.Audio;
using Utils;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour, IEventListener
{
    private GameObject _gameObject;

    [Header("Controllers")]
    [SerializeField] private PlatformerController m_platformerController;

    [Header("Audio settings")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup jingleMixerGroup;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

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
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip activateKeyPartClip;


    [SerializeField] private AudioClip activatePlatformerClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip collectStrawberryClip;

    [SerializeField] private AudioClip activateRpgClip;
    [SerializeField] private float coinPitchModifier;
    [SerializeField] private AudioClip rpgCoinClip;
    [SerializeField] private AudioClip rpgKeyClip;
    [SerializeField] private AudioClip shopBuyClip;
    [SerializeField] private AudioClip openChestClip;

    [SerializeField] private AudioClip activateCardClip;
    [SerializeField] private AudioClip pickupCardClip;
    [SerializeField] private AudioClip craftCardClip;
    [SerializeField] private AudioClip failedCraftCardClip;

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

    void Start()
    {
        DungeonRoomSystem.Instance.GetEventDispatcher()
            .RegisterEvent<OnRoomChanged>(this, OnRoomChanged);
    }

    private void ListenEvent()
    {
        // Player interactions
        PlayerDataManager.OnActivateKeyPart += ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact += OnCollectArtifact;
        PlayerDataManager.OnUseArtifact += OnSealedRoom;
        PlayerDataManager.OnHit += Hit;
        PlayerDataManager.OnHeal += Heal;
        Spawner.OnSpawn += OnProjectileSpawn;
        //Projectile.OnHit += 

        RPGController.OnGetCoins += GetRpgCoin;
        RPGController.OnGetKeys += GetRpgKey;
        RPGObject.OnOpenChest += OpenChest;
        RPGObject.OnFailOpenChest += ImpossibleAction;
        RPGObject.OnBuyItem += BuyItem;
        RPGObject.OnFailBuyItem += ImpossibleAction;

        PlatformerController.OnGetStrawberries += OnGetStrawberries;
        m_platformerController.platformerMovementController.Jumped += Jump;
        m_platformerController.platformerMovementController.GroundedChanged += GroundedChanged;

        CardGameController.OnGettingCard += OnGettingCard;
        CardGameController.OnCraftSuccess += OnCardCraft;
        CardGameController.OnCraftFailed += CraftFailed;
        CardGameController.OnTryCraftWithoutCard += ImpossibleAction;

        // Game interactions
        // GameManager.OnGameRestart += Restart;
        GameManager.OnGameLoose += OnDeath;
        // GameManager.OnGameWin += Win;
        ExitPortal.OnCrossPortal += CrossPortal;

        GameManager.OnActivatePlatformerGame += ActivatePlatformer;
        GameManager.OnDeactivatePlatformerGame += DeactivatePlatformer;
        GameManager.OnActivateRPGGame += ActivateRpg;
        GameManager.OnDeactivateRPGGame += DeactivateRpg;
        GameManager.OnActivateCardGame += ActivateCardGame;
        GameManager.OnDeactivateCardGame += DeactivateCardGame;
    }

    private void UnListenEvent()
    {
        PlayerDataManager.OnActivateKeyPart -= ActivateKeyPart;
        PlayerDataManager.OnCollectArtifact -= OnCollectArtifact;
        PlayerDataManager.OnUseArtifact -= OnSealedRoom;
        PlayerDataManager.OnHit -= Hit;
        PlayerDataManager.OnHeal -= Heal;
        DungeonRoomSystem.Instance.GetEventDispatcher().UnregisterEvent<OnRoomChanged>(this);
        Spawner.OnSpawn -= OnProjectileSpawn;

        RPGController.OnGetCoins -= GetRpgCoin;
        RPGController.OnGetKeys -= GetRpgKey;
        RPGObject.OnOpenChest -= OpenChest;
        RPGObject.OnFailOpenChest -= ImpossibleAction;
        RPGObject.OnBuyItem -= BuyItem;
        RPGObject.OnFailBuyItem -= ImpossibleAction;

        PlatformerController.OnGetStrawberries -= OnGetStrawberries;
        m_platformerController.platformerMovementController.Jumped -= Jump;
        m_platformerController.platformerMovementController.GroundedChanged -= GroundedChanged;

        CardGameController.OnGettingCard -= OnGettingCard;
        CardGameController.OnCraftSuccess -= OnCardCraft;
        CardGameController.OnCraftFailed -= CraftFailed;
        CardGameController.OnTryCraftWithoutCard -= ImpossibleAction;

        GameManager.OnGameLoose -= OnDeath;
        GameManager.OnActivatePlatformerGame -= ActivatePlatformer;
        GameManager.OnDeactivatePlatformerGame -= DeactivatePlatformer;
        GameManager.OnActivateRPGGame -= ActivateRpg;
        GameManager.OnDeactivateRPGGame -= DeactivateRpg;
        GameManager.OnActivateCardGame -= ActivateCardGame;
        GameManager.OnDeactivateCardGame -= DeactivateCardGame;
    }

    public void PlayAudio(AudioClip clip, AudioMixerGroup mixerGroup, float pitch = 1f, float volume = 1f)
    {
        AudioSource audioSource = _gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length / Math.Abs(audioSource.pitch));
    }

    public void PlayJingle(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        PlayAudio(clip, jingleMixerGroup, pitch, volume);
    }

    public void PlaySfx(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        PlayAudio(clip, sfxMixerGroup, pitch, volume);
    }

    private void Hit(int _newValue, int _delta)
    {
        PlaySfx(damageClip);
    }

    private void Heal(int _newValue, int _delta)
    {
        PlaySfx(healClip);
    }

    private void OnRoomChanged(OnRoomChanged _obj)
    {
        if(_obj.m_from != null)
        {
            PlaySfx(enteringDoorClip, Random.Range(minPitch, maxPitch));
        } 
    }

    private void OnCollectArtifact(int newValue, int delta)
    {
        PlaySfx(collectArtifactClip, Random.Range(minPitch, maxPitch));
    }

    private void OnSealedRoom(int newValue, int delta)
    {
        PlaySfx(sealRoomClip, Random.Range(minPitch, maxPitch));
    }

    private void OnProjectileSpawn()
    {
        PlaySfx(projectileSpawnClip, Random.Range(minPitch, maxPitch));
    }

    private void OnDeath()
    {
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

    private void ImpossibleAction()
    {
        PlaySfx(errorClip);
    }

    // Plateformer
    #region platformer

    public void ActivatePlatformer()
    {
        PlayJingle(activatePlatformerClip);
    }

    public void DeactivatePlatformer()
    {
        // PlayJingle(activatePlatformerClip, -1f);
    }

    private void Jump()
    {
        PlaySfx(jumpClip, Random.Range(minPitch, maxPitch));
    }

    private void GroundedChanged(bool grounded, float velocity)
    {
        if (grounded)
            PlaySfx(landingClip, Random.Range(minPitch, maxPitch));
    }

    private void OnGetStrawberries(int total, int number)
    {
        PlaySfx(collectStrawberryClip);
    }

    #endregion
    
    // RPG
    #region RPG

    private void GetRpgCoin(int total, int number)
    {
        PlaySfx(rpgCoinClip, 1f + total * coinPitchModifier);
    }

    private void GetRpgKey(int total, int number)
    {
        PlaySfx(rpgKeyClip);
    }

    private void OpenChest()
    {
        PlaySfx(openChestClip);
    }

    private void BuyItem()
    {
        PlaySfx(shopBuyClip);
    }

    public void ActivateRpg()
    {
        PlayJingle(activateRpgClip);
    }

    public void DeactivateRpg()
    {
        // PlayJingle(activateRpgClip, -1f);
    }

    #endregion

    // Card game
    #region Cardgame

    public void ActivateCardGame()
    {
        PlayJingle(activateCardClip);
    }

    public void DeactivateCardGame()
    {
        // PlayJingle(activateCardClip, -1f);
    }

    private void OnGettingCard(Card card)
    {
        PlaySfx(pickupCardClip);
    }

    private void OnCardCraft(CraftCardResult result, Vector3 position)
    {
        PlayJingle(craftCardClip, 1.2f);
    }

    private void CraftFailed()
    {
        PlaySfx(failedCraftCardClip, 1.2f);
    }

    #endregion

    void Update()
    {
        
    }
}
