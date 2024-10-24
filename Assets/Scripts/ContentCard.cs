using Cysharp.Threading.Tasks;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class ContentCard : MonoBehaviour
{
    [SerializeField] private Image portraitSprite, illustrationSprite_L, illustrationSprite_R, reverseSprite, locationSprite;
    [SerializeField] private Localize bannerLoc, reverseLoc, cardLoc_L, cardLoc_R;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InputCard input_L, input_R;
    [SerializeField] private MapController mapController;
    [SerializeField] private InventoryAnimation inventoryAnimation;
    [SerializeField] private HPController hpController;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private Animator animL, animR;

    private CardSetScriptableObject _cardSetScriptableObject, _nextCardSetScriptableObject;
    private Sprite _giveItem, _haveItem, _location;
    private CardData _cardData;
    private AudioClip _reverseAudioClip, _bannerAudioClip;
    private int _changeLifeCount = 0;
    private bool _changeBanner;
    private bool _removeItem;
    private bool _checkRemove;
    private bool _isGiveItem;

    public CardSetScriptableObject CardSetScriptableObject
    {
        get => _cardSetScriptableObject;
        set => _cardSetScriptableObject = value;
    }

    public CardSetScriptableObject NextCardSetScriptableObject
    {
        get => _nextCardSetScriptableObject;
        set => _nextCardSetScriptableObject = value;
    }

    public bool CheckRemove { get => _checkRemove; set => _checkRemove = value; }
    public AudioClip ReverseAudioClip { get => _reverseAudioClip; set => _reverseAudioClip = value; }
    public AudioClip BannerAudioClip { get => _bannerAudioClip; set => _bannerAudioClip = value; }
    public bool IsGiveItem { get => _isGiveItem; set => _isGiveItem = value; }

    public void SetContent(CardSetScriptableObject cardSetScriptableObject)
    {
        CardSetScriptableObject = cardSetScriptableObject;

        Debug.Log($" + Load CardSet name: [{cardSetScriptableObject.name}] +");
        portraitSprite.sprite = cardSetScriptableObject?.portrait;
        BannerAudioClip = null;
        BannerAudioClip = cardSetScriptableObject?.bannerSound;
        input_L.AnimationClip = cardSetScriptableObject?.leftCard?.animationClip;
        input_L.AnimatorController = cardSetScriptableObject?.leftCard?.animator;
        illustrationSprite_L.sprite = cardSetScriptableObject?.leftCard?.illustrationSprite;

        input_R.AnimationClip = cardSetScriptableObject?.rightCard?.animationClip;
        input_R.AnimatorController = cardSetScriptableObject?.rightCard?.animator;
        illustrationSprite_R.sprite = cardSetScriptableObject?.rightCard?.illustrationSprite;


        bannerLoc.Term = cardSetScriptableObject?.bannerText;
        cardLoc_L.Term = cardSetScriptableObject?.leftCard?.messageText;
        cardLoc_R.Term = cardSetScriptableObject?.rightCard?.messageText;

        if (cardSetScriptableObject.checkPoint) mapController.ActivateCheckPoint();
    }

    public void CheckRequestItem(CardType cardType)
    {
        _cardData = SetSideCard(cardType);
        StopAnim();
        CheckChangeLocation(_cardData);

        CheckRemove = _cardData.removeItemSprite;

        _changeBanner = _cardData.changeBanner; // Проверка на смену баннера

        if (_cardData.requiredItemSprite != null) RequestItem(_cardData);// Проверка, требуется ли предмет
        else NotRequestItem(_cardData);

        CheckGiveItem(_cardData);
    }

    private void RequestItem(CardData cardData)
    {
        if (inventoryController.CheckHaveItem(cardData)) // Есть предмет в инвентаре
        {
            NextCardSetScriptableObject = cardData.nextSetIfItem;

            _haveItem = cardData.requiredItemSprite;
            _removeItem = cardData.requiredItemSprite;
            reverseLoc.Term = cardData.reverseTopTextIfItem;
            reverseSprite.sprite = cardData.reverseSpriteIfItem;
            _reverseAudioClip = null;
            _reverseAudioClip = cardData?.reverseAudioClipItem;
        }
        else                                       // Нет предмета
        {
            NextCardSetScriptableObject = cardData.nextSetIfNoItem;
            _changeLifeCount = cardData.changeLifePointsIfNoItem;
            reverseLoc.Term = cardData.reverseTopTextIfNoItem;
            reverseSprite.sprite = cardData.reverseSpriteIfNoItem;
            _reverseAudioClip = null;
            _reverseAudioClip = cardData?.reverseAudioClipNoItem;
        }
    }

    private void NotRequestItem(CardData cardData)
    {
        NextCardSetScriptableObject = cardData.nextSetIfItem;
        _changeLifeCount = cardData.changeLifePointsIfItem;
        reverseLoc.Term = cardData.reverseTopTextIfItem;
        reverseSprite.sprite = cardData.reverseSpriteIfItem;
        _reverseAudioClip = null;
        _reverseAudioClip = cardData?.reverseAudioClipItem;
    }

    private void CheckGiveItem(CardData cardData)
    {
        _isGiveItem = cardData.itemSprite;

        if (_isGiveItem) // Проверка, дается ли предмет
        {
            Debug.Log($"Провера, требуется предмет? {CheckRemove}");
            _giveItem = cardData.itemSprite;

            if (CheckRemove) return;

            inventoryController.GiveSprite = _giveItem;
            Debug.Log($"Present: {_giveItem.name}");
        }
        else
        {
            _giveItem = null;
        }
    }

    private CardData SetSideCard(CardType cardType)
    {
        CardData cardData = null;
        if (cardType == CardType.Left) cardData = CardSetScriptableObject.leftCard;
        else if (cardType == CardType.Right) cardData = CardSetScriptableObject.rightCard;
        return cardData;
    }

    public async UniTask SetItemToInventory()
    {
        if (_giveItem)
        {
            await inventoryAnimation
           .MoveItemToInventory(_giveItem, reverseSprite.transform, inventoryController.GetTarget());
            Debug.Log($" ++++  Add item = {_giveItem}");
            _giveItem = null;
        }
    }

    public async UniTask RemoveItemFromInventory()
    {
        if (!CheckRemove) return;
        if (_removeItem)
        {
            inventoryController.RemoveItem(_haveItem);
            _giveItem = null;
            await inventoryAnimation.RemoveItemFromInventory(_haveItem);
        }
    }

    public void ChangeCountHP()
    {
        hpController.ChangeCountHP(_changeLifeCount);
    }

    public async UniTask ShowBanner()
    {
        if (_changeBanner) await bannerAnimation.ShowBanner();
        else await bannerAnimation.ShowMessageBanner();

        await UniTask.Yield();
    }

    public async UniTask HoldBanner()
    {
        if (_changeBanner) await bannerAnimation.HideBanner();
        else await bannerAnimation.HideMessageBanner();
    }

    private void CheckChangeLocation(CardData cardData)
    {
        if (cardData.locationSprite != null) _location = cardData.locationSprite; // Проверка на смену локации
        else _location = null;
    }

    public async UniTask ChangeLocationSprite()
    {
        if (_location == null) return;
        await gameController.ChangeBG(_location);
    }

    private void StopAnim()
    {
        animL.enabled = false;
        animR.enabled = false;
    }
}



