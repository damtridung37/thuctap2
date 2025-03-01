using System;
using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    
    public Button Button => button;
    
    private void Start()
    {
        button.onClick.AddListener(FlipCard);
    }

    public void SetCardData(CardData cardData)
    {
        title.text = cardData.title;
        description.text = cardData.description;
        icon.sprite = cardData.icon;
    }

    [ContextMenu("Flip")]
    public void FlipCard()
    {
        LSequence.Create()
            .Append(LMotion.Create(0f,90f,.25f)
                .WithOnComplete(() =>
                {
                    front.SetActive(true);
                    back.SetActive(false);
                })
                .Bind(this, (x,target)=>target.transform.localRotation = Quaternion.Euler(0f,x,0f)))
            .Append(LMotion.Create(90f,0f,.25f)
                .Bind(this, (x,target)=>target.transform.localRotation = Quaternion.Euler(0f,x,0f)))
            .Run();
    }

    private void OnDisable()
    {
        front.SetActive(false);
        back.SetActive(true);
    }
}
