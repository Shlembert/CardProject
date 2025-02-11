using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private Image bG;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private BannerAnimation bannerAnimation;
    [SerializeField] private GameController gameController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private HPController hpController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private DataSlotsManager dataSlotsManager;

    private Transform _transform;
    private bool _isClick = false;

    private void Start()
    {
        _transform = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isClick) return;
        _transform.DOScale(Vector3.one * 1.2f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isClick) return;
        _transform.DOScale(Vector3.one, 0.3f);
    }

    public async void LoadInventorySpritesFromGameData(GameData gameData)
    {
        List<Sprite> items = new List<Sprite>();

        foreach (var itemAddress in gameData.Items)
        {
            // ��������� ������ ���������� �� ������
            var handle = Addressables.LoadAssetAsync<Sprite>(itemAddress);
            await handle.Task; // ���� ���������� ��������

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite sprite = handle.Result;
                items.Add(sprite);
            }
            else
            {
                Debug.LogError($"�� ������� ��������� ������ �� ������: {itemAddress}");
            }
           // Addressables.Release(handle);
        }
        // ������ �������� ����������� ������� � ���������� ���������
        inventoryController.LoadInventorySprites(items);
    }

    public async Task<CardSetScriptableObject> LoadCardSetAsync(GameData gameData)
    {
        // �����������, ��� � gameData ���� �������� CardSet, ������� �������� AssetReference
        var handle = gameData.CardSet.LoadAssetAsync<CardSetScriptableObject>();
        await handle.Task; // ���� ���������� ��������

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            CardSetScriptableObject cardSet = handle.Result;
           // Addressables.Release(handle);
            return cardSet; // ���������� ����������� ScriptableObject
        }
        else
        {
            Debug.LogError($"�� ������� ��������� CardSet �� ������: {gameData.CardSet}");
            return null; // ��� ��������� ������ ��-������
        }
    }

    public async Task<Sprite> LoadLocationSpriteAsync(GameData gameData)
    {
        // �����������, ��� � gameData ���� �������� Location, ������� �������� AssetReferenceSprite
        var handle = gameData.Location.LoadAssetAsync<Sprite>();
        await handle.Task; // ���� ���������� ��������

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite sprite = handle.Result;
           // Addressables.Release(handle);
            return sprite; // ���������� ����������� ������
        }
        else
        {
            Debug.LogError($"�� ������� ��������� ������ �� ������: {gameData.Location}");
            return null; // ��� ��������� ������ ��-������
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isClick) return; 
        _transform.DOScale(Vector3.one, 0.2f);
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        if (_isClick) return; _isClick = true;

        if (dataSlotsManager.CurrentSaveButton != null)
        {
            SaveButton saveButton = dataSlotsManager.CurrentSaveButton.GetComponent<SaveButton>();
            GameData gameData = saveButton.GameData;

            soundController.PlayClick();
            cardAnimation.HideCard();
            await bannerAnimation.HideBanner();
            hpController.SetHP(gameData.Health);
            inventoryController.ClearInventory();

            LoadInventorySpritesFromGameData(gameData);

            contentCard.SetContent(await LoadCardSetAsync(gameData));

            bG.sprite = await LoadLocationSpriteAsync(gameData);

            gameController.GameLoad();

            await bannerAnimation.ShowBanner();
            await cardAnimation.ShowCards();
        }
        _isClick = false;
    }
}
