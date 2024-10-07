using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource musAudioSource, sfxAudioSource;
    [SerializeField] private Slider musSlider, soundSlider;
    [SerializeField] private Image musImage, soundImage;
    [SerializeField] private Sprite onVolume, offVolume;
    [SerializeField] private AudioClip gameMus, mapMus, menuMus, flipClip, reverseClip, tapClip;
    [SerializeField] private float transitionDuration;
    [SerializeField] private float musicMinValue, musicMaxValue, soundMinValue, soundMaxValue;

    private bool isMusicMuted = false;
    private bool isSoundMuted = false;

    private void Start()
    {
        // Загружаем настройки звука при старте
        LoadSoundSettings();

        // Назначаем методы для управления громкостью на слайдеры
        musSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);

        // Устанавливаем диапазоны значений слайдеров
        musSlider.minValue = musicMinValue;  // Минимум 0
        musSlider.maxValue = musicMaxValue;  // Максимум 1
        soundSlider.minValue = soundMinValue; // Минимум 0
        soundSlider.maxValue = soundMaxValue; // Максимум 1
    }

    // Метод для загрузки настроек звука
    private void LoadSoundSettings()
    {
        // Проверяем, есть ли сохраненные настройки
        bool hasSavedSettings = CheckForSavedSettings(); // Замените на свою логику проверки

        if (hasSavedSettings)
        {
            // Загрузите сохраненные настройки здесь
            (float musicVolume, float soundVolume, bool musicMuted, bool soundMuted) = GetSavedSoundSettings(); // Замените на свою логику загрузки

            LoadSoundSettings(musicVolume, soundVolume, musicMuted, soundMuted);
        }
        else
        {
            // Если сохранений нет, устанавливаем громкость по умолчанию и включаем звук
            LoadSoundSettings(0.3f, 0.3f, false, false);
        }
    }

    // Метод для проверки наличия сохраненных настроек (примерная логика)
    private bool CheckForSavedSettings()
    {
        // Здесь должна быть ваша логика проверки наличия сохраненных настроек
        return false; // Вернуть true, если есть сохраненные настройки
    }

    // Метод для получения сохраненных настроек (примерная логика)
    private (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSavedSoundSettings()
    {
        // Здесь должна быть ваша логика загрузки сохраненных настроек
        return (0.5f, 0.5f, false, false); // Примерные значения
    }

    // Метод изменения громкости музыки по слайдеру
    private void ChangeMusicVolume(float value)
    {
        musAudioSource.volume = value;
    }

    // Метод изменения громкости звука по слайдеру с воспроизведением клика
    private void ChangeSoundVolume(float value)
    {
        sfxAudioSource.volume = value;
        PlayClick(); // Воспроизведение звука при изменении громкости
    }

    // Метод для включения и отключения музыки
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted; // Переключаем состояние звука
        musAudioSource.mute = isMusicMuted;
        musImage.sprite = isMusicMuted ? offVolume : onVolume; // Меняем спрайт
    }

    // Метод для включения и отключения звуковых эффектов
    public void ToggleSound()
    {
        isSoundMuted = !isSoundMuted; // Переключаем состояние звука
        sfxAudioSource.mute = isSoundMuted;
        soundImage.sprite = isSoundMuted ? offVolume : onVolume; // Меняем спрайт
    }

    // Метод для получения текущих настроек звука
    public (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSoundSettings()
    {
        return (musAudioSource.volume, sfxAudioSource.volume, isMusicMuted, isSoundMuted);
    }

    // Метод для загрузки настроек звука
    public void LoadSoundSettings(float musicVolume, float soundVolume, bool musicMuted, bool soundMuted)
    {
        musAudioSource.volume = musicVolume;
        sfxAudioSource.volume = soundVolume;
        musAudioSource.mute = musicMuted;
        sfxAudioSource.mute = soundMuted;

        musSlider.value = musicVolume;
        soundSlider.value = soundVolume;

        musImage.sprite = musicMuted ? offVolume : onVolume;
        soundImage.sprite = soundMuted ? offVolume : onVolume;
    }

    // Воспроизведение звука Flip
    public void PlayFlip()
    {
        sfxAudioSource.PlayOneShot(flipClip);
    }

    // Воспроизведение звука при клике
    public void PlayClick()
    {
        sfxAudioSource.PlayOneShot(tapClip);
    }

    // Воспроизведение звука по переданному клипу
    public void PlaySoundClip(AudioClip clip)
    {
        if (clip)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    // Воспроизведение звука Reverse Flip
    public void PlayReverseFlip()
    {
        sfxAudioSource.PlayOneShot(reverseClip);
    }

    // Воспроизведение фоновой музыки для меню
    public void PlayMenuMusic()
    {
        StartCoroutine(FadeToNewMusic(menuMus));
    }

    // Воспроизведение фоновой музыки для игры
    public void PlayGameMusic()
    {
        StartCoroutine(FadeToNewMusic(gameMus));
    }

    // Воспроизведение фоновой музыки для карты
    public void PlayMapMusic()
    {
        StartCoroutine(FadeToNewMusic(mapMus));
    }

    // Корутин для плавного перехода на новый музыкальный трек
    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        if (musAudioSource.isPlaying)
        {
            // Плавное уменьшение громкости текущей музыки
            yield return StartCoroutine(FadeOut());
        }

        musAudioSource.clip = newClip;
        musAudioSource.Play();

        // Плавное увеличение громкости новой музыки
        yield return StartCoroutine(FadeIn());
    }

    // Плавное уменьшение громкости
    private IEnumerator FadeOut()
    {
        float startVolume = musAudioSource.volume;

        while (musAudioSource.volume > 0)
        {
            musAudioSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musAudioSource.Stop();
        musAudioSource.volume = startVolume; // Возвращаем громкость
    }

    // Плавное увеличение громкости
    private IEnumerator FadeIn()
    {
        musAudioSource.volume = 0f;
        float targetVolume = musSlider.value; // Используем значение из слайдера

        while (musAudioSource.volume < targetVolume)
        {
            musAudioSource.volume += Time.deltaTime / transitionDuration;
            yield return null;
        }
    }
}
