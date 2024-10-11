using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject prefabSaveButton;
    [SerializeField] private Transform contentParent, dialoguePanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private List<SaveButton> buttons;

    private GameObject _currentSaveButton;
    private static readonly string ScreenshotPath = Application.dataPath + "/SaveData/Screenshots/";

    private void Awake()
    {
        EnsureScreenshotDirectoryExists();
    }

    private void EnsureScreenshotDirectoryExists()
    {
        if (!Directory.Exists(ScreenshotPath))
        {
            Directory.CreateDirectory(ScreenshotPath);
        }
    }

    public List<SaveButton> Buttons { get => buttons; set => buttons = value; }

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
        foreach (var button in Buttons)
        {
            button.enabled = on;
            button.GetComponent<ButtonAnimation>().enabled = on;
        }
    }

    public void SelectButton(GameObject saveButton)
    {
        _currentSaveButton = saveButton;
    }

    public async void CreateSaveButton()
    {
        NoTextAnimation(inputField.text != "");

        if (inputField.text != "")
        {
            GameObject go = Instantiate(prefabSaveButton, contentParent);
            SaveButton saveButton = go.GetComponent<SaveButton>();
            buttons.Add(saveButton);
            saveButton.NameSave.text = inputField.text;
            saveButton.DataTime.text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Ждем создания скриншота
            saveButton.Image.sprite = await CreateScreenshot();

            go.SetActive(false);
            dialoguePanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                go.SetActive(true);
                go.transform.DOScale(0, 0.3f).From().SetEase(Ease.OutBack).OnComplete(() =>
                {
                    saveButton.IsSelected = false;
                    inputField.text = "";
                });
            });
        }
        else Debug.Log("Введите имя!");
    }

    private void NoTextAnimation(bool run)
    {
        Color color = textField.color;

        if (run)
        {
            textField.DOKill();
            dialoguePanel.DOKill();
            textField.color = color;
        }
        else MoveDialogueFail();
    }

    private void MoveDialogueFail()
    {
        var sequence = DOTween.Sequence();
        textField.DOColor(new Color(0, 0, 0), 0.2f).SetLoops(4, LoopType.Yoyo);
        for (int i = 0; i < 3; i++)
        {
            sequence.Append(dialoguePanel.DOMoveX(dialoguePanel.position.x + 0.3f, 0.05f))
                           .Append(dialoguePanel.DOMoveX(dialoguePanel.position.x, 0.05f))
                           .Append(dialoguePanel.DOMoveX(dialoguePanel.position.x - 0.3f, 0.05f))
                           .Append(dialoguePanel.DOMoveX(dialoguePanel.position.x, 0.05f));
        }
    }

    private async UniTask<Sprite> CreateScreenshot()
    {
        EnsureScreenshotDirectoryExists();
        string filePath = $"{ScreenshotPath}/Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        gameController.ClearForScreenshot();
        ScreenCapture.CaptureScreenshot(filePath);
        await UniTask.Delay(1);
        gameController.CompleteScreenshot();

        // Ждем, пока файл не появится
        await UniTask.WaitUntil(() => File.Exists(filePath));

        // Загрузка файла как текстуры и создание спрайта
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        Sprite screenshotSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        // Удаляем файл после создания спрайта
        File.Delete(filePath);

        return screenshotSprite;
    }

    public void DestroySaveButton()
    {
        if (_currentSaveButton != null)
        {
            buttons.Remove(_currentSaveButton.GetComponent<SaveButton>());
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
