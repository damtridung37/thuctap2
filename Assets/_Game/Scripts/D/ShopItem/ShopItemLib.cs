using System.Collections.Generic;
using UnityEngine;

namespace D
{
    [CreateAssetMenu(fileName = "Item", menuName = "Shop/ShopItem")]
    public class ShopItemLib : ScriptableObject
    {
        [SerializeReference] private List<ShopItem> shopItems = new List<ShopItem>();

        /// <summary>
        /// Get random item from the shop
        /// </summary>
        /// <param name="count"></param>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public ShopItem[] GetRandomItem(int count = 3, Rarity rarity = Rarity.Common)
        {
            List<ShopItem> items = new List<ShopItem>();
            int totalWeight = 0;
            foreach (var item in shopItems)
            {
                if (item.rarity >= rarity)
                {
                    items.Add(item);
                    totalWeight += (int)item.rarity;
                }
            }

            ShopItem[] result = new ShopItem[count];

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0, totalWeight);
                int currentWeight = 0;
                foreach (var item in items)
                {
                    currentWeight += (int)item.rarity;
                    if (random < currentWeight)
                    {
                        result[i] = item;
                        break;
                    }
                }
            }

            return result;
        }

    }
}
