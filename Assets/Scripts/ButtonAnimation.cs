using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       _transform.DOScale(_transform.localScale * 1.1f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _transform.DOScale(Vector3.one, 0.3f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _transform.DOScale(_transform.localScale * 0.9f, 0.3f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _transform.DOScale(Vector3.one, 0.3f);
    }
}
