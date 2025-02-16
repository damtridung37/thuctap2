using UnityEngine;
using UnityEngine.UI;

namespace D
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private ShopItemUI[] shopItems;
        [SerializeField] private ShopItemLib shopItemLib;
        [SerializeField] private Button closeBtn;

        private void Start()
        {
            closeBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private int currentShopId = 0;

        public void Init(int shopId)
        {
            gameObject.SetActive(true);

            if (currentShopId == shopId)
            {
                return;
            }

            ShopItem[] items = shopItemLib.GetRandomItem(3, Rarity.Legendary);

            for (int i = 0; i < items.Length; i++)
            {
                shopItems[i].SetData(items[i]);
            }

            currentShopId = shopId;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
