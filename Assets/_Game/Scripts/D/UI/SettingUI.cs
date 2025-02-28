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
            });

            sfxSlider.onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.SetSfxVolume(value);
            });

            musicBtn.onClick.AddListener(() =>
            {
                isMusicMuted = !isMusicMuted;
                SoundManager.Instance.ToggleMusic(isMusicMuted);
            });

            sfxBtn.onClick.AddListener(() =>
            {
                isSfxMuted = !isSfxMuted;
                SoundManager.Instance.ToggleSfx(isSfxMuted);
            });
        }
    }
}
