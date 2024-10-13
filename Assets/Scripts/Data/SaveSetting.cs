using UnityEngine;

public class SaveSetting : MonoBehaviour
{
    [SerializeField] private SoundController soundController;

    private const string MusicMutedKey = "MusicMuted";
    private const string SoundMutedKey = "SoundMuted";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    private void Awake()
    {
        Load();
    }

    public void Save()
    {
        // Получаем текущие настройки звука из SoundController
        (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) = soundController.GetSoundSettings();

        // Сохраняем данные в PlayerPrefs
        PlayerPrefs.SetInt(MusicMutedKey, isMusicMuted ? 1 : 0);
        PlayerPrefs.SetInt(SoundMutedKey, isSoundMuted ? 1 : 0);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(SoundVolumeKey, soundVolume);

        // Сохраняем изменения
        PlayerPrefs.Save();
    }

    private void Load()
    {
        // Загружаем настройки звука
        bool isMusicMuted = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
        bool isSoundMuted = PlayerPrefs.GetInt(SoundMutedKey, 0) == 1;
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.3f);
        float soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 0.3f);

        // Устанавливаем настройки в SoundController
        soundController.LoadSoundSettings(musicVolume, soundVolume, isMusicMuted, isSoundMuted);
    }
}
