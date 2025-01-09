using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CardManager : Singleton<CardManager>
{
    [SerializeField]
    List<BaseCard> cards = new List<BaseCard>();
    
    List<Effect> activeEffects = new List<Effect>();
    
    private void SuffuleCards()
    {
        cards.Shuffle();
    }
    
    public List<BaseCard> Get3Card()
    {
        if (cards.Count == 0)
        {
            SuffuleCards();
        }
        var result = new List<BaseCard>();
        
        for (int i = 0; i < 3; i++)
        {
            if (cards.Count == 0) break;
            
            result.Add(cards[^1]);
            cards.RemoveAt(cards.Count - 1);
        }

        return result;
    }

    public void SelectCard(BaseCard baseCard)
    {
        if (baseCard is Effect e)
        {
            activeEffects.Add(e);
            Instantiate(baseCard, transform);
        }
        
        GameManager.Instance.ChangeGameState(GameState.Playing);
    }
    
    public void ReturnCard(BaseCard baseCard)
    {
        cards.Add(baseCard);
    }
}

public static class ListExtensions
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
