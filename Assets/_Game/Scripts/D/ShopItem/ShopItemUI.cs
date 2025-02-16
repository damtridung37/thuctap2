using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace D
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button buyButton;

        public bool IsBought { get; private set; }
        public void SetData(ShopItem item)
        {
            IsBought = false;
            // Set data to UI
            icon.sprite = item.icon;
            nameText.text = item.name;
            descriptionText.text = item.description;
            priceText.text = "Cost: " + item.price.ToString();

            nameText.color = item.rarity switch
            {
                Rarity.Common => Color.green,
                Rarity.Uncommon => Color.blue,
                Rarity.Rare => Color.magenta,
                Rarity.Epic => Color.yellow,
                Rarity.Legendary => Color.red,
                _ => Color.white
            };

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                // Buy item
                item.Activate((b) =>
                {
                    if (b)
                    {
                        IsBought = b;
                        buyButton.interactable = false;
                        priceText.text = "Sold";
                    }
                });
            });
            buyButton.interactable = !IsBought;
        }
    }
}
