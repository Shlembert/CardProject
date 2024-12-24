using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BannerAnimation : MonoBehaviour
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private SoundController soundController;
    [SerializeField] private Image portrait;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform textPanel, portret;
    [SerializeField] private float posHold, posShow;
    [SerializeField] private Vector3 _normalPos, _holdPos;
    [SerializeField] private int delay;

    private RuntimeAnimatorController _animatorController;
    private AnimationClip _animationClip;

    public RuntimeAnimatorController AnimatorController { get => _animatorController; set => _animatorController = value; }
    public AnimationClip AnimationClip { get => _animationClip; set => _animationClip = value; }

    private void Start()
    {
        _normalPos = portret.position;
        portret.position = _holdPos;
        Vector3 startPos = new Vector3(posHold, textPanel.transform.position.y, textPanel.position.z);
        textPanel.position = startPos;
    }

    public async UniTask ShowBanner()
    {
        var taskCompletionSource = new UniTaskCompletionSource();

        portret.position = _normalPos;

        portret.DOScale(0, 0.7f).SetEase(Ease.OutBack).From().OnComplete(async () =>
        {
           if(contentCard.CardSetScriptableObject.bannerAnimator !=null) PlayAnimation();
            await ShowMessageBanner();
            await UniTask.Delay(delay);
            taskCompletionSource.TrySetResult();  // Завершаем таск
        });

        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }

    public async UniTask HideBanner()
    {
        var taskCompletionSource = new UniTaskCompletionSource();
        StopAnimation();

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

    private void PlayAnimation()
    {
        if (_animatorController != null && _animationClip != null)
        {
            animator.runtimeAnimatorController = _animatorController;
            animator.enabled = true;
            animator.Play(_animationClip.name);
            animator.speed = 1f;
        }
    }

    public void StopAnimation()
    {
        if (animator != null && _animationClip != null)
        {
            animator.Play(_animationClip.name, 0, 0f);
            animator.speed = 0f;
            animator.enabled = false;
        }
    }

    public async UniTask ShowMessageBanner()
    {
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
        var taskCompletionSource = new UniTaskCompletionSource();

        textPanel.DOMoveX(posHold, 0.5f, false).OnComplete(() =>
        {
            taskCompletionSource.TrySetResult(); // Завершаем таск
        });
        await taskCompletionSource.Task; // Ожидаем завершения анимации
    }
}
