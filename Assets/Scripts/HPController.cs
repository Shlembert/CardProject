using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private HPAnimation hPAnimation;
    [SerializeField] private List<Image> hpImages;
    [SerializeField] private GameController gameController;

    private int _currentHP = 3;

    public async void ChangeCountHP(int count)
    {
        int currentHp = 0;

        // Подсчитываем текущее количество включенных сердечек
        foreach (var image in hpImages)
        {
            if (image.enabled)
            {
                currentHp++;
            }
        }

        int newHp = Mathf.Clamp(currentHp + count, 0, hpImages.Count); // ограничиваем диапазон

        if (newHp < currentHp)
        {
            // Выключаем сердечки с конца списка
            for (int i = currentHp - 1; i >= newHp; i--)
            {
                hPAnimation.RemoveLife(hpImages[i]);
                if (_currentHP >= 2) _currentHP--;
                else 
                {
                    Debug.Log("Game Over");
                    gameController.IsGameOver = true;

                  await  gameController.ShowDead();
                    return;
                }
                
            }
        }
        else if (newHp > currentHp)
        {
            // Включаем сердечки с начала списка
            for (int i = currentHp; i < newHp; i++)
            {
                hPAnimation.AddLife(hpImages[i]);
            }
        }
    }

    // Метод для установки точного количества жизней
    public void SetHP(int targetHp)
    {
        targetHp = Mathf.Clamp(targetHp, 0, hpImages.Count); // Ограничиваем диапазон

        int currentHp = GetCurrentHP(); // Получаем текущее количество жизней

        if (targetHp < currentHp)
        {
            // Выключаем сердечки с конца списка
            for (int i = currentHp - 1; i >= targetHp; i--)
            {
                hPAnimation.RemoveLife(hpImages[i]);
                _currentHP--;
            }
            
        }
        else if (targetHp > currentHp)
        {
            // Включаем сердечки с начала списка
            for (int i = currentHp; i < targetHp; i++)
            {
                hPAnimation.AddLife(hpImages[i]);
                _currentHP++;
            }
        }
    }

    public int GetCurrentHP()
    {
        int currentHp = 0;

        // Подсчитываем количество включенных сердечек
        foreach (var image in hpImages)
        {
            if (image.enabled)
            {
                currentHp++;
            }
        }

        return currentHp;
    }
}
