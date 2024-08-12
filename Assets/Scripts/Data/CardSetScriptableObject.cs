using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSet", menuName = "Card Game/Card Set")]
public class CardSetScriptableObject : ScriptableObject
{
    public Sprite portrait; // ������� ��� ����
    [TextArea(3, 3)] public string bannerText; // ����� �� �������
    public int indexCheckPoint; // ������ ��������� ��������

    public CardData leftCard; // ����� �����
    public CardData rightCard; // ������ �����
}
