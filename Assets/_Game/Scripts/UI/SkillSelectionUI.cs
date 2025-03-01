using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private CardUI[] cardUIs;
    
    private List<BaseCard> cards = new List<BaseCard>();

    private void OnEnable()
    {
        cards = CardManager.Instance.Get3Card();
        
        if(cards.Count == 0)
        {
            GameManager.Instance.ChangeGameState(GameState.Playing);
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.Paused);
        }
        
        for (int i = 0; i < cards.Count; i++)
        {
            int index = i;
            cardUIs[index].SetCardData(cards[i].CardData());
            cardUIs[index].gameObject.SetActive(true);
            cardUIs[index].Button.onClick.RemoveAllListeners();
            cardUIs[index].Button.onClick.AddListener(() =>
            {
                CardManager.Instance.SelectCard(cards[index]);
                cards.Remove(cards[index]);
                this.gameObject.SetActive(false);
            });
            cardUIs[index].FlipCard();
        }
    }

    private void OnDisable()
    {
        foreach (var card in cards)
        {
            CardManager.Instance.ReturnCard(card);
        }
        
        foreach (var cardUI in cardUIs)
        {
            cardUI.gameObject.SetActive(false);
        }
    }
}
