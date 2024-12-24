using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private Image bG;
    [SerializeField] private Sprite loadBg;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private HPController hpController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private MapController mapController;

    [Space]
    [SerializeField] private CardSetScriptableObject cardSetScriptableObject;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _transform.DOScale(Vector3.one * 1.2f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _transform.DOScale(Vector3.one, 0.3f);
    }

    public async void OnPointerDown(PointerEventData eventData)
    {
        soundController.PlayClick();
        hpController.ChangeCountHP(3);
        inventoryController.ClearInventory();
        _transform.DOScale(Vector3.one, 0.2f);
        
        cardAnimation.HideCard();
        bG.sprite = loadBg;
        hpController.CurrentHP = 3;

        mapController.DeactivateCheckPoint(1);
        cardAnimation.HideCard();
        await bannerAnimation.HideBanner();
        await gameController.HideDead();
        contentCard.SetContent(cardSetScriptableObject);
        await bannerAnimation.ShowBanner();
        await cardAnimation.ShowCards();
    }
}
