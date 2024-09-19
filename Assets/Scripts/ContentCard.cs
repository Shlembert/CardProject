using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentCard : MonoBehaviour
{
    [SerializeField] private Image portraitSprite, illustrationSprite_L, illustrationSprite_R, reverseSprite, locationSprite;
    [SerializeField] private TMP_Text bannerText, reverseTopText, cartText_L, cardText_R;
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

    private int _changeLifeCount = 0;
    private bool _changeBanner;
    private bool _removeItem;
    private bool _checkRemove;

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

    public void SetContent(CardSetScriptableObject cardSetScriptableObject)
    {
        CardSetScriptableObject = cardSetScriptableObject;
        Debug.Log(cardSetScriptableObject.name);
        portraitSprite.sprite = cardSetScriptableObject?.portrait;

        input_L.AnimationClip = cardSetScriptableObject?.leftCard?.animationClip;
        input_L.AnimatorController = cardSetScriptableObject?.leftCard?.animator;
        illustrationSprite_L.sprite = cardSetScriptableObject?.leftCard?.illustrationSprite;

        input_R.AnimationClip = cardSetScriptableObject?.rightCard?.animationClip;
        input_R.AnimatorController = cardSetScriptableObject?.rightCard?.animator;
        illustrationSprite_R.sprite = cardSetScriptableObject?.rightCard?.illustrationSprite;

        bannerText.text = cardSetScriptableObject?.bannerText;
        cartText_L.text = cardSetScriptableObject?.leftCard.messageText;
        cardText_R.text = cardSetScriptableObject?.rightCard.messageText;

        if (cardSetScriptableObject.checkPoint) mapController.ActivateCheckPoint();
    }

    public void CheckRequestItem(CardType cardType)
    {
        CardData cardData = SetSideCard(cardType);
        StopAnim();
        CheckChangeLocation(cardData);
        CheckRemove = cardData.removeItemSprite;
        reverseSprite.enabled = true;

        _changeBanner = cardData.changeBanner; // �������� �� ����� �������

        if (cardData.requiredItemSprite != null) RequestItem(cardData);// ��������, ��������� �� �������
        else NotRequestItem(cardData);
        
    }

    private void RequestItem(CardData cardData) 
    {
        if (inventoryController.CheckHaveItem(cardData)) // ���� ������� � ���������
        {
            NextCardSetScriptableObject = cardData.nextSetIfItem;

            _haveItem = cardData.requiredItemSprite;
            _removeItem = cardData.requiredItemSprite;
            reverseTopText.text = cardData.reverseTopTextIfItem;
        }
        else                                       // ��� ��������
        {
            NextCardSetScriptableObject = cardData.nextSetIfNoItem;
            _changeLifeCount = cardData.changeLifePointsIfNoItem;
            reverseTopText.text = cardData.reverseTopTextIfNoItem;
        }

        reverseSprite.sprite = cardData.requiredItemSprite;
    }

    private void NotRequestItem(CardData cardData)
    {
        NextCardSetScriptableObject = cardData.nextSetIfItem;
        _changeLifeCount = cardData.changeLifePointsIfItem;

        reverseTopText.text = cardData.reverseTopTextIfItem;

        if (cardData.itemSprite) // ��������, ������ �� �������
        {
            reverseSprite.sprite = cardData.itemSprite;
            _giveItem = cardData.itemSprite;
        }
        else
        {
            if (cardData.reverseSpriteIfItem) reverseSprite.sprite = cardData.reverseSpriteIfItem;
            else reverseSprite.enabled = false;
            _giveItem = null;
        }
    }

    private void CheckChangeLocation(CardData cardData)
    {
        if (cardData.locationSprite != null) _location = cardData.locationSprite; // �������� �� ����� �������
        else _location = null;
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

    public async UniTask RemoveItemFromInventory()
    {
        if (CheckRemove)
        {
            inventoryController.RemoveItem(_haveItem);
            _giveItem = null;
            await inventoryAnimation.RemoveItemFromInventory(_haveItem);
        }
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



