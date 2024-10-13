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
    private float musicVolume;
    private float soundVolume;

    private void Start()
    {
        musSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);

        musSlider.minValue = musicMinValue;
        musSlider.maxValue = musicMaxValue;
        soundSlider.minValue = soundMinValue;
        soundSlider.maxValue = soundMaxValue;
    }

    private (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSavedSoundSettings()
    {
        return (0.5f, 0.5f, false, false); // Примерные значения
    }

    private void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        musAudioSource.volume = isMusicMuted ? 0 : musicVolume;
    }

    private void ChangeSoundVolume(float value)
    {
        soundVolume = value;
        sfxAudioSource.volume = isSoundMuted ? 0 : soundVolume;
        PlayClick();
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musAudioSource.mute = isMusicMuted;
        musAudioSource.volume = isMusicMuted ? 0 : musicVolume;
        musImage.sprite = isMusicMuted ? offVolume : onVolume;
    }

    public void ToggleSound()
    {
        isSoundMuted = !isSoundMuted;
        sfxAudioSource.mute = isSoundMuted;
        sfxAudioSource.volume = isSoundMuted ? 0 : soundVolume;
        soundImage.sprite = isSoundMuted ? offVolume : onVolume;
    }

    public (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSoundSettings()
    {
        return (musicVolume, soundVolume, isMusicMuted, isSoundMuted);
    }

    public void LoadSoundSettings(float musicVolume, float soundVolume, bool musicMuted, bool soundMuted)
    {
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
        isMusicMuted = musicMuted;
        isSoundMuted = soundMuted;

        musAudioSource.volume = musicMuted ? 0 : musicVolume;
        sfxAudioSource.volume = soundMuted ? 0 : soundVolume;
        musAudioSource.mute = musicMuted;
        sfxAudioSource.mute = soundMuted;

        musSlider.value = musicVolume;
        soundSlider.value = soundVolume;

        musImage.sprite = musicMuted ? offVolume : onVolume;
        soundImage.sprite = soundMuted ? offVolume : onVolume;
    }

    public void PlayFlip() => sfxAudioSource.PlayOneShot(flipClip);

    public void PlayClick() => sfxAudioSource.PlayOneShot(tapClip);

    public void PlaySoundClip(AudioClip clip)
    {
        if (clip)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayReverseFlip() => sfxAudioSource.PlayOneShot(reverseClip);

    public void PlayMenuMusic() => StartCoroutine(FadeToNewMusic(menuMus));

    public void PlayGameMusic() => StartCoroutine(FadeToNewMusic(gameMus));

    public void PlayMapMusic() => StartCoroutine(FadeToNewMusic(mapMus));

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        if (musAudioSource.isPlaying)
        {
            // Плавное уменьшение громкости текущей музыки
            yield return StartCoroutine(FadeOut());
        }

        musAudioSource.clip = newClip;
        musAudioSource.volume = 0; // Начинаем с нулевой громкости
        musAudioSource.Play();

        // Плавное увеличение громкости новой музыки до значения `musicVolume`
        yield return StartCoroutine(FadeIn());
    }


    private IEnumerator FadeOut()
    {
        float startVolume = musAudioSource.volume;

        while (musAudioSource.volume > 0)
        {
            musAudioSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musAudioSource.Stop();
        musAudioSource.volume = musicVolume; // Возвращаем громкость
    }

    private IEnumerator FadeIn()
    {
        musAudioSource.volume = 0f;
        float targetVolume = musicVolume;

        while (musAudioSource.volume < targetVolume)
        {
            musAudioSource.volume += Time.deltaTime / transitionDuration;
            yield return null;
        }

        // Убедимся, что громкость точно равна `musicVolume`
        musAudioSource.volume = musicVolume;
    }
}
