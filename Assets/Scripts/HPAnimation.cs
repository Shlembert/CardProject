using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HPAnimation : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private GameController gameController;

    private Color _normalColor;

    public void AddLife(Image image)
    {
        Debug.Log($"Анимация добавления");
        Transform transform = image.GetComponent<Transform>();

        image.enabled = true;
        transform.DOScale(transform.localScale * 2, duration).From();
        image.DOFade(0, duration).From();

    }

    public  void RemoveLife(Image image) 
    {
        Transform transform = image.GetComponent<Transform>();
        _normalColor = image.color;

        transform.DOScale(transform.localScale * 2, duration);
        image.DOFade(0, duration).OnComplete(() =>
        {
            image.enabled = false;
            transform.localScale = Vector3.one;
            image.color = _normalColor;

        });
    }
}
