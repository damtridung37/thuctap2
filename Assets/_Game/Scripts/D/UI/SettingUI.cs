using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace D
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button menuBtn;
        [SerializeField] private Button continueBtn;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button musicBtn;
        [SerializeField] private Button sfxBtn;

        private bool isMusicMuted = false;
        private bool isSfxMuted = false;

        private void OnEnable()
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1);
            isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
            isSfxMuted = PlayerPrefs.GetInt("SfxMuted", 0) == 1;
            D.SoundManager.Instance.ToggleMusic(isMusicMuted);
            D.SoundManager.Instance.ToggleSfx(isSfxMuted);
            D.SoundManager.Instance.SetMusicVolume(musicSlider.value);
            D.SoundManager.Instance.SetSfxVolume(sfxSlider.value);
        }

        private void Start()
        {
            restartBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.GetMap(1, true);
                this.gameObject.SetActive(false);
            });
            menuBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Menu");
                this.gameObject.SetActive(false);
            });
            continueBtn.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

            musicSlider.onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.SetMusicVolume(value);
                PlayerPrefs.SetFloat("MusicVolume", value);
            });

            sfxSlider.onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.SetSfxVolume(value);
                PlayerPrefs.SetFloat("SfxVolume", value);
            });

            musicBtn.onClick.AddListener(() =>
            {
                isMusicMuted = !isMusicMuted;
                SoundManager.Instance.ToggleMusic(isMusicMuted);
                PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
            });

            sfxBtn.onClick.AddListener(() =>
            {
                isSfxMuted = !isSfxMuted;
                SoundManager.Instance.ToggleSfx(isSfxMuted);
                PlayerPrefs.SetInt("SfxMuted", isSfxMuted ? 1 : 0);
            });
        }
    }
}
