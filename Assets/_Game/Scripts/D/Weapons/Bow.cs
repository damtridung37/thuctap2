using UnityEngine;

namespace D
{
    public class Bow : Weapon
    {
        [SerializeField] private Animator bowAnim;

        public override void Attack(Character character)
        {
            this.character = character;
            bowAnim.speed = character.StatBuffs[StatType.AttackSpeed].GetValue();
            bowAnim.Play("Fire");
        }

        public void Fire()
        {
            var arrow = ammoPool.Pull(firePoint.position, firePoint.rotation);
            arrow.gameObject.SetActive(true);
            arrow.Init(character, ammoSpeed);
        }
    }
}
