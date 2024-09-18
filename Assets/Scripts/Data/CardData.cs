using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class CardData
{
    // Основная информация для карты
    [TextArea(3, 3)] public string messageText; // Текст сообщения
    public Sprite illustrationSprite; // Иллюстрация для карты
    public AnimatorController animator; // Аниматор для бухих
    public AnimationClip animationClip; // Анимация для карты

    // Информация о предметах
    public Sprite itemSprite; // Спрайт предмета, если карта дает предмет
    public Sprite requiredItemSprite; // Спрайт требуемого предмета
    public bool removeItemSprite; // Удаление требуемого предмета 

    // Изменения в баннере и локации
    public bool changeBanner; // Флаг для изменения баннера
    public Sprite locationSprite; // Спрайт для изменения фона (локации)

    // Следующий набор карт
    public CardSetScriptableObject nextSetIfItem; // Следующий набор, если предмет есть
    public CardSetScriptableObject nextSetIfNoItem; // Следующий набор, если предмета нет

    // Реверс карты (если предмет есть)
    [Header("Реверс карты (если предмет есть)")]
    public Sprite reverseSpriteIfItem; // Спрайт реверса карты (есть предмет)
    [TextArea(3, 3)] public string reverseTopTextIfItem; // Текст реверса карты
    public int changeLifePointsIfItem; // Изменить очки жизни

    // Реверс карты (если предмета нет)
    [Header("Реверс карты (если предмета нет)")]
    public Sprite reverseSpriteIfNoItem; // Спрайт реверса карты (нет предмета)
    [TextArea(3, 3)] public string reverseTopTextIfNoItem; // Текст реверса карты
    public int changeLifePointsIfNoItem; // Изменить очки жизни
}
