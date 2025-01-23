using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private Image locationBG;
    [SerializeField] private HPController hPController;
    [SerializeField] private MapController mapController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private ContentCard contentCard;

    private const string SaveFilePath = "/SaveData/GameProgress.json";

    public async UniTask SaveGameProgress(SaveButton saveSlot)
    {
        // Собираем данные прогресса
        saveSlot.GameData.Health = hPController.GetCurrentHP();
        saveSlot.GameData.CheckpointCount = mapController.GetCountCheckPoints();

        // Используем Addressable AssetReference для локации
        saveSlot.GameData.Location = locationBG.sprite != null ? new AssetReferenceSprite(locationBG.sprite.name) : null;
        // Используем Addressable AssetReference для CardSet
        saveSlot.GameData.CardSet = contentCard.CardSetScriptableObject != null ? new AssetReference(contentCard.CardSetScriptableObject.name) : null;

        // Получаем список адресов предметов из инвентаря
        saveSlot.GameData.Items = GetAssetReferences(inventoryController.GetInventorySprites());

        await UniTask.Yield();

        // Сохраняем прогресс в файл JSON
        SaveGameProgress(saveSlot.GameData);
    }

    // Получаем список AssetReference для предметов инвентаря
    private List<AssetReferenceSprite> GetAssetReferences(List<Sprite> sprites)
    {
        var assetReferences = new List<AssetReferenceSprite>();
        foreach (var sprite in sprites)
        {
            if (sprite != null)
            {
                assetReferences.Add(new AssetReferenceSprite(sprite.name));
            }
        }
        return assetReferences;
    }

    public void SaveGameProgress(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData, true); // Используем JsonUtility
        string fullPath = Application.persistentDataPath + SaveFilePath;

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // Создаем папку, если она не существует
        File.WriteAllText(fullPath, jsonData);
        Debug.Log("Прогресс игры сохранен!");
    }

    // Загрузка данных из файла JSON
    public GameData LoadGameProgress()
    {
        string fullPath = Application.persistentDataPath + SaveFilePath;

        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData); // Используем JsonUtility
            Debug.Log("Прогресс игры загружен!");
            return gameData;
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден!");
            return null;
        }
    }
}

[Serializable]
public class GameData
{
    public int Health;
    public AssetReferenceSprite Location;
    public List<AssetReferenceSprite> Items;
    public int CheckpointCount;
    public AssetReference CardSet;
}
