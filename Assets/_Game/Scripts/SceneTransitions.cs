using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private Animator transitionAnim;

    private void Start()
	{
		transitionAnim = GetComponent<Animator> ();
	}

	public void LoadScene(string sceneName)
	{
		StartCoroutine(Transition(sceneName));
	}

	IEnumerator Transition(string sceneName)
	{
		transitionAnim.SetTrigger("End");
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(sceneName);
	}
}
