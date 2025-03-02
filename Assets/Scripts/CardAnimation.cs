using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Transform leftCard, rightCard, reverseCard;
    [SerializeField] private GameController gameController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private float duration, currentDuration;

    private Vector3 _normalPosLeft, _normalPosRight, _normalPosReverse;
    private InputCard _inputCard_L, _inputCard_R, _inputCard_Rev;

    public float Duration { get => duration; set => duration = value; }
    public float CurrentDuration { get => currentDuration; set => currentDuration = value; }

    private void Awake()
    {
        _inputCard_L = leftCard.GetComponent<InputCard>();
        _inputCard_R = rightCard.GetComponent<InputCard>();
        _inputCard_Rev = reverseCard.GetComponent<InputCard>();
    }
    private void Start()
    {
        _normalPosLeft = leftCard.position;
        _normalPosRight = rightCard.position;
        _normalPosReverse = reverseCard.position;
        
        HideCard();
        leftCard.gameObject.SetActive(true);
        rightCard.gameObject.SetActive(true);
        InputCardActivate(false);
    }

    private void InputCardActivate(bool active)
    {
        _inputCard_L.enabled = active;
        _inputCard_R.enabled = active;
        _inputCard_Rev.enabled = active;
    }

    public async UniTask ShowCards()
    {
        var taskCompletionSource = new UniTaskCompletionSource();

        InputCardActivate(false);
        soundController.PlayFlip();
        leftCard.DOMove(_normalPosLeft, Duration, false).SetEase(Ease.OutBack);
        rightCard.DOMove(_normalPosRight, Duration, false).SetEase(Ease.OutBack);
        leftCard.DOScale(0, Duration).From();
        rightCard.DOScale(0, Duration).From().OnComplete(() =>
        {
            InputCardActivate(true);

            // Проверяем позицию курсора после активации карт
            _inputCard_L.CheckCursorPosition();
            _inputCard_R.CheckCursorPosition();

            taskCompletionSource.TrySetResult(); // Завершаем таск
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }


    public void HideCard()
    {
        InputCardActivate(false);
        reverseCard.position = _normalPosReverse;
        leftCard.position = reverseCard.position;
        rightCard.position = reverseCard.position;
    }

    public void FlipCard(CardType cardType)
    {
        Transform currentCard = null;
        Transform holdCard = null;
        
        switch (cardType)
        {
            case CardType.Left:
                currentCard = leftCard;
                holdCard = rightCard;
                break;
            case CardType.Right:
                currentCard = rightCard;
                holdCard = leftCard;
                break;
        }

        InputCardActivate(false);
        soundController.PlayFlip();
        holdCard.DOMove(_normalPosReverse, Duration, false).SetEase(Ease.InBack);
        holdCard.DOScale(0.001f, Duration);
        currentCard.DOScale(Vector3.one, CurrentDuration).OnComplete(() => 
        {
            currentCard.DOMoveX(0, Duration, false).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                currentCard.DOScaleX(0.001f, CurrentDuration).OnComplete(() =>
                {
                    reverseCard.localScale = Vector3.one;
                    reverseCard.position = currentCard.position;
                    _inputCard_Rev.IsClick = false;
                    soundController.PlayReverseFlip();

                    reverseCard.DOScaleX(0.001f, CurrentDuration).From().OnComplete(() =>
                    {
                        _inputCard_Rev.CheckCursorPosition();
                        holdCard.localScale = Vector3.one;
                        currentCard.position = _normalPosReverse;
                        currentCard.localScale = Vector3.one;
                        InputCardActivate(true);
                    });
                });
            });
        });
    }

    public async UniTask HoldReverseCard()
    {
        InputCardActivate(false);
        var taskCompletionSource = new UniTaskCompletionSource();
        reverseCard.DOMove(_normalPosReverse, Duration, false).SetEase(Ease.InBack).OnComplete(() =>
        {
            taskCompletionSource.TrySetResult(); // Завершаем таск
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }
}
