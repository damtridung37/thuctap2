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

        public void ToggleMusic(bool isMute)
        {
            musicSource.mute = isMute;
        }

        public void ToggleSfx(bool isMute)
        {
            sfxSource.mute = isMute;
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            sfxSource.volume = volume;
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
        Agis_Shot,
        Death1,
        Death2,
        Death3,
    }
}
