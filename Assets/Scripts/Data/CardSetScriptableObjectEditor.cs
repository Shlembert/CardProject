#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardSetScriptableObject))]
public class CardSetScriptableObjectEditor : Editor
{
    private GUIStyle _boldStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ��������� ������� �����
        if (_boldStyle == null)
        {
            _boldStyle = new GUIStyle(EditorStyles.boldLabel);
            _boldStyle.fontSize += 1;
        }

        // ���� ��� �����
        EditorGUILayout.LabelField("���������� � ����", _boldStyle);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("portrait"), new GUIContent("�������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerAnimator"), new GUIContent("�������� ��������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationClip"), new GUIContent("�������� ��������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerSound"), new GUIContent("���� ��������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bannerText"), new GUIContent("����� �������"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("checkPoint"), new GUIContent("��������"));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // �������������� �����
        EditorGUILayout.Space();

        // ������ ��� ����� �����
        EditorGUILayout.LabelField("����� �����", _boldStyle);
        DrawCardFields("leftCard");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // �������������� �����
        EditorGUILayout.Space();

        // ������ ��� ������ �����
        EditorGUILayout.LabelField("������ �����", _boldStyle);
        DrawCardFields("rightCard");

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCardFields(string cardPropertyPath)
    {
        // �������� ����
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.messageText"), new GUIContent("����� ���������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.illustrationSprite"), new GUIContent("�����������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.animationClip"), new GUIContent("��������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.animator"), new GUIContent("����������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.itemSprite"), new GUIContent("������ �������� �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.requiredItemSprite"), new GUIContent("������ ���������� ��������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.removeItemSprite"), new GUIContent("������� ������ ��� �������������"));

        // ���� ��� ��������� ������� � ������� locationSpriteNoItem
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeBanner"), new GUIContent("������� ������"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.nextSetIfItem"), new GUIContent("��������� ����� (���� �������)"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.nextSetIfNoItem"), new GUIContent("��������� ����� (��� ��������)"));

        EditorGUILayout.Space();

        // ������ ����� (���� ������� ����)
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.isWinIfItem"), new GUIContent("������?"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.winContentIfItem"), new GUIContent("����� ������� ������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseSpriteIfItem"), new GUIContent("������ �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseAudioClipItem"), new GUIContent("���� �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseTopTextIfItem"), new GUIContent("����� �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeLifePointsIfItem"), new GUIContent("�������� ���� �����"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.locationSpriteIfItem"), new GUIContent("������� ������� (������) ���� �������"));


        EditorGUILayout.Space();

        // ������ ����� (���� �������� ���)
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.isWinIfNoItem"), new GUIContent("������?"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.winContentIfNoItem"), new GUIContent("����� ������� ������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseSpriteIfNoItem"), new GUIContent("������ �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseAudioClipNoItem"), new GUIContent("���� �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.reverseTopTextIfNoItem"), new GUIContent("����� �������"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.changeLifePointsIfNoItem"), new GUIContent("�������� ���� �����"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty($"{cardPropertyPath}.locationSpriteNoItem"), new GUIContent("������� ������� (������) ��� ��������"));
    }
}

#endif