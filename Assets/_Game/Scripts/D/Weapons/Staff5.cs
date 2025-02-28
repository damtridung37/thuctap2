using UnityEngine;

namespace D
{
    public class Staff5 : Weapon
    {

        public override void Attack(Character character)
        {
            this.character = character;
            Invoke(nameof(Fire), 0.2f);
            Invoke(nameof(Fire), 0.4f);
            Invoke(nameof(Fire), 0.6f);
            Invoke(nameof(Fire), 0.8f);
            Invoke(nameof(Fire), 0.1f);
        }

        public void Fire()
        {
            var ammo = ammoPool.Pull(firePoint.position, firePoint.rotation);
            ammo.transform.SetParent(ammoHolder);
            ammo.gameObject.SetActive(true);
            ammo.transform.Rotate(0, 0, Random.Range(-30, 30));
            ammo.Init(character, ammoSpeed);
        }
    }
}
