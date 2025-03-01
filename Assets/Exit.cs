using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public Button exitButton;

	private void Start()
	{
		exitButton.onClick.AddListener(QuitGame);
	}

	private void QuitGame() 
	{ 
		Application.Quit();
	}

}
