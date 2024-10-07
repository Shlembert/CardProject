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
        // ��������� ��������� ����� ��� ������
        LoadSoundSettings();

        // ��������� ������ ��� ���������� ���������� �� ��������
        musSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);

        // ������������� ��������� �������� ���������
        musSlider.minValue = musicMinValue;  // ������� 0
        musSlider.maxValue = musicMaxValue;  // �������� 1
        soundSlider.minValue = soundMinValue; // ������� 0
        soundSlider.maxValue = soundMaxValue; // �������� 1
    }

    // ����� ��� �������� �������� �����
    private void LoadSoundSettings()
    {
        // ���������, ���� �� ����������� ���������
        bool hasSavedSettings = CheckForSavedSettings(); // �������� �� ���� ������ ��������

        if (hasSavedSettings)
        {
            // ��������� ����������� ��������� �����
            (float musicVolume, float soundVolume, bool musicMuted, bool soundMuted) = GetSavedSoundSettings(); // �������� �� ���� ������ ��������

            LoadSoundSettings(musicVolume, soundVolume, musicMuted, soundMuted);
        }
        else
        {
            // ���� ���������� ���, ������������� ��������� �� ��������� � �������� ����
            LoadSoundSettings(0.3f, 0.3f, false, false);
        }
    }

    // ����� ��� �������� ������� ����������� �������� (��������� ������)
    private bool CheckForSavedSettings()
    {
        // ����� ������ ���� ���� ������ �������� ������� ����������� ��������
        return false; // ������� true, ���� ���� ����������� ���������
    }

    // ����� ��� ��������� ����������� �������� (��������� ������)
    private (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSavedSoundSettings()
    {
        // ����� ������ ���� ���� ������ �������� ����������� ��������
        return (0.5f, 0.5f, false, false); // ��������� ��������
    }

    // ����� ��������� ��������� ������ �� ��������
    private void ChangeMusicVolume(float value)
    {
        musAudioSource.volume = value;
    }

    // ����� ��������� ��������� ����� �� �������� � ���������������� �����
    private void ChangeSoundVolume(float value)
    {
        sfxAudioSource.volume = value;
        PlayClick(); // ��������������� ����� ��� ��������� ���������
    }

    // ����� ��� ��������� � ���������� ������
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted; // ����������� ��������� �����
        musAudioSource.mute = isMusicMuted;
        musImage.sprite = isMusicMuted ? offVolume : onVolume; // ������ ������
    }

    // ����� ��� ��������� � ���������� �������� ��������
    public void ToggleSound()
    {
        isSoundMuted = !isSoundMuted; // ����������� ��������� �����
        sfxAudioSource.mute = isSoundMuted;
        soundImage.sprite = isSoundMuted ? offVolume : onVolume; // ������ ������
    }

    // ����� ��� ��������� ������� �������� �����
    public (float musicVolume, float soundVolume, bool isMusicMuted, bool isSoundMuted) GetSoundSettings()
    {
        return (musAudioSource.volume, sfxAudioSource.volume, isMusicMuted, isSoundMuted);
    }

    // ����� ��� �������� �������� �����
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

    // ��������������� ����� Flip
    public void PlayFlip()
    {
        sfxAudioSource.PlayOneShot(flipClip);
    }

    // ��������������� ����� ��� �����
    public void PlayClick()
    {
        sfxAudioSource.PlayOneShot(tapClip);
    }

    // ��������������� ����� �� ����������� �����
    public void PlaySoundClip(AudioClip clip)
    {
        if (clip)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    // ��������������� ����� Reverse Flip
    public void PlayReverseFlip()
    {
        sfxAudioSource.PlayOneShot(reverseClip);
    }

    // ��������������� ������� ������ ��� ����
    public void PlayMenuMusic()
    {
        StartCoroutine(FadeToNewMusic(menuMus));
    }

    // ��������������� ������� ������ ��� ����
    public void PlayGameMusic()
    {
        StartCoroutine(FadeToNewMusic(gameMus));
    }

    // ��������������� ������� ������ ��� �����
    public void PlayMapMusic()
    {
        StartCoroutine(FadeToNewMusic(mapMus));
    }

    // ������� ��� �������� �������� �� ����� ����������� ����
    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        if (musAudioSource.isPlaying)
        {
            // ������� ���������� ��������� ������� ������
            yield return StartCoroutine(FadeOut());
        }

        musAudioSource.clip = newClip;
        musAudioSource.Play();

        // ������� ���������� ��������� ����� ������
        yield return StartCoroutine(FadeIn());
    }

    // ������� ���������� ���������
    private IEnumerator FadeOut()
    {
        float startVolume = musAudioSource.volume;

        while (musAudioSource.volume > 0)
        {
            musAudioSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musAudioSource.Stop();
        musAudioSource.volume = startVolume; // ���������� ���������
    }

    // ������� ���������� ���������
    private IEnumerator FadeIn()
    {
        musAudioSource.volume = 0f;
        float targetVolume = musSlider.value; // ���������� �������� �� ��������

        while (musAudioSource.volume < targetVolume)
        {
            musAudioSource.volume += Time.deltaTime / transitionDuration;
            yield return null;
        }
    }
}
