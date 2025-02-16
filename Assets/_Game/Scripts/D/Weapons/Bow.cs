using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public class Bow : Weapon
    {
        [SerializeField] private Ammo arrowPrefab;
        [SerializeField] private float ammoSpeed;
        [SerializeField] private Animator bowAnim;
        private Character character;

        public override void Attack(Character character)
        {
            this.character = character;
            bowAnim.speed = character.StatBuffs[StatType.AttackSpeed].GetValue();
            bowAnim.Play("Fire");
        }

        public void Fire()
        {
            var arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            arrow.gameObject.SetActive(true);
            arrow.Init(character, ammoSpeed);
        }
    }
}
