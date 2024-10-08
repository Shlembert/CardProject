using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private Image bG;
    [SerializeField] private Sprite loadBg;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private HPController hpController;

    [Space]
    [SerializeField] private CardSetScriptableObject cardSetScriptableObject;

    private Transform _transform;
    private bool _click = false;

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
        if (_click) return; _click = true;
        soundController.PlayClick();
        _transform.DOScale(Vector3.one, 0.2f);
        contentCard.SetContent(cardSetScriptableObject);
        cardAnimation.HideCard();
        bG.sprite = loadBg;
        await bannerAnimation.HideBanner();
         gameController.HideMenu();
        await bannerAnimation.ShowBanner();
        await cardAnimation.ShowCards();
        _click = false;
    }
}
