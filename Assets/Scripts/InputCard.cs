using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private SoundController soundController;
    [SerializeField] private GameController gameController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CardType cardType;
    [SerializeField] private Animator _animator;

    private Transform _transform;
    private AnimationClip _animationClip;
    private RuntimeAnimatorController _animatorController;
    private bool _isClick = false;

    public AnimationClip AnimationClip { get => _animationClip; set => _animationClip = value; }
    public RuntimeAnimatorController AnimatorController { get => _animatorController; set => _animatorController = value; }
    public bool IsClick { get => _isClick; set => _isClick = value; }

    private async void Start()
    {
        _transform = transform;
        // ���� ���� ����, ����� ���������, ��� ��� ���������� ����������������
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
            _animator.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickCard();
    }

    private void ClickCard()
    {
        if (IsClick) return;

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

    private async void ClickReverse()
    {
        IsClick = true;
        bool full = inventoryController.CheckFullInventory();

        contentCard.ChangeCountHP();

        if (full) await inventoryController.WaitForBagHide();
        else await contentCard.SetItemToInventory();

        ContinueAfterHideBag();
    }

    public async void ContinueAfterHideBag()
    {
        await contentCard.RemoveItemFromInventory();
        soundController.PlaySoundClip(contentCard.ReverseAudioClip);
        await cardAnimation.HoldReverseCard();

        await contentCard.HoldBanner();
        await contentCard.ChangeLocationSprite();
        if (contentCard.IsWin)
        {
            Debug.Log("WIN!");
            await gameController.ShowWin();
        }
        else
        {
            contentCard.SetContent(contentCard.NextCardSetScriptableObject);

            await UniTask.Delay(600);
            await contentCard.ShowBanner();
            await cardAnimation.ShowCards();
            IsClick = false;
        }
    }

    // ����� ��� �������� ������� ������� � ������ OnPointerEnter ��� �������������
    public void CheckCursorPosition()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // ������� ������ ��� �������� ����������� ��������
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // ���������, �������� �� ������ ������ ������
        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
            {
                OnPointerEnter(pointerData); // ������������� �������� ������� OnPointerEnter
                break;
            }
        }
    }
}
