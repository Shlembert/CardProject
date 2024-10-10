using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabSaveButton;
    [SerializeField] private Transform contentParent, dialoguePanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private List<Button> buttons;
    private GameObject _currentSaveButton;

    private void Start()
    {
        inputField.characterLimit = 20;
    }
    public void ShowDialogue()
    {
        dialoguePanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            ButtonOn(true);
        });
    }

    public void HideDialogue()
    {
        ButtonOn(false);
        dialoguePanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
    }

    private void ButtonOn(bool on)
    {
        foreach (var button in buttons)
        {
            button.enabled = on;
            button.GetComponent<ButtonAnimation>().enabled = on;
        }
    }

    public void SelectButton(GameObject saveButton)
    {
        _currentSaveButton = saveButton;
    }

    public void CreateSaveButton()
    {
        if (inputField.text != "")
        {
            GameObject go = Instantiate(prefabSaveButton, contentParent);
            SaveButton saveButton = go.GetComponent<SaveButton>();
            
            saveButton.NameSave.text = inputField.text;
            saveButton.DataTime.text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            saveButton.Image.sprite = null;
            go.SetActive(false);
            dialoguePanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                go.SetActive(true);
                go.transform.DOScale(0, 0.3f).From().SetEase(Ease.OutBack).OnComplete(() =>
                {
                    inputField.text = "";
                });
            });

            // Очищаем поле ввода после создания сохранения
            
        }
        else Debug.Log("Введите имя!");
    }

    public void DestroySaveButton()
    {
        if (_currentSaveButton != null)
        {
            _currentSaveButton.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(_currentSaveButton);
                _currentSaveButton = null;
            });
        }
        else Debug.Log("Не выбрано сохранение!");
    }
}

[Serializable]
public class GameData
{
    public int Health;
    public float MusicVolume;
    public float SoundVolume;
    public bool MusicMute;
    public bool SoundMute;
    public string Name;
    public string DataTime;
    public Sprite Screenshot;
    public Sprite Location;
    public List<Sprite> Items;
    public List<GameObject> Checkpoints;
    public List<GameObject> SaveButtons;
    public CardSetScriptableObject CardSetScriptableObject;
}
