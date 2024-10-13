using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
        // �������� ������ ���������
        saveSlot.GameData.Health = hPController.GetCurrentHP();
        saveSlot.GameData.CheckpointCount = mapController.GetCountCheckPoints();

        // �������� AssetReference ����� ��� ���� �������
        saveSlot.GameData.Location = GetAssetReference(locationBG.sprite);

        // �������� AssetReference ����� ��� CardSet
        saveSlot.GameData.CardSet = GetAssetReference(contentCard.CardSetScriptableObject);

        // �������� ������ ������� ��������� �� ���������
        saveSlot.GameData.Items = GetAssetReferences(inventoryController.GetInventorySprites());

        await UniTask.Yield();

        // ��������� �������� � ���� JSON
        SaveGameProgress(saveSlot.GameData);
    }

    // ����� ��� ��������� ������ ������� �� ������� Sprite
    private AssetReferenceSprite GetAssetReference(Sprite sprite)
    {
        return new AssetReferenceSprite(AssetDatabase.GetAssetPath(sprite));
    }

    // ����� ��� ��������� ������ ������� �� ScriptableObject
    private AssetReference GetAssetReference(ScriptableObject scriptableObject)
    {
        return new AssetReference(AssetDatabase.GetAssetPath(scriptableObject));
    }

    // �������� ������ AssetReference ��� ��������� ���������
    private List<AssetReferenceSprite> GetAssetReferences(List<Sprite> sprites)
    {
        var assetReferences = new List<AssetReferenceSprite>();
        foreach (var sprite in sprites)
        {
            assetReferences.Add(GetAssetReference(sprite));
        }
        return assetReferences;
    }

    public void SaveGameProgress(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData, true); // ���������� JsonUtility
        string fullPath = Application.persistentDataPath + SaveFilePath;

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // ������� �����, ���� ��� �� ����������
        File.WriteAllText(fullPath, jsonData);
        Debug.Log("�������� ���� ��������!");
    }

    // �������� ������ �� ����� JSON
    public GameData LoadGameProgress()
    {
        string fullPath = Application.persistentDataPath + SaveFilePath;

        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData); // ���������� JsonUtility
            Debug.Log("�������� ���� ��������!");
            return gameData;
        }
        else
        {
            Debug.LogWarning("���� ���������� �� ������!");
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
