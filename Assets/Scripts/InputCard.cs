using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private GameController gameController;
    [SerializeField] private CardType cardType;
    [SerializeField] private Animator _animator;

    private Transform _transform;
    private AnimationClip _animationClip;
    private AnimatorController _animatorController;

    public AnimationClip AnimationClip { get => _animationClip; set => _animationClip = value; }
    public AnimatorController AnimatorController { get => _animatorController; set => _animatorController = value; }

    private void Start()
    {
        _transform = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _transform.DOScale(_transform.localScale * 1.1f, 0.3f);
        // Анимация при наведении
        if (_animationClip != null && _animator != null)
        {
            _animator.runtimeAnimatorController = AnimatorController;
            _animator.enabled = true;
            _animator.Play(_animationClip.name);
            _animator.speed = 1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _transform.DOScale(Vector3.one, 0.3f);
        // Отключение анимации с переходом на первый кадр
        if (_animationClip != null && _animator != null)
        {
            _animator.Play(_animationClip.name, 0, 0f); // Вернуть на первый кадр
            _animator.speed = 0f; // Остановить анимацию
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickCard();
    }

    private void ClickCard()
    {
        if (_animator) _animator.enabled = false;

        if (cardType != CardType.Reverse)
        {
            contentCard.CheckRequestItem(cardType);
            cardAnimation.FlipCard(cardType);
        }
        else
        {
            ClickReverse();
        }
    }

    // Клик по реверсу карты с последующим воспроизведением сценария
    private async void ClickReverse()
    {
        contentCard.ChangeCountHP();

        if (contentCard.RemoveItem) await contentCard.RemoveItemFromInventory();
        if (gameController.IsGameOver)
        {
            gameController.IsGameOver = false;
            return;
        }

        await contentCard.SetItemToInventory();
        await cardAnimation.HoldReverseCard();
        await contentCard.HoldBanner();
        await contentCard.ChangeLocationSprite();
        contentCard.SetContent(contentCard.NextCardSetScriptableObject);

        await UniTask.Delay(600);
        await contentCard.ShowBanner();
        await cardAnimation.ShowCards();
    }
}
