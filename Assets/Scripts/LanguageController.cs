using I2.Loc;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public void SetLanguage(string language)
    {
        LocalizationManager.CurrentLanguageCode = language;
    }
}
