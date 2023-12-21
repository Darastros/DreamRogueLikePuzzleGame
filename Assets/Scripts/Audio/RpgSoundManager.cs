using RPG;
using UnityEngine;

public class RpgSoundManager : SoundManager
{
    [SerializeField] private AudioClip activateRpgClip;
    [SerializeField] private AudioClip deactivateRpgClip;
    [SerializeField] private float coinPitchModifier;
    [SerializeField] private AudioClip rpgCoinClip;
    [SerializeField] private AudioClip rpgKeyClip;
    [SerializeField] private AudioClip shopBuyClip;
    [SerializeField] private AudioClip openChestClip;

    protected override void ListenEvent()
    {
        GameManager.OnActivateRPGGame += ActivateRpg;
        GameManager.OnDeactivateRPGGame += DeactivateRpg;

        RPGController.OnGetCoins += GetRpgCoin;
        RPGController.OnGetKeys += GetRpgKey;
        RPGObject.OnOpenChest += OpenChest;
        RPGObject.OnFailOpenChest += ImpossibleAction;
        // RPGObject.OnBuyItem += BuyItem;
        RPGObject.OnFailBuyItem += ImpossibleAction;
    }

    protected override void UnListenEvent()
    {
        GameManager.OnActivateRPGGame -= ActivateRpg;
        GameManager.OnDeactivateRPGGame -= DeactivateRpg;

        RPGController.OnGetCoins -= GetRpgCoin;
        RPGController.OnGetKeys -= GetRpgKey;
        RPGObject.OnOpenChest -= OpenChest;
        RPGObject.OnFailOpenChest -= ImpossibleAction;
        // RPGObject.OnBuyItem -= BuyItem;
        RPGObject.OnFailBuyItem -= ImpossibleAction;
    }

    private void GetRpgCoin(int total, int number)
    {
        if (number > 0)
            PlaySfx(rpgCoinClip, 1f + total * coinPitchModifier);
    }

    private void GetRpgKey(int total, int number)
    {
        if (number > 0)
            PlaySfx(rpgKeyClip, volume: 0.4f);
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
        PlayJingle(activateRpgClip, 1f, activationJingleVolume);
    }

    public void DeactivateRpg()
    {
        PlayJingle(deactivateRpgClip, 1f, activationJingleVolume);
    }
}
