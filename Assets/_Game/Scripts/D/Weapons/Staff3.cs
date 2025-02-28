using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public class Staff3 : Weapon
    {

        public override void Attack(Character character)
        {
            this.character = character;
            Fire();
        }

        public void Fire()
        {
            var ammo = ammoPool.Pull(firePoint.position, firePoint.rotation);
            ammo.transform.SetParent(ammoHolder);
            ammo.gameObject.SetActive(true);
            ammo.Init(character, ammoSpeed);
        }
    }
}
