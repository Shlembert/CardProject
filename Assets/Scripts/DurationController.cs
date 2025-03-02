using UnityEngine;
using UnityEngine.UI;

public class DurationController : MonoBehaviour
{
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private InventoryAnimation inventoryAnimation;
    [SerializeField] private Image iconSpeed;
    [SerializeField] private Sprite iconNormal, iconFast, iconNone;
    [SerializeField] private DurationType durationType;

    int _bannerDelay;
    private float _durationAvatar,
                  _durationBannerPanel,
                  _durationCard,
                  _durationCurrentCard,
                  _durationGameController,
                  _inventoryDuration;

    private void Start()
    {
        GetDurationValue();
    }

    private void GetDurationValue()
    {
        _bannerDelay = bannerAnimation.Delay;
        _durationAvatar = bannerAnimation.AvatarDuration;
        _durationBannerPanel = bannerAnimation.TextDuration;
        _durationCard = cardAnimation.Duration;
        _durationCurrentCard = cardAnimation.CurrentDuration;
        _durationGameController = gameController.Duration;
        _inventoryDuration = inventoryAnimation.Duration;
    }

    private void SetNoneDuration()
    {
        iconSpeed.sprite = iconNone;
        bannerAnimation.Delay = 0;
        bannerAnimation.TextDuration = 0;
        bannerAnimation.AvatarDuration = 0;
        cardAnimation.Duration = 0;
        cardAnimation.CurrentDuration = 0;
        gameController.Duration = 0;
        inventoryAnimation.Duration = 0;
    }

    private void SetNormalDuration()
    {
        iconSpeed.sprite = iconNormal;
        bannerAnimation.Delay = _bannerDelay;
        bannerAnimation.AvatarDuration = _durationAvatar;
        bannerAnimation.TextDuration = _durationBannerPanel;
        cardAnimation.Duration = _durationCard;
        cardAnimation.CurrentDuration = _durationCurrentCard;
        gameController.Duration = _durationGameController;
        inventoryAnimation.Duration = _inventoryDuration;
    }

    private void SetFastDuration()
    {
        iconSpeed.sprite = iconFast;
        bannerAnimation.Delay /= 2;
        bannerAnimation.TextDuration *= 0.5f;
        bannerAnimation.AvatarDuration *= 0.5f;
        cardAnimation.Duration *= 0.5f;
        cardAnimation.CurrentDuration *= 0.5f;
        gameController.Duration *= 0.5f;
        inventoryAnimation.Duration *= 0.5f;
    }

    public void ClickButton()
    {
        switch (durationType)
        {
            case DurationType.None:
                durationType = DurationType.Normal;
                SetNormalDuration();
                break;

            case DurationType.Normal:
                durationType = DurationType.Fast;
                SetFastDuration();
                break;

            case DurationType.Fast:
                durationType = DurationType.None;
                SetNoneDuration();
                break;

            default:
                Debug.LogWarning("Неизвестный тип!");
                break;
        }
    }
}

    public enum DurationType { None, Normal, Fast }
