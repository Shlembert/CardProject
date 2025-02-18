using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private HPController hpController;
    [SerializeField] private Image locationImage;

    public void SaveData(string saveName)
    {
        Data data = new Data
        {
            HPCount = hpController.GetCurrentHP(),
            IndexCheckPoint = 1, // Здесь нужно указать реальный индекс чекпоинта
            Items = GetInventorySpritesPaths(),
            Location = locationImage.sprite != null ? locationImage.sprite.name : null,
            ScreenShot = TakeScreenshot()
        };

        // Сохранение в PlayerPrefs
        Data.Save(data);

        // Также можно сохранить дополнительную информацию о сохранении (имя, дата и т.д.)
        PlayerPrefs.SetString("SaveName", saveName);
        PlayerPrefs.SetString("SaveDate", System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Data data = Data.Load();

        if (data != null)
        {
            hpController.ChangeCountHP(data.HPCount);
            inventoryController.LoadInventorySprites(LoadSpritesFromPaths(data.Items));
            locationImage.sprite = Data.LoadSprite(data.Location);
            // Также можно загрузить дату и имя сохранения, если это требуется
        }
    }

    private List<string> GetInventorySpritesPaths()
    {
        List<string> paths = new List<string>();
        foreach (Sprite sprite in inventoryController.GetInventorySprites())
        {
            paths.Add(sprite.name); // Убедитесь, что пути к спрайтам корректно возвращаются
        }
        return paths;
    }

    private List<Sprite> LoadSpritesFromPaths(List<string> paths)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (string path in paths)
        {
            Sprite sprite = Data.LoadSprite(path);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
        }
        return sprites;
    }

    private string TakeScreenshot()
    {
        string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string path = System.IO.Path.Combine(Application.persistentDataPath, screenshotName);

        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("Screenshot saved to: " + path);

        return screenshotName;
    }
}
