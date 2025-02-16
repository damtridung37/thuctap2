using System;
namespace D
{
    [Serializable]
    public class WeaponItem : ShopItem
    {
        public int damage;
        public int attackSpeed;
        public int range;
        public override void Activate(System.Action<bool> callback)
        {
            if (GameManager.Instance.playerData.CurrentGold < price)
            {
                callback(false);
                return;
            }
            GlobalEvent<(int, bool)>.Trigger("On_PlayerGoldChanged", (price, false));
            //Player.Instance.EquipWeapon(this);
            callback(true);
        }
    }
}
