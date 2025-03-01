using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : Character, IPoolable<Enemy>
    {
        [Header("Enemy Type")]
        [SerializeField] EnemyType enemyType;

        [Header("Health Drop")]
        [SerializeField]
        protected int healthPickUpChance;

        [Header("Movement")]
        [SerializeField]
        protected float stopDistance;

        protected SpriteRenderer spriteRenderer;

        protected Player player;

        protected float distanceToPlayer;

        public bool CanEarnGold = true;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            player = D.Player.Instance;
        }

        public override void TakeDamage(float damageAmount)
        {
            //currentHealth -= damageAmount;
            float postMitigationDamage = damageAmount * (100f / (100 + statBuffs[StatType.Armor].GetValue()));
            currentHealth -= postMitigationDamage;
            if (currentHealth <= 0)
            {
                int randomNumber = Random.Range(0, 101);
                if (randomNumber < healthPickUpChance)
                {
                    PrefabManager.Instance.SpawnHealthPickUp(transform.position);
                }

                ExpPickUp temp = PrefabManager.Instance.SpawnExpPickUp(transform.position);
                temp.boxCollider.enabled = true;
                SfxType sfxType = Random.Range(1, 4) switch
                {
                    1 => SfxType.Death1,
                    2 => SfxType.Death2,
                    3 => SfxType.Death3,
                    _ => SfxType.Death1
                };
                SoundManager.Instance.PlaySfx(sfxType);
                ReturnToPool();
            }
        }

        protected override void FixedUpdate()
        {
            if (player.IsDead) return;

            attackTime -= Time.fixedDeltaTime;
            if (attackTime < 0) attackTime = 0;

            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            Move();
            Flip();
            Render();
            Attack();
        }

        protected void Render()
        {
            if (player.transform.position.y > transform.position.y)
            {
                spriteRenderer.sortingOrder = 1;
            }
            else
            {
                spriteRenderer.sortingOrder = -1;
            }
        }

        protected void Flip()
        {
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        protected override void Move()
        {
            if (distanceToPlayer > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, statBuffs[StatType.Speed].GetValue() * Time.deltaTime);
            }
        }

        protected override void Attack()
        {
            if (distanceToPlayer > stopDistance) return;

            if (attackTime <= 0)
            {
                attackTime += (1 / statBuffs[StatType.AttackSpeed].GetValue());

                float damage = statBuffs[StatType.Damage].GetValue();
                float critChance = statBuffs[StatType.CritChance].GetValue();
                float critDamage = statBuffs[StatType.CritDamage].GetValue();
                bool isCrit = false;
                float randomNumber = Random.Range(0, 101);
                if (randomNumber <= critChance)
                {
                    damage *= 1 + critDamage / 100f;
                    isCrit = true;
                }

                player.TakeDamage(damage);
                PrefabManager.Instance.ShowDamageText(damage, player.transform.position, isCrit);
            }
        }
        private Action<Enemy> returnAction;
        public void Initialize(Action<Enemy> returnAction)
        {
            this.returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            this.returnAction?.Invoke(this);
        }
    }
}
