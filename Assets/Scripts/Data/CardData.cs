using UnityEngine;
[System.Serializable]
public class CardData
{
    // �������� ���������� ��� �����
    [TextArea(3, 3)] public string messageText; // ����� ���������
    public Sprite illustrationSprite; // ����������� ��� �����

    public RuntimeAnimatorController animator; // �������� ��� ������� ��������
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
    public AudioClip reverseAudioClipItem; // ���� ���� �� ������ (���� �������)
    [TextArea(3, 3)] public string reverseTopTextIfItem; // ����� ������� �����
    public int changeLifePointsIfItem; // �������� ���� �����

    // ������ ����� (���� �������� ���)
    [Header("������ ����� (���� �������� ���)")]
    public Sprite reverseSpriteIfNoItem; // ������ ������� ����� (��� ��������)
    public AudioClip reverseAudioClipNoItem; // ���� ���� �� ������ (��� ��������)
    [TextArea(3, 3)] public string reverseTopTextIfNoItem; // ����� ������� �����
    public int changeLifePointsIfNoItem; // �������� ���� �����
}
