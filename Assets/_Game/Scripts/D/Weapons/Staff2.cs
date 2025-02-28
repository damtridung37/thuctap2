using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public class Staff2 : Weapon
    {

        public override void Attack(Character character)
        {
            this.character = character;
            Fire();
        }

        public void Fire()
        {

            // 5 ammo (15 degrees between each ammo)
            int start = -30;
            for (int i = 1; i < 5; i++)
            {
                var ammo2 = ammoPool.Pull(firePoint.position, firePoint.rotation);
                ammo2.transform.SetParent(ammoHolder);
                ammo2.gameObject.SetActive(true);
                ammo2.transform.Rotate(0, 0, start + (15 * i));
                ammo2.Init(character, ammoSpeed);
            }
        }
    }
}
