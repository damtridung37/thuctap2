using System;
using UnityEngine;
namespace D
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        public AudioClip[] musicDict;

        [Header("SFX")]
        [SerializeField] private AudioSource sfxSource;
        public AudioClip[] sfxDict;

        bool isMusicMuted = false;
        bool isSfxMuted = false;
        bool isMuted = false;

        public void PlayMusic(MusicType type)
        {
            musicSource.clip = musicDict[(int)type];
            musicSource.Play();
        }

        public void PlayCustomMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(SfxType type)
        {
            sfxSource.PlayOneShot(sfxDict[(int)type]);
        }

        public void PlayCustomSfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void ToggleMusic()
        {
            isMusicMuted = !isMusicMuted;
            musicSource.mute = isMusicMuted;
        }

        public void ToggleSfx()
        {
            isSfxMuted = !isSfxMuted;
            sfxSource.mute = isSfxMuted;
        }

        public void ToggleMute()
        {
            isMuted = !isMuted;
            musicSource.mute = isMuted;
            sfxSource.mute = isMuted;
        }
    }

    public enum MusicType
    {
        Main,
        Battle,
        Shop
    }

    public enum SfxType
    {
        Agis_Shot
    }

    [Serializable]
    public class SfxDict : SerializableDictionary<SfxType, AudioClip>
    {

    }

    [Serializable]
    public class MusicDict : SerializableDictionary<MusicType, AudioClip>
    {
    }
}
