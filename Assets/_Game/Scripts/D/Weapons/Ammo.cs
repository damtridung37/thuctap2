using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Ammo : MonoBehaviour
    {
        private Character owner;
        private float speed;
        private float autoDestroyTime = 5f;
        private BoxCollider2D boxCollider;

        // private void Start()
        // {
        //     boxCollider = GetComponent<BoxCollider2D>();
        // }

        public void Init(Character owner, float speed)
        {
            this.owner = owner;
            this.speed = speed;
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.enabled = true;
        }
        private void FixedUpdate()
        {
            transform.position += transform.up * (speed * Time.fixedDeltaTime);
            autoDestroyTime -= Time.fixedDeltaTime;
            if (autoDestroyTime <= 0)
            {
                Destroy(gameObject);
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
                    Debug.Log("Damage: " + damage + " Crit Chance: " + critChance + " Crit Damage: " + critDamage);
                    bool isCrit = false;
                    float randomNumber = Random.Range(0, 101);
                    if (randomNumber < critChance)
                    {
                        damage *= 1 + critDamage / 100f;
                        isCrit = true;
                    }

                    character.TakeDamage(damage);
                    PrefabManager.Instance.ShowDamageText(damage, character.transform.position, isCrit);
                    Destroy(gameObject);
                }
            }
        }
    }
}
