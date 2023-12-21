using CardGame;
using UnityEngine;

public class CardGameSoundManager : SoundManager
{
    [SerializeField] private AudioClip activateCardClip;
    [SerializeField] private AudioClip deactivateCardClip;
    [SerializeField] private AudioClip pickupCardClip;
    [SerializeField] private AudioClip craftCardClip;
    [SerializeField] private AudioClip failedCraftCardClip;

    protected override void ListenEvent()
    {
        GameManager.OnActivateCardGame += ActivateCardGame;
        GameManager.OnDeactivateCardGame += DeactivateCardGame;
        CardGameController.OnGettingCard += OnGettingCard;
        CardGameController.OnCraftSuccess += OnCardCraft;
        CardGameController.OnCraftFailed += CraftFailed;
        CardGameController.OnTryCraftWithoutCard += ImpossibleAction;
    }

    protected override void UnListenEvent()
    {
        GameManager.OnActivateCardGame -= ActivateCardGame;
        GameManager.OnDeactivateCardGame -= DeactivateCardGame;
        CardGameController.OnGettingCard -= OnGettingCard;
        CardGameController.OnCraftSuccess -= OnCardCraft;
        CardGameController.OnCraftFailed -= CraftFailed;
        CardGameController.OnTryCraftWithoutCard -= ImpossibleAction;
    }

    public void ActivateCardGame()
    {
        PlayJingle(activateCardClip, 1f, activationJingleVolume);
    }

    public void DeactivateCardGame()
    {
        PlayJingle(deactivateCardClip, 1f, activationJingleVolume);
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
        PlayJingle(failedCraftCardClip, 1.2f);
    }
}
