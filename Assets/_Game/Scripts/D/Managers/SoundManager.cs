using UnityEngine;
namespace D
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        public AudioClip[] musicDict;
        public int defaultMusic = -1;

        [Header("SFX")]
        [SerializeField] private AudioSource sfxSource;
        public AudioClip[] sfxDict;

        private void Start()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
            float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1);
            bool isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
            bool isSfxMuted = PlayerPrefs.GetInt("SfxMuted", 0) == 1;
            D.SoundManager.Instance.ToggleMusic(isMusicMuted);
            D.SoundManager.Instance.ToggleSfx(isSfxMuted);
            D.SoundManager.Instance.SetMusicVolume(musicVolume);
            D.SoundManager.Instance.SetSfxVolume(sfxVolume);
            if (defaultMusic != -1)
            {
                PlayMusic((MusicType)defaultMusic);
            }
        }

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
        Menu,
        Story
    }

    public enum SfxType
    {
        Agis_Shot,
        Death1,
        Death2,
        Death3,
    }
}
