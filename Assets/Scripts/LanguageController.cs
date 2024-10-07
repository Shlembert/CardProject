using I2.Loc;
using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown; // ������ �� Dropdown

    private void Start()
    {
        // ��������� ����� ��� ��������� ��������� �������� � Dropdown
        languageDropdown.onValueChanged.AddListener(OnLanguageChange);

        // ������������� ��������� ���� (�����������)
        SetLanguage(languageDropdown.value);
    }

    private void OnLanguageChange(int index)
    {
        // ������������� ���� �� �������
        SetLanguage(index);
    }

    public void SetLanguage(int index)
    {
        string languageCode;

        // ���������� ��� ����� �� �������
        switch (index)
        {
            case 0:
                languageCode = "ru";
                break;
            case 1:
                languageCode = "en";
                break;
           
            default: // ���� ������ �� �������, �� ������� �� ���������
                languageCode = "ru";
                break;
        }

        // ������������� ���� � LocalizationManager
        LocalizationManager.CurrentLanguageCode = languageCode;
    }
}
