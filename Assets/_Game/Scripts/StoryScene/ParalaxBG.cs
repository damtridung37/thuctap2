using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParalaxBG : MonoBehaviour
{
    [SerializeField] private RawImage farImage;
    [SerializeField] private RawImage midImage;
    [SerializeField] private RawImage closeImage;
    [SerializeField] private string[] storyLines;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private CanvasGroup loadScene;

    Vector2 farPos;
    Vector2 midPos;
    Vector2 closePos;

    int currentSceneIndex;

    private int currentLine = 0;
    bool isWriting = false;

    private void Start()
    {
        currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) return;
        LMotion.Create(0, 0, 0).RunWithoutBinding();
        WriteTextAnimation();
    }


    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        farPos = new Vector2(mousePos.x * 0.2f, (mousePos.y - .5f) * 0.2f);
        midPos = new Vector2(mousePos.x * 0.3f, (mousePos.y - .5f) * 0.3f);
        closePos = new Vector2(mousePos.x * 0.4f, (mousePos.y - .5f) * 0.4f);

        if (!isWriting && Input.GetMouseButtonDown(0))
        {
            WriteTextAnimation();
        }

    }

    private void FixedUpdate()
    {
        farImage.uvRect = new Rect(Vector2.Lerp(farImage.uvRect.position, farPos, 0.01f), new Vector2(1, 1));
        midImage.uvRect = new Rect(Vector2.Lerp(midImage.uvRect.position, midPos, 0.02f), new Vector2(1, 1));
        closeImage.uvRect = new Rect(Vector2.Lerp(closeImage.uvRect.position, closePos, 0.03f), new Vector2(1, 1));
    }

    /// <summary>
    /// Write char by char the text of the story
    /// </summary>
    private void WriteTextAnimation()
    {
        isWriting = true;
        if (currentSceneIndex == 0)
        {
            return;
        }
        if (currentLine < storyLines.Length)
        {
            storyText.text = "";
            StartCoroutine(WriteText(storyLines[currentLine]));
        }
        else
        {
            storyText.text = "";
            // End of the story
            if (loadScene != null)
                LMotion.Create(0f, 1f, 2f)
                .WithOnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(5))
                .Bind(loadScene, (x, s) => s.alpha = x);
        }
    }


    private System.Collections.IEnumerator WriteText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            storyText.text += text[i];
            yield return new WaitForSeconds(0.1f);
        }
        currentLine++;
        isWriting = false;
    }
}
