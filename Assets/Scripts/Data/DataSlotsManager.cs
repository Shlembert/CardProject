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
    public GameObject CurrentSaveButton { get => _currentSaveButton; set => _currentSaveButton = value; }

    private async void Start()
    {
        inputField.characterLimit = 20;
        await LoadSlotsFromJson(); // Загрузка слотов при старте игры
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
        CurrentSaveButton = saveButton;
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
            saveButtonData.GameData = saveButton.GameData;

            go.SetActive(false);
            dialoguePanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                go.SetActive(true);
                go.transform.DOScale(0, 0.3f).From().SetEase(Ease.OutBack).OnComplete(async () =>
                {
                    saveButton.IsSelected = false;
                    await SaveAllSlotsToJson(); // Сохранение после добавления нового слота

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

        int width = Screen.width / 5;
        int height = Screen.height / 5;
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Camera.main.targetTexture = renderTexture;

        gameController.ClearForScreenshot();
        Camera.main.Render();
        gameController.CompleteScreenshot();

        RenderTexture.active = renderTexture;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotTexture.Apply();

        // Асинхронно получаем данные текстуры
        byte[] fileData = screenshotTexture.EncodeToPNG();
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(screenshotTexture);

        // Записываем файл асинхронно
        await UniTask.SwitchToThreadPool(); // Переход на пул потоков
        await File.WriteAllBytesAsync(filePath, fileData);
        await UniTask.SwitchToMainThread(); // Возврат в основной поток

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

    public async UniTask SaveAllSlotsToJson()
    {
        List<SaveButtonData> saveSlots = new List<SaveButtonData>();

        foreach (var button in buttons)
        {
            SaveButton slotData = button;
            Debug.Log($"Данные записанные в json: \n Name:{slotData.SaveButtonData.Name} || Data: {slotData.SaveButtonData.DataTime} || Adress img: {slotData.SaveButtonData.ScreenShotAddress}");
            saveSlots.Add(slotData.SaveButtonData);
        }

        string json = JsonUtility.ToJson(new SaveButtonDataList(saveSlots), true);
        await File.WriteAllTextAsync(SaveSlotsFilePath, json);

        Debug.Log("Все слоты успешно сохранены в JSON.");
    }

    // Метод для загрузки всех слотов из JSON
    public async UniTask LoadSlotsFromJson()
    {
        if (File.Exists(SaveSlotsFilePath))
        {
            string json = await File.ReadAllTextAsync(SaveSlotsFilePath);
            SaveButtonDataList saveSlotsData = JsonUtility.FromJson<SaveButtonDataList>(json);

            foreach (var slotData in saveSlotsData.Slots)
            {
                GameObject go = Instantiate(prefabSaveButton, contentParent);
                SaveButton saveButton = go.GetComponent<SaveButton>();
                buttons.Add(saveButton);

                saveButton.NameSave.text = slotData.Name;
                saveButton.DataTime.text = slotData.DataTime;
                saveButton.SaveButtonData = slotData;

                // Загружаем скриншот асинхронно
                saveButton.Image.sprite = await LoadScreenShot(slotData.ScreenShotAddress);
                saveButton.GameData = slotData.GameData;
               
                // Устанавливаем слот неактивным, если нужно
                go.SetActive(false);
                go.transform.DOScale(0, 0.3f).From().SetEase(Ease.OutBack).OnComplete(() =>
                {
                    go.SetActive(true);
                    saveButton.IsSelected = false;
                });
            }

            Debug.Log("Слоты успешно загружены из JSON.");
        }
        else
        {
            Debug.Log("Файл сохранений не найден.");
        }
    }


    public void DestroySaveButton()
    {
        if (CurrentSaveButton != null)
        {
            buttons.Remove(CurrentSaveButton.GetComponent<SaveButton>());
            CurrentSaveButton.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                // Получаем SaveButton и затем доступ к SaveButtonData
                SaveButton saveButton = CurrentSaveButton.GetComponent<SaveButton>();
                SaveButtonData saveButtonData = saveButton.SaveButtonData;

                string path = saveButtonData.ScreenShotAddress;
                DeleteScreenshotBinary(path);
                // Сохранение после удаления слота
                SaveAllSlotsToJson().Forget();
                Destroy(CurrentSaveButton);
                CurrentSaveButton = null;
            });
        }
        else
        {
            Debug.Log("Не выбрано сохранение!");
        }
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
public class SaveButtonDataList
{
    public List<SaveButtonData> Slots;
    public SaveButtonDataList(List<SaveButtonData> slots)
    {
        Slots = slots;
    }
}

[Serializable]
public class SaveButtonData
{
    public string Name;
    public string DataTime;
    public string ScreenShotAddress; 
    public GameData GameData; 
}

