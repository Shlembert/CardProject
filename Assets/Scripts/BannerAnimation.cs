using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BannerAnimation : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Transform banner, panel;
    [SerializeField] private float posHold, posShow;
    [SerializeField] private Vector3 _normalPos, _holdPos;

    private float _normalPosX;
    private Color _normalColor;

    private void Start()
    {
        _normalPos = panel.position;
        panel.position = _holdPos;
        Vector3 startPos = new Vector3(posHold, banner.transform.position.y, banner.position.z);
        banner.position = startPos;
    }

    public async UniTask ShowBanner()
    {
        // ���������� UniTaskCompletionSource ��� ���������� ����� ����� ��������
        var taskCompletionSource = new UniTaskCompletionSource();

        panel.position = _normalPos;

        panel.DOScale(0, 0.7f).SetEase(Ease.OutBack).From().OnComplete(() =>
        {
            banner.DOMoveX(posShow, 0.5f, false).OnComplete(() =>
            {
                taskCompletionSource.TrySetResult(); // ��������� ����
            });
        });

        await taskCompletionSource.Task; // ������� ���������� ��������
    }

    public async UniTask HideBanner()
    {
        // ���������� UniTaskCompletionSource ��� ���������� ����� ����� ��������
        var taskCompletionSource = new UniTaskCompletionSource();

        banner.DOMoveX(posHold, 0.5f, false).OnComplete(() =>
        {
            panel.DOScale(0, 0.7f).SetEase(Ease.InBack).OnComplete(() =>
            {
                panel.position = _holdPos;
                panel.localScale = Vector3.one;

                taskCompletionSource.TrySetResult(); // ��������� ����
            });
        });
        await taskCompletionSource.Task; // ������� ���������� ��������
    }

    public async UniTask ShowMessageBanner()
    {
        // ���������� UniTaskCompletionSource ��� ���������� ����� ����� ��������
        var taskCompletionSource = new UniTaskCompletionSource();

        banner.DOMoveX(posShow, 0.5f, false).OnComplete(() =>
        {
            taskCompletionSource.TrySetResult(); // ��������� ����
        });
        await taskCompletionSource.Task; // ������� ���������� ��������
    }

    public async UniTask HideMessageBanner()
    {
        // ���������� UniTaskCompletionSource ��� ���������� ����� ����� ��������
        var taskCompletionSource = new UniTaskCompletionSource();

        banner.DOMoveX(posHold, 0.5f, false).OnComplete(() =>
        {
            taskCompletionSource.TrySetResult(); // ��������� ����
        });
        await taskCompletionSource.Task; // ������� ���������� ��������
    }
}
