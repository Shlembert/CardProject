using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private SoundController soundController;
    [SerializeField] private GameController gameController;
    [SerializeField] private CardType cardType;
    [SerializeField] private Animator _animator;

    private Transform _transform;
    private AnimationClip _animationClip;
    private AnimatorController _animatorController;
    private bool _isClick = false;
    public AnimationClip AnimationClip { get => _animationClip; set => _animationClip = value; }
    public AnimatorController AnimatorController { get => _animatorController; set => _animatorController = value; }
    public bool IsClick { get => _isClick; set => _isClick = value; }

    private async void Start()
    {
        _transform = transform;
        // Ждем один кадр, чтобы убедиться, что все компоненты инициализированы
        await UniTask.Yield();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_transform == null) return;

        _transform.DOScale(_transform.localScale * 1.1f, 0.3f);

        if (_animator != null && _animationClip != null && _animatorController != null)
        {
            _animator.runtimeAnimatorController = AnimatorController;
            _animator.enabled = true;
            _animator.Play(_animationClip.name);
            _animator.speed = 1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_transform == null) return;

        _transform.DOScale(Vector3.one, 0.3f);

        if (_animator != null && _animationClip != null && _animator.isActiveAndEnabled)
        {
            _animator.Play(_animationClip.name, 0, 0f);
            _animator.speed = 0f;
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
            if (IsClick) return;
            ClickReverse();
        }
    }

    private async void ClickReverse()
    {
        IsClick = true;

        if (gameController.IsGameOver){ gameController.IsGameOver = false; return; }

        contentCard.ChangeCountHP();
        await contentCard.SetItemToInventory();
        await contentCard.RemoveItemFromInventory();

        soundController.PlaySoundClip(contentCard.ReverseAudioClip);
        await cardAnimation.HoldReverseCard();
       
        await contentCard.HoldBanner();
        await contentCard.ChangeLocationSprite();
        contentCard.SetContent(contentCard.NextCardSetScriptableObject);

        await UniTask.Delay(600);
        await contentCard.ShowBanner();
        await cardAnimation.ShowCards();
        IsClick = false;
    }

    // Метод для проверки позиции курсора и вызова OnPointerEnter при необходимости
    public void CheckCursorPosition()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Создаем список для хранения результатов рэйкаста
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Проверяем, содержит ли список данный объект
        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
            {
                OnPointerEnter(pointerData); // Принудительно вызываем событие OnPointerEnter
                break;
            }
        }
    }
}
