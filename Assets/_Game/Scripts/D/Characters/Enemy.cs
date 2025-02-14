using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace D
{
    public class Enemy : Character
    {
        [Header("Enemy Type")]
        [SerializeField] EnemyType enemyType;
        
        [Header("Health Drop")]
        [SerializeField]
        private int healthPickUpChance;
        
        [Header("Movement")]
        protected float stopDistance;
        
        private Player player;
        
        private float attackTime;
        
        protected override void Awake()
        {
            base.Awake();
            player = D.Player.Instance;
        }
        
        public override void TakeDamage(float damageAmount)
            {
                currentHealth -= damageAmount;
                if (currentHealth <= 0)
                {
                    int randomNumber = Random.Range(0, 101);
                    if (randomNumber < healthPickUpChance) 
                    {
                        PrefabManager.Instance.SpawnHealthPickUp(transform.position);             
                    }

                    ExpPickUp temp = PrefabManager.Instance.SpawnExpPickUp(transform.position);
        
                    PrefabManager.Instance.SpawnDeathEffect(transform.position);
                    
                    //ReturnToPool();
                }
            }

        protected void FixedUpdate()
        {
            attackTime -= Time.fixedDeltaTime;
            if (attackTime < 0) attackTime = 0;
            
            if(player.IsDead) return;
            Move();
        }

        protected virtual void Move()
        {
            
                if (player.transform.position.x < transform.position.x)
                {
                    // Người chơi bên trái -> Quái vật quay trái (lật trục X)
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    // Người chơi bên phải -> Quái vật quay phải (bình thường)
                    transform.localScale = new Vector3(1, 1, 1);
                }

                if (Vector2.Distance(transform.position, player.transform.position) > stopDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, statBuffs[StatType.Speed].GetValue() * Time.deltaTime);
                }
                else
                {
                    if (attackTime <= 0)
                    {
                        attackTime += (1 / statBuffs[StatType.AttackSpeed].GetValue());
                        StartCoroutine(Attack());
                    }
                }
            
        }
        
        IEnumerator Attack()
        {
            float damage = statBuffs[StatType.Damage].GetValue();
            float critChance = statBuffs[StatType.CritChance].GetValue();
            float critDamage = statBuffs[StatType.CritDamage].GetValue();
            bool isCrit = false;
            float randomNumber = Random.Range(0, 101);
            if (randomNumber <= critChance)
            {
                damage *= 1 + critDamage/100f;
                isCrit = true;
            }
            
            player.TakeDamage(damage);
            PrefabManager.Instance.ShowDamageText(damage, player.transform.position, isCrit);

            yield return null;
            // Vector2 originalPosition = transform.position;
            // Vector2 targetPosition = player.transform.position;
            //
            // float percent = 0;
            // while (percent <= 1)
            // {
            //     percent += Time.deltaTime * (1 / statBuffs[StatType.AttackSpeed].GetValue());
            //     float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            //     transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
            //     yield return null;
            // }
        }
    }
}
