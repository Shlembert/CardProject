using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private Image bG;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private SoundController soundController;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private HPController hpController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private MapController mapController;

    private CheckPoint _checkPoint;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _checkPoint = GetComponent<CheckPoint>();
        _checkPoint.Location = bG.sprite;
        _checkPoint.HpCount = hpController.GetCurrentHP();
        _checkPoint.CardSetScriptableObject = contentCard.CardSetScriptableObject;
        _checkPoint.Items.Clear();
        _checkPoint.Items = inventoryController.GetInventorySprites();
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
        _transform.DOScale(Vector3.one, 0.2f);
        contentCard.SetContent(_checkPoint.CardSetScriptableObject);
        cardAnimation.HideCard();
        bG.sprite = _checkPoint.Location;
        hpController.SetHP(_checkPoint.HpCount);
        inventoryController.LoadInventorySprites(_checkPoint.Items);
        mapController.DeactivateCheckPoint(_checkPoint.Index +1);
        await bannerAnimation.HideBanner();
        await gameController.HideMap();
        await bannerAnimation.ShowBanner();
        await cardAnimation.ShowCards();
    }
}
