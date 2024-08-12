using UnityEngine;

[System.Serializable]
public class CardData
{
    // �������� ���������� ��� �����
    [TextArea(3, 3)] public string messageText; // ����� ���������
    public Sprite illustrationSprite; // ����������� ��� �����

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
    [TextArea(3, 3)] public string reverseTopTextIfItem; // ������� ����� ������� �����
    [TextArea(3, 3)] public string reverseBottomTextIfItem; // ������ ����� ������� �����
    public int changeLifePointsIfItem; // �������� ���� �����

    // ������ ����� (���� �������� ���)
    [Header("������ ����� (���� �������� ���)")]
    public Sprite reverseSpriteIfNoItem; // ������ ������� ����� (��� ��������)
    [TextArea(3, 3)] public string reverseTopTextIfNoItem; // ������� ����� ������� �����
    [TextArea(3, 3)] public string reverseBottomTextIfNoItem; // ������ ����� ������� �����
    public int changeLifePointsIfNoItem; // �������� ���� �����
}
