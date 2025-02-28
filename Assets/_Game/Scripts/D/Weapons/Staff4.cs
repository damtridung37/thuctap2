using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public class Staff4 : Weapon
    {

        public override void Attack(Character character)
        {
            this.character = character;
            Invoke(nameof(Fire), 0.15f);
            Invoke(nameof(Fire), 0.3f);
            Invoke(nameof(Fire), 0.45f);
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
