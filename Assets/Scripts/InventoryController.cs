using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InputCard inputCard;
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private List<Image> inventory, fullBag;
    [SerializeField] private Transform fullBagPanel, target;

    private Transform _target, _itemTransform;
    private CardData _cardData;
    private Sprite _sprite, _giveSprite;
    private Image _image;

    private void Awake()
    {
        _image = fullBag[fullBag.Count - 1];
    }

    public Transform Target { get => _target; set => _target = value; }
    public Sprite GiveSprite { get => _giveSprite; set => _giveSprite = value; }

    // Проверка, есть ли предмет
    public bool CheckHaveItem(CardData cardData)
    {
        _cardData = cardData;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].sprite == cardData.requiredItemSprite)
            {
                _itemTransform = inventory[i].transform;
                return true;
            }
        }
        return false;
    }

    public Transform GetTransformItem()
    {
        return _itemTransform;
    }

    // Добавление предмета в инвентарь
    public void AddedItem(Sprite sprite)
    {
        foreach (Image image in inventory)
        {
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = sprite;
                return;
            }
        }
    }

    public Transform GetTarget()
    {
        Transform target = null;

        foreach (Image image in inventory)
        {
            if (!image.enabled) return image.transform;
        }
        return target;
    }

    public void ClearInventory()
    {
        foreach (Image image in inventory)
        {
            image.sprite = null;
            image.enabled = false;
        }
    }

    // Удаление предмета из инвентаря
    public void RemoveItem(Sprite sprite)
    {
        // Находим индекс первого совпадения
        int indexToRemove = -1;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].enabled && inventory[i].sprite == sprite)
            {
                indexToRemove = i;
                inventory[i].sprite = null;
                inventory[i].enabled = false;
                break;
            }
        }

        // Если предмет найден, удаляем его и сортируем инвентарь
        if (indexToRemove != -1)
        {
            inventory[indexToRemove].enabled = false;
            inventory[indexToRemove].sprite = null;

            // Сортируем оставшиеся элементы
            for (int i = indexToRemove; i < inventory.Count - 1; i++)
            {
                inventory[i].sprite = inventory[i + 1].sprite;
                inventory[i].enabled = inventory[i + 1].enabled;
            }

            // Отключаем последнюю ячейку
            inventory[inventory.Count - 1].enabled = false;
            inventory[inventory.Count - 1].sprite = null;
        }
        else
        {
            Debug.LogWarning("Предмет не найден в инвентаре.");
        }
    }

    // Получить список предметов для сохранения
    public List<Sprite> GetInventorySprites()
    {
        List<Sprite> sprites = new List<Sprite>();

        foreach (Image image in inventory)
        {
            if (image.enabled && image.sprite != null)
            {
                sprites.Add(image.sprite);
            }
        }

        return sprites;
    }

    // Загрузить предметы в инвентарь
    public void LoadInventorySprites(List<Sprite> sprites)
    {
        // Сбрасываем инвентарь
        foreach (Image image in inventory)
        {
            image.enabled = false;
            image.sprite = null;
        }

        // Загружаем сохранённые спрайты
        for (int i = 0; i < sprites.Count; i++)
        {
            if (i < inventory.Count)
            {
                inventory[i].sprite = sprites[i];
                inventory[i].enabled = true;
            }
        }
    }

    public bool CheckFullInventory()
    {
        if(!contentCard.IsGiveItem) { return false; }
        foreach (Image image in inventory)
        {
            if (!image.enabled)
            {
                return false;
            }
        }
        // Если инвентарь полностью заполнен
        ShowBag();
        return true;
    }

    private void ShowBag()
    {
        TransitItemToBag();

        fullBagPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            EnableAnimationItemBag(true);
        });
    }

    private TaskCompletionSource<bool> _bagHiddenCompletionSource;

    public Task WaitForBagHide()
    {
        _bagHiddenCompletionSource = new TaskCompletionSource<bool>();
        return _bagHiddenCompletionSource.Task;
    }

    public async void HideBag(bool added)
    {
        if (added) await TransitItemToInventory();

        EnableAnimationItemBag(false);
        fullBagPanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            inputCard.ContinueAfterHideBag();
        });
    }

    public void ChoseItemChange(Transform itemTransform)
    {
        target.DOMoveX(itemTransform.position.x, 0.5f).SetEase(Ease.InOutBack);
        _image = itemTransform.gameObject.GetComponent<Image>();
        Debug.Log(_image.name);
        _sprite = _image.sprite;
    }

    private void TransitItemToBag()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            fullBag[i].sprite = inventory[i].sprite;
        }
    }

    private void EnableAnimationItemBag(bool enable)
    {
        foreach (var item in fullBag)
        {
            item.GetComponent<ButtonAnimation>().enabled = enable;
            item.GetComponent<Button>().interactable = enable;
        }
    }

    private async UniTask TransitItemToInventory()
    {
        Color color = _image.color;
        var taskCompletionSource = new UniTaskCompletionSource();

        _image.transform.DOScale(5f, 0.3f);
        _image.DOFade(0, 0.3f).OnComplete(() =>
        {
            if (GiveSprite) _image.sprite = GiveSprite;
            GiveSprite = null;
            _image.transform.DOScale(Vector3.one, 0.3f);
            _image.color = color;
            _image.DOFade(0, 0.3f).From().OnComplete(() =>
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    inventory[i].sprite = fullBag[i].sprite;
                }
                taskCompletionSource.TrySetResult();
            });
        });
        await taskCompletionSource.Task;
    }
}
