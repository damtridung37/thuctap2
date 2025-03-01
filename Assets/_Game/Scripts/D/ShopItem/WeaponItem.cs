using System;
namespace D
{
    [Serializable]
    public class WeaponItem : ShopItem
    {
        public string weaponName;
        public override void Activate(System.Action<bool> callback)
        {
            if (GameManager.Instance.playerData.CurrentGold < price)
            {
                callback(false);
                return;
            }
            GlobalEvent<(int, bool)>.Trigger("On_PlayerGoldChanged", (price, false));
            Player.Instance.ChangeWeapon(weaponName);
            callback(true);
        }
    }
}
