using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private HPAnimation hPAnimation;
    [SerializeField] private List<Image> hpImages;

    public void ChangeCountHP(int count)
    {
        int currentHp = 0;

        // ������������ ������� ���������� ���������� ��������
        foreach (var image in hpImages)
        {
            if (image.enabled)
            {
                currentHp++;
            }
        }

        int newHp = Mathf.Clamp(currentHp + count, 0, hpImages.Count); // ������������ ��������

        if (newHp < currentHp)
        {
            // ��������� �������� � ����� ������
            for (int i = currentHp - 1; i >= newHp; i--)
            {
                hPAnimation.RemoveLife(hpImages[i]);
            }
        }
        else if (newHp > currentHp)
        {
            // �������� �������� � ������ ������
            for (int i = currentHp; i < newHp; i++)
            {
                hPAnimation.AddLife(hpImages[i]);
            }
        }

        CheckCountHP();
    }

    private void CheckCountHP()
    {
        bool allDisabled = true;
        foreach (var image in hpImages)
        {
            if (image.enabled)
            {
                allDisabled = false;
                break;
            }
        }

        if (allDisabled)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game over");
    }
}
