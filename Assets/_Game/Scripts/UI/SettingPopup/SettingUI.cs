using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public Sprite soundOn;
    public Sprite soundOff;
    public UnityEngine.UI.Image soundImage;
    private bool isSoundPlaying = true;
    private void Awake() {
        if(PlayerPrefs.HasKey("Sound"))
        {
            isSoundPlaying = PlayerPrefs.GetInt("Sound") == 1;
        }
        else
        {
            isSoundPlaying = true;

            PlayerPrefs.SetInt("Sound", 1);
        }

        AudioListener.volume = isSoundPlaying ? 1 : 0;

        soundImage.sprite = isSoundPlaying ? soundOn : soundOff;
    }
    public void BackToHome()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettingPopup()
    {
        GameManager.Instance.ChangeGameState(GameState.Paused);
    }

    public void CloseSettingPopup()
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
    }

    public void ActiveSound()
    {
        isSoundPlaying = !isSoundPlaying;
        AudioListener.volume = isSoundPlaying ? 1 : 0;
        soundImage.sprite = isSoundPlaying ? soundOn : soundOff;

        PlayerPrefs.SetInt("Sound", isSoundPlaying ? 1 : 0);
    }
}
