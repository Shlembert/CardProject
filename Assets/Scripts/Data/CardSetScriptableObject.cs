using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSet", menuName = "Card Game/Card Set")]
public class CardSetScriptableObject : ScriptableObject
{
    public Sprite location; // ������� ��� ����
    public Sprite portrait; // ������� ��� ����
    public RuntimeAnimatorController bannerAnimator; // �������� �������
    public AnimationClip animationClip; // �������� ��� �������
    public AudioClip bannerSound; // ���� �������
    [TextArea(3, 3)] public string bannerText; // ����� �� �������
    public bool checkPoint; // ������ ��������� ��������

    public CardData leftCard; // ����� �����
    public CardData rightCard; // ������ �����
}
