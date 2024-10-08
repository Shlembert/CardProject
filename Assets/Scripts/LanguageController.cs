using DG.Tweening;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private Transform languagePanel;
    [SerializeField] private float duration, delayInterval;
    [SerializeField] private List<Button> buttons;

    private void Start()
    {
        string currentLanguage = LocalizationManager.CurrentLanguageCode;
        Debug.Log(currentLanguage);
        foreach (var button in buttons)
        {
            if (button.name == currentLanguage) 
            {
                button.interactable = false;
            }

            button.GetComponent<ButtonAnimation>().enabled = false;
        }
    }

    public void ShowPanel()
    {
        foreach (var button in buttons) button.transform.localScale = Vector3.zero;
        languagePanel.position = Vector3.zero;
        languagePanel.localScale = Vector3.zero;
        languagePanel.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(languagePanel.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));
        sequence.OnComplete (() => ShowButtons());
    }

    public void HidePanel()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(languagePanel.DOScale(0, duration).SetEase(Ease.InBack));
        sequence.OnComplete(() => 
        languagePanel.gameObject.SetActive(false));
    }

    private void ShowButtons()
    {
        float currentDelay = 0f;

        foreach (Button button in buttons)
        {
            button.transform.DOScale(Vector3.one, duration * 0.2f)
                .SetEase(Ease.OutBack)
                .SetDelay(currentDelay);

            currentDelay += delayInterval; // Увеличиваем задержку для следующей кнопки
            if (button.enabled && button.name != LocalizationManager.CurrentLanguageCode)
            {
                button.GetComponent<ButtonAnimation>().enabled = true;
            }
        }
    }


    public void SetLanguage(Button button)
    {
        LocalizationManager.CurrentLanguageCode = button.name;
        foreach (Button button1 in buttons)
        {
            if (button1.enabled)
            {
                button1.interactable = true;
                button1.GetComponent<ButtonAnimation>().enabled = true;
            }
        }
        button.interactable = false;
        button.GetComponent<ButtonAnimation>().enabled = false;
    }
}
