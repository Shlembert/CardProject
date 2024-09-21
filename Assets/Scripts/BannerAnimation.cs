using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BannerAnimation : MonoBehaviour
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private SoundController soundController;
    [SerializeField] private Transform textPanel, portret;
    [SerializeField] private float posHold, posShow;
    [SerializeField] private Vector3 _normalPos, _holdPos;

    private float _normalPosX;
    private Color _normalColor;

    private void Start()
    {
        _normalPos = portret.position;
        portret.position = _holdPos;
        Vector3 startPos = new Vector3(posHold, textPanel.transform.position.y, textPanel.position.z);
        textPanel.position = startPos;
    }

    public async UniTask ShowBanner()
    {
        // Используем UniTaskCompletionSource для завершения таска после анимации
        var taskCompletionSource = new UniTaskCompletionSource();

        portret.position = _normalPos;

        portret.DOScale(0, 0.7f).SetEase(Ease.OutBack).From().OnComplete(async () =>
        {
            await ShowMessageBanner();
            await UniTask.Delay(1000);
            taskCompletionSource.TrySetResult();
        });

        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }

    public async UniTask HideBanner()
    {
        // Используем UniTaskCompletionSource для завершения таска после анимации
        var taskCompletionSource = new UniTaskCompletionSource();

        textPanel.DOMoveX(posHold, 0.5f, false).OnComplete(() =>
        {
            portret.DOScale(0, 0.7f).SetEase(Ease.InBack).OnComplete(() =>
            {
                portret.position = _holdPos;
                portret.localScale = Vector3.one;

                taskCompletionSource.TrySetResult(); // Завершаем таск
            });
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }

    public async UniTask ShowMessageBanner()
    {
        // Используем UniTaskCompletionSource для завершения таска после анимации
        var taskCompletionSource = new UniTaskCompletionSource();
        soundController.PlaySoundClip(contentCard.BannerAudioClip);
        textPanel.DOMoveX(posShow, 0.5f, false).OnComplete(() =>
        {

            taskCompletionSource.TrySetResult(); // Завершаем таск
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }

    public async UniTask HideMessageBanner()
    {
        // Используем UniTaskCompletionSource для завершения таска после анимации
        var taskCompletionSource = new UniTaskCompletionSource();

        textPanel.DOMoveX(posHold, 0.5f, false).OnComplete(() =>
        {
            taskCompletionSource.TrySetResult(); // Завершаем таск
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }
}
