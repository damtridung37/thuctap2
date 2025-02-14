using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private string[] gameoverTexts;
    [SerializeField] private string[] tooltips;

    public void Open(bool isGameover = false)
    {
        gameObject.SetActive(true);
        if(isGameover)
            loadingText.text = gameoverTexts[Random.Range(0, gameoverTexts.Length)];
        else
            loadingText.text = tooltips[Random.Range(0, tooltips.Length)];
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
