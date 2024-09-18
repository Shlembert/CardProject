using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class CardData
{
    // �������� ���������� ��� �����
    [TextArea(3, 3)] public string messageText; // ����� ���������
    public Sprite illustrationSprite; // ����������� ��� �����
    public AnimatorController animator; // �������� ��� �����
    public AnimationClip animationClip; // �������� ��� �����

    // ���������� � ���������
    public Sprite itemSprite; // ������ ��������, ���� ����� ���� �������
    public Sprite requiredItemSprite; // ������ ���������� ��������
    public bool removeItemSprite; // �������� ���������� �������� 

    // ��������� � ������� � �������
    public bool changeBanner; // ���� ��� ��������� �������
    public Sprite locationSprite; // ������ ��� ��������� ���� (�������)

    // ��������� ����� ����
    public CardSetScriptableObject nextSetIfItem; // ��������� �����, ���� ������� ����
    public CardSetScriptableObject nextSetIfNoItem; // ��������� �����, ���� �������� ���

    // ������ ����� (���� ������� ����)
    [Header("������ ����� (���� ������� ����)")]
    public Sprite reverseSpriteIfItem; // ������ ������� ����� (���� �������)
    [TextArea(3, 3)] public string reverseTopTextIfItem; // ����� ������� �����
    public int changeLifePointsIfItem; // �������� ���� �����

    // ������ ����� (���� �������� ���)
    [Header("������ ����� (���� �������� ���)")]
    public Sprite reverseSpriteIfNoItem; // ������ ������� ����� (��� ��������)
    [TextArea(3, 3)] public string reverseTopTextIfNoItem; // ����� ������� �����
    public int changeLifePointsIfNoItem; // �������� ���� �����
}
