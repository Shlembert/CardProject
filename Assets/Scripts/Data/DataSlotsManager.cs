using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DataSlotsManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameDataManager gameDataManager;
    [SerializeField] private GameObject prefabSaveButton;
    [SerializeField] private Transform contentParent, dialoguePanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private List<SaveButton> buttons;

    private GameObject _currentSaveButton;
    private static readonly string ScreenshotPath = Application.dataPath + "/SaveData/Screenshots/";
    private static readonly string SaveSlotsFilePath = Application.dataPath + "/SaveData/saveSlots.json";

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

    public void ShowDialog()
    {
        dialoguePanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            ButtonOn(true);
        });
    }

    public void HideDialog()
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

    // Создаем Слот сохранения
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
            string path = await CreateScreenshot();
            // Устанавливаем спрайт скриншота в Слот
            saveButton.Image.sprite = await LoadScreenShot(path);

            SaveButtonData saveButtonData = saveButton.SaveButtonData;
            saveButtonData.DataTime = saveButton.DataTime.text;
            saveButtonData.Name = saveButton.NameSave.text;
            saveButtonData.ScreenShotAddress = path;

            await gameDataManager.SaveGameProgress(saveButton);

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

    private async UniTask<string> CreateScreenshot()
    {
        EnsureScreenshotDirectoryExists();
        string filePath = $"{ScreenshotPath}/Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";

        int width = Screen.width / 10;
        int height = Screen.height / 10;
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Camera.main.targetTexture = renderTexture;

        gameController.ClearForScreenshot();
        Camera.main.Render();
        gameController.CompleteScreenshot();

        RenderTexture.active = renderTexture;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotTexture.Apply();

        byte[] fileData = screenshotTexture.EncodeToPNG();
        await File.WriteAllBytesAsync(filePath, fileData);

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(screenshotTexture);

        return filePath;
    }

    public async UniTask<Sprite> LoadScreenShot(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Файл не найден по пути: {filePath}");
            return null;
        }

        byte[] fileData = await File.ReadAllBytesAsync(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        Sprite screenshotSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return screenshotSprite;
    }

    public void DestroySaveButton()
    {
        if (_currentSaveButton != null)
        {
            buttons.Remove(_currentSaveButton.GetComponent<SaveButton>());
            _currentSaveButton.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                SaveButtonData saveButtonData = _currentSaveButton.GetComponent<SaveButtonData>();
                string path = saveButtonData.ScreenShotAddress;
                DeleteScreenshotBinary(path);
                Destroy(_currentSaveButton);
                _currentSaveButton = null;
            });
        }
        else Debug.Log("Не выбрано сохранение!");
    }

    private void DeleteScreenshotBinary(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Файл {filePath} успешно удален.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при удалении файла {filePath}: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"Файл {filePath} не найден.");
        }
    }
}



[Serializable]
public class SaveButtonData
{
    public string Name;
    public string DataTime;
    public string ScreenShotAddress; // Изменено на string
}
