using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSet", menuName = "Card Game/Card Set")]
public class CardSetScriptableObject : ScriptableObject
{
    public Sprite location; // Локация для сета
    public Sprite portrait; // Портрет для сета
    public RuntimeAnimatorController bannerAnimator; // Аниматор баннера
    public AnimationClip animationClip; // Анимация для баннера
    public AudioClip bannerSound; // Звук баннера
    [TextArea(3, 3)] public string bannerText; // Текст на баннере
    public bool checkPoint; // Индекс активации чепоинта

    public CardData leftCard; // Левая карта
    public CardData rightCard; // Правая карта
}
