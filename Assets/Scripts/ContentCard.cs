using Cysharp.Threading.Tasks;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class ContentCard : MonoBehaviour
{
    [SerializeField] private Image portraitSprite, illustrationSprite_L, illustrationSprite_R, reverseSprite, locationSprite;
    [SerializeField] private Localize bannerLoc, reverseLoc, cardLoc_L, cardLoc_R;
    [SerializeField] private CardSetScriptableObject startSet;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InputCard input_L, input_R;
    [SerializeField] private MapController mapController;
    [SerializeField] private InventoryAnimation inventoryAnimation;
    [SerializeField] private HPController hpController;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private WinController winController;
    [SerializeField] private Animator animL, animR, animB;

    private CardSetScriptableObject _cardSetScriptableObject, _nextCardSetScriptableObject;
    private Sprite _giveItem, _haveItem, _location;
    private CardData _cardData;
    private AudioClip _reverseAudioClip, _bannerAudioClip;
    private int _changeLifeCount = 0;
    private bool _changeBanner;
    private bool _removeItem;
    private bool _checkRemove;
    private bool _isGiveItem;
    private bool _isWin;

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
    public bool IsWin { get => _isWin; set => _isWin = value; }

    public void SetContent(CardSetScriptableObject cardSetScriptableObject)
    {
        if (cardSetScriptableObject == null) cardSetScriptableObject = startSet;

        CardSetScriptableObject = cardSetScriptableObject;
        Debug.Log($" + Load CardSet name: [{cardSetScriptableObject.name}] +");

        portraitSprite.sprite = cardSetScriptableObject.portrait;

        if (cardSetScriptableObject.bannerAnimator && cardSetScriptableObject.animationClip)
        {
            bannerAnimation.AnimatorController = cardSetScriptableObject.bannerAnimator;
            bannerAnimation.AnimationClip = cardSetScriptableObject.animationClip;
        }

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

        CheckRemove = _cardData.removeItemSprite;

        _changeBanner = _cardData.changeBanner; // �������� �� ����� �������

        if (_cardData.requiredItemSprite != null) RequestItem(_cardData);// ��������, ��������� �� �������
        else NotRequestItem(_cardData);

        CheckGiveItem(_cardData);
    }

    private void RequestItem(CardData cardData)
    {
        if (inventoryController.CheckHaveItem(cardData)) // ���� ������� � ���������
        {
            NextCardSetScriptableObject = cardData.nextSetIfItem;
            _isWin = cardData.isWinIfItem;
            if (_isWin) winController.SetWinContent(cardData.winContentIfItem);
            _haveItem = cardData.requiredItemSprite;
            _removeItem = cardData.requiredItemSprite;
            reverseLoc.Term = cardData.reverseTopTextIfItem;
            reverseSprite.sprite = cardData.reverseSpriteIfItem;
            _location = cardData.locationSpriteIfItem;

            _reverseAudioClip = null;
            _reverseAudioClip = cardData?.reverseAudioClipItem;
        }
        else                                       // ��� ��������
        {

            NextCardSetScriptableObject = cardData.nextSetIfNoItem;
            _isWin = cardData.isWinIfNoItem;
            if (_isWin) winController.SetWinContent(cardData.winContentIfNoItem);
            _changeLifeCount = cardData.changeLifePointsIfNoItem;
            reverseLoc.Term = cardData.reverseTopTextIfNoItem;
            reverseSprite.sprite = cardData.reverseSpriteIfNoItem;
            _location = cardData.locationSpriteNoItem;
            _reverseAudioClip = null;
            _reverseAudioClip = cardData?.reverseAudioClipNoItem;
        }
    }

    private void NotRequestItem(CardData cardData)
    {
        NextCardSetScriptableObject = cardData.nextSetIfItem;
        _isWin = cardData.isWinIfItem;
        if (_isWin) winController.SetWinContent(cardData.winContentIfItem);
        _changeLifeCount = cardData.changeLifePointsIfItem;
        reverseLoc.Term = cardData.reverseTopTextIfItem;
        reverseSprite.sprite = cardData.reverseSpriteIfItem;
        _location = cardData.locationSpriteIfItem;
        _reverseAudioClip = null;
        _reverseAudioClip = cardData?.reverseAudioClipItem;
    }

    private void CheckGiveItem(CardData cardData)
    {
        _isGiveItem = cardData.itemSprite;

        if (_isGiveItem) // ��������, ������ �� �������
        {
            Debug.Log($"�������, ��������� �������? {CheckRemove}");
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



