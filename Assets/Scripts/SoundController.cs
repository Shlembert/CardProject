using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource musAudioSource, sfxAudioSource;
    [SerializeField] private AudioClip gameMus, mapMus, menuMus, flipClip, reverseClip, tapClip;
    [SerializeField] private float transitionDuration; // Время перехода

    public void PlayFlip()
    {
        sfxAudioSource.PlayOneShot(flipClip);
    }

    public void PlayClick()
    {
        sfxAudioSource.PlayOneShot(tapClip);
    }

    public void PlaySoundClip(AudioClip clip)
    {
        if (clip) 
        {
            sfxAudioSource.PlayOneShot(clip);
        } 
    }
    public void PlayReverseFlip()
    {
        sfxAudioSource.PlayOneShot(reverseClip);
    }
    public void PlayMenuMusic()
    {
        StartCoroutine(FadeToNewMusic(menuMus));
    }

    public void PlayGameMusic()
    {
        StartCoroutine(FadeToNewMusic(gameMus));
    }

    public void PlayMapMusic()
    {
        StartCoroutine(FadeToNewMusic(mapMus));
    }

    // Корутин для плавного переключения музыки
    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        if (musAudioSource.isPlaying)
        {
            // Плавно уменьшаем громкость текущего трека
            yield return StartCoroutine(FadeOut());
        }

        // Меняем трек
        musAudioSource.clip = newClip;
        musAudioSource.Play();

        // Плавно увеличиваем громкость нового трека
        // yield return StartCoroutine(FadeIn());
        yield return null;
    }

    // Метод для плавного уменьшения громкости
    private IEnumerator FadeOut()
    {
        float startVolume = musAudioSource.volume;

        while (musAudioSource.volume > 0)
        {
            musAudioSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musAudioSource.Stop();
        musAudioSource.volume = startVolume; // Возвращаем громкость к исходному значению
    }

    // Метод для плавного увеличения громкости
    private IEnumerator FadeIn()
    {
        musAudioSource.volume = 0f;
        float targetVolume = 0.5f; // Или другое значение громкости по умолчанию

        while (musAudioSource.volume < targetVolume)
        {
            musAudioSource.volume += Time.deltaTime / transitionDuration;
            yield return null;
        }
    }
}
