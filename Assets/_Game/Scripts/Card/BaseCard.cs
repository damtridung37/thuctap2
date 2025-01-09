using UnityEngine;

public class BaseCard : MonoBehaviour
{
    [SerializeField]
    protected CardDataSO cardData;
    
    public CardData CardData()
    {
        return cardData.data;
    }
}
