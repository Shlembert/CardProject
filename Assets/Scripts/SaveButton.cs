using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private Image image, body;
    [SerializeField] private TMP_Text nameSave, dataTime;
    [SerializeField] private Color originColor, selectedColor;

    private Transform _transform;
    private bool _isSelected = true;

    public Image Image { get => image; set => image = value; }
    public TMP_Text NameSave { get => nameSave; set => nameSave = value; }
    public TMP_Text DataTime { get => dataTime; set => dataTime = value; }
    public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    private void Start()
    {
        _transform = transform;
    }

    public void SetOrigColor()
    {
        body.color = originColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_isSelected) return;
        foreach (SaveButton button in dataManager.Buttons) 
        { 
            button.IsSelected = false; 
            button.SetOrigColor();
        }

        _isSelected = true;
        body.color = selectedColor;
        _transform.DOScale(Vector3.one, 0.3f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        _transform.DOScale(_transform.localScale * 1.1f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        _transform.DOScale(Vector3.one, 0.3f);
    }
}
