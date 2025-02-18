using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardData
{
    // Основная информация для карты
    public string messageText; // Текст сообщения
    public Sprite illustrationSprite; // Иллюстрация для карты

    public RuntimeAnimatorController animator; // Аниматор для подмены анимации
    public AnimationClip animationClip; // Анимация для карты

    // Информация о предметах
    public Sprite itemSprite; // Спрайт предмета, если карта дает предмет
    public Sprite requiredItemSprite; // Спрайт требуемого предмета
    public bool removeItemSprite; // Удаление требуемого предмета 

    // Изменения в баннере и локации
    public bool changeBanner; // Флаг для изменения баннера

    // Следующий набор карт
    public CardSetScriptableObject nextSetIfItem; // Следующий набор, если предмет есть
    public CardSetScriptableObject nextSetIfNoItem; // Следующий набор, если предмета нет

    // Реверс карты (если предмет есть)
    [Header("Реверс карты (если предмет есть)")]
    public bool isWinIfItem; // Победная клмбинация
    public List<WinContent> winContentIfItem; // набор победных слайдов
    public Sprite locationSpriteIfItem; // Спрайт для изменения фона (локации) есть предмет
    public Sprite reverseSpriteIfItem; // Спрайт реверса карты (есть предмет)
    public AudioClip reverseAudioClipItem; // Звук клик на реверс (есть предмет)
    public string reverseTopTextIfItem; // Текст реверса карты
    public int changeLifePointsIfItem; // Изменить очки жизни

    // Реверс карты (если предмета нет)
    [Header("Реверс карты (если предмета нет)")]
    public bool isWinIfNoItem; // Победная клмбинация
    public List<WinContent> winContentIfNoItem; // набор победных слайдов
    public Sprite locationSpriteNoItem; // Спрайт для изменения фона (локации) нет предмета
    public Sprite reverseSpriteIfNoItem; // Спрайт реверса карты (нет предмета)
    public AudioClip reverseAudioClipNoItem; // Звук клик на реверс (нет предмета)
    public string reverseTopTextIfNoItem; // Текст реверса карты
    public int changeLifePointsIfNoItem; // Изменить очки жизни
}
