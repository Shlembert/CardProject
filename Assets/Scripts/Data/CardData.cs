using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardData
{
    // �������� ���������� ��� �����
    public string messageText; // ����� ���������
    public Sprite illustrationSprite; // ����������� ��� �����

    public RuntimeAnimatorController animator; // �������� ��� ������� ��������
    public AnimationClip animationClip; // �������� ��� �����

    // ���������� � ���������
    public Sprite itemSprite; // ������ ��������, ���� ����� ���� �������
    public Sprite requiredItemSprite; // ������ ���������� ��������
    public bool removeItemSprite; // �������� ���������� �������� 

    // ��������� � ������� � �������
    public bool changeBanner; // ���� ��� ��������� �������

    // ��������� ����� ����
    public CardSetScriptableObject nextSetIfItem; // ��������� �����, ���� ������� ����
    public CardSetScriptableObject nextSetIfNoItem; // ��������� �����, ���� �������� ���

    // ������ ����� (���� ������� ����)
    [Header("������ ����� (���� ������� ����)")]
    public bool isWinIfItem; // �������� ����������
    public List<WinContent> winContentIfItem; // ����� �������� �������
    public Sprite locationSpriteIfItem; // ������ ��� ��������� ���� (�������) ���� �������
    public Sprite reverseSpriteIfItem; // ������ ������� ����� (���� �������)
    public AudioClip reverseAudioClipItem; // ���� ���� �� ������ (���� �������)
    public string reverseTopTextIfItem; // ����� ������� �����
    public int changeLifePointsIfItem; // �������� ���� �����

    // ������ ����� (���� �������� ���)
    [Header("������ ����� (���� �������� ���)")]
    public bool isWinIfNoItem; // �������� ����������
    public List<WinContent> winContentIfNoItem; // ����� �������� �������
    public Sprite locationSpriteNoItem; // ������ ��� ��������� ���� (�������) ��� ��������
    public Sprite reverseSpriteIfNoItem; // ������ ������� ����� (��� ��������)
    public AudioClip reverseAudioClipNoItem; // ���� ���� �� ������ (��� ��������)
    public string reverseTopTextIfNoItem; // ����� ������� �����
    public int changeLifePointsIfNoItem; // �������� ���� �����
}
