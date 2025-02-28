using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Ammo : MonoBehaviour, IPoolable<Ammo>
    {
        private Character owner;
        private float speed;
        [SerializeField]
        private float autoDestroyTime = 5f;
        private float countdown;
        private BoxCollider2D boxCollider;
        [SerializeField] private float damageScale = 1f;
        [SerializeField] private bool canPierce = false;



        public void Init(Character owner, float speed)
        {
            this.owner = owner;
            this.speed = speed;
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.enabled = true;
            countdown = autoDestroyTime;
        }

        private Action<Ammo> returnAction;

        public void Initialize(System.Action<Ammo> returnAction)
        {
            this.returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            this.returnAction?.Invoke(this);
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * (speed * Time.fixedDeltaTime);
            countdown -= Time.fixedDeltaTime;
            if (countdown <= 0)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Character character))
            {
                if (character != owner)
                {
                    float damage = owner.StatBuffs[StatType.Damage].GetValue();
                    float critChance = owner.StatBuffs[StatType.CritChance].GetValue();
                    float critDamage = owner.StatBuffs[StatType.CritDamage].GetValue();
                    bool isCrit = false;
                    float randomNumber = Random.Range(0, 101);
                    if (randomNumber < critChance)
                    {
                        damage *= 1 + critDamage / 100f;
                        isCrit = true;
                    }

                    damage *= damageScale;

                    character.TakeDamage(damage);
                    PrefabManager.Instance.ShowDamageText(damage, character.transform.position, isCrit);
                    if (!canPierce)
                    {
                        ReturnToPool();
                    }
                }
            }
        }
    }
}
