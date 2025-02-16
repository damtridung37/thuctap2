using System;
using UnityEngine;

namespace D
{
    [Serializable]

    public class ShopItem
    {
        public string name;
        public string description;
        public Sprite icon;
        public int price;
        public Rarity rarity;

        public virtual void Activate(Action<bool> callback)
        {
            if (GameManager.Instance.playerData.CurrentGold < price)
            {
                callback(false);
                return;
            }
            GlobalEvent<(int, bool)>.Trigger("On_PlayerGoldChanged", (price, false));
        }

    }

    public enum Rarity : int
    {
        Common = 50,
        Uncommon = 30,
        Rare = 15,
        Epic = 4,
        Legendary = 1
    }
}
