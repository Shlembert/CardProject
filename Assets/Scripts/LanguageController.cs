using I2.Loc;
using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown; // Ссылка на Dropdown

    private void Start()
    {
        // Назначаем метод для обработки изменения значения в Dropdown
        languageDropdown.onValueChanged.AddListener(OnLanguageChange);

        // Устанавливаем начальный язык (опционально)
        SetLanguage(languageDropdown.value);
    }

    private void OnLanguageChange(int index)
    {
        // Устанавливаем язык по индексу
        SetLanguage(index);
    }

    public void SetLanguage(int index)
    {
        string languageCode;

        // Определяем код языка по индексу
        switch (index)
        {
            case 0:
                languageCode = "ru";
                break;
            case 1:
                languageCode = "en";
                break;
           
            default: // Если ничего не выбрано, то русский по умолчанию
                languageCode = "ru";
                break;
        }

        // Устанавливаем язык в LocalizationManager
        LocalizationManager.CurrentLanguageCode = languageCode;
    }
}
