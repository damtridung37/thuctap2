using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card Data", order = 0)]
public class CardDataSO : ScriptableObject
{
    public CardData data;
}

[Serializable]
public struct CardData
{
    public CardType type;
    public string title;
    public string description;
    public Sprite icon;
}

public enum CardType
{
    Equipment,
    Effect,
    Stat
}
