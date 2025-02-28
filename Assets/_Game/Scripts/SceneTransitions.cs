using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private Animator transitionAnim;

    private void Start()
    {
        transitionAnim = GetComponent<Animator>();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        if (sceneName.Equals("game"))
        {
            //PlayerPrefs.SetInt("FirstTime", 1);
            if (PlayerPrefs.HasKey("FirstTime"))
            {
                if (PlayerPrefs.GetInt("FirstTime") == 0)
                {
                    sceneName = "Main";
                }
            }
        }
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
