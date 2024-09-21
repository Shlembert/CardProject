using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardSetScriptableObject))]
public class CardSetScriptableObjectEditor : Editor
{
    private GUIStyle _boldStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Настройка жирного стиля
        if (_boldStyle == null)
        {
            _boldStyle = new GUIStyle(EditorStyles.boldLabel);
            _boldStyle.fontSize += 1;
        }

        // Поля для сетов
        EditorGUILayout.LabelField("Информация о сете", _boldStyle);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("portrait"), new GUIContent("Портрет"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerSound"), new GUIContent("Звук портрета"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerText"), new GUIContent("Текст баннера"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("checkPoint"), new GUIContent("Чекпоинт"));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // Разделительная черта
        EditorGUILayout.Space();

        // Секция для левой карты
        EditorGUILayout.LabelField("Левая карта", _boldStyle);
        DrawCardFields("leftCard");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // Разделительная черта
        EditorGUILayout.Space();

        // Секция для правой карты
        EditorGUILayout.LabelField("Правая карта", _boldStyle);
        DrawCardFields("rightCard");

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCardFields(string cardPropertyPath)
    {
        // Основные поля
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.messageText"), new GUIContent("Текст сообщения"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.illustrationSprite"), new GUIContent("Иллюстрация"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.animationClip"), new GUIContent("Анимация"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.animator"), new GUIContent("Контроллер"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.itemSprite"), new GUIContent("Спрайт предмета награды"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.requiredItemSprite"), new GUIContent("Спрайт требуемого предмета"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.removeItemSprite"), new GUIContent("Удалить спрайт при использовании"));

        // Поля для изменения баннера и локации
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeBanner"), new GUIContent("Сменить баннер"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.locationSprite"), new GUIContent("Сменить локацию (спрайт)"));
       

        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.nextSetIfItem"), new GUIContent("Следующий набор (есть предмет)"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.nextSetIfNoItem"), new GUIContent("Следующий набор (нет предмета)"));

        EditorGUILayout.Space();

        // Реверс карты (если предмет есть)
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseSpriteIfItem"), new GUIContent("Спрайт реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseAudioClipItem"), new GUIContent("Звук реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseTopTextIfItem"), new GUIContent("Текст реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeLifePointsIfItem"), new GUIContent("Изменить очки жизни"));

        EditorGUILayout.Space();

        // Реверс карты (если предмета нет)
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseSpriteIfNoItem"), new GUIContent("Спрайт реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseAudioClipNoItem"), new GUIContent("Звук реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseTopTextIfNoItem"), new GUIContent("Текст реверса"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeLifePointsIfNoItem"), new GUIContent("Изменить очки жизни"));
    }
}
