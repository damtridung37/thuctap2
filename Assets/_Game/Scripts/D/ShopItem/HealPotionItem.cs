using System;
namespace D
{
    [Serializable]
    public class HealPotionItem : ShopItem
    {
        public override void Activate(Action<bool> c)
        {
            if (GameManager.Instance.playerData.CurrentGold < price)
            {
                c(false);
                return;

            }
            GlobalEvent<(int, bool)>.Trigger("On_PlayerGoldChanged", (price, false));
            float playerMaxHealth = Player.Instance.StatBuffs[StatType.Health].GetValue();
            int healPercentage = rarity switch
            {
                Rarity.Common => 5,
                Rarity.Uncommon => 15,
                Rarity.Rare => 30,
                Rarity.Epic => 50,
                Rarity.Legendary => 75,
                _ => 0,
            };
            Player.Instance.Heal(healPercentage * playerMaxHealth / 100);
            c(true);
        }
    }
}
