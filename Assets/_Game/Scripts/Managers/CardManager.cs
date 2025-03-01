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

        SuffuleCards();
        
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
        for(int i = 0; i < n; i++)
        {
            int r = i + rng.Next(n - i);
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }
}
