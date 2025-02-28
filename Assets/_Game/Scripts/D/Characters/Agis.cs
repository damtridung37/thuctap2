using System;
using System.Linq;
using UnityEngine;

namespace D
{
    public class Agis : Enemy
    {
        [Header("Summon settings")]
        [SerializeField] private float timeBetweenSummons;
        private float summonTime;
        [SerializeField] private int quantityPerSummon = 1;
        private Transform[] summonPoints;

        [Header("Shooting settings")]
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private AgisBullet bulletPrefab;

        [Header("Audio")]
        [SerializeField] private AudioClip[] sfx;
        [SerializeField] private AudioClip[] sfxHit;

        private ObjectPool<AgisBullet> bulletPool;
        private int hitPlayerCount = 0;

        protected override void Awake()
        {
            summonTime = timeBetweenSummons;
            bulletPool = new ObjectPool<AgisBullet>(bulletPrefab, new GameObject("AgisBullet").transform, 10);
            GlobalEvent<float>.Subscribe("On_EnemyDown", OnEnemyDown);
            base.Awake();
        }

        public override void InitStats()
        {
            base.InitStats();
            UIManager.Instance.InitBossUI("Agis", new HealthData
            {
                currentHealth = currentHealth,
                maxHealth = statBuffs[StatType.Health].GetValue(),
                isHealing = false
            });
            GlobalEvent<bool>.Subscribe("OnPlayerDead", OnPlayerDead);
        }

        private void OnPlayerDead(bool isDead)
        {
            if (isDead)
            {
                GlobalEvent<float>.Unsubscribe("On_EnemyDown", OnEnemyDown);
                GlobalEvent<bool>.Unsubscribe("OnPlayerDead", OnPlayerDead);
                UIManager.Instance.DisableBossUI();
                Destroy(this.gameObject);
            }
        }

        private void OnEnemyDown(float enemyMaxHp)
        {
            this.currentHealth -= enemyMaxHp;
            if (this.currentHealth <= 0)
            {
                this.currentHealth = 0;
                OnDead();
            }
            GlobalEvent<HealthData>.Trigger("BossHealthChanged", new HealthData
            {
                currentHealth = currentHealth,
                maxHealth = statBuffs[StatType.Health].GetValue(),
                isHealing = false
            });
        }

        private void OnDead()
        {
            GlobalEvent<bool>.Trigger("OnBossDead", true);
            GlobalEvent<float>.Unsubscribe("On_EnemyDown", OnEnemyDown);
            UIManager.Instance.DisableBossUI();
            Destroy(this.gameObject);
        }

        protected override void FixedUpdate()
        {
            if (player.IsDead) return;

            attackTime -= Time.fixedDeltaTime;
            if (attackTime < 0) attackTime = 0;

            summonTime -= Time.fixedDeltaTime;
            if (summonTime < 0) summonTime = 0;

            this.Attack();
        }

        protected override void Attack()
        {
            if (attackTime <= 0)
            {
                attackTime += (1 / statBuffs[StatType.AttackSpeed].GetValue());

                // Get random shoot point
                Transform shootPoint = firePoints[UnityEngine.Random.Range(0, firePoints.Length)];


                Vector2 direction = (player.transform.position - shootPoint.position).normalized;

                // look at player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                shootPoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                var temp = bulletPool.Pull(shootPoint.position);
                temp.direction = direction;
                temp.isShoot = true;
                temp.onHitPlayer = OnHitPlayer;

                SoundManager.Instance.PlaySfx(SfxType.Agis_Shot);
            }

            if (summonTime <= 0)
            {
                summonTime = timeBetweenSummons;
                Summon();
            }
        }

        private void OnHitPlayer()
        {
            hitPlayerCount++;
            Debug.Log(hitPlayerCount);
            if (hitPlayerCount < 5 && hitPlayerCount > 0)
            {
                SoundManager.Instance.PlayCustomSfx(sfxHit[hitPlayerCount - 1]);
            }
            if (hitPlayerCount == 4)
            {
                player.TakeDamage(9999);
                PrefabManager.Instance.ShowDamageText(9999, player.transform.position, false);
            }
        }

        private void Summon()
        {
            AudioClip clip = sfx[UnityEngine.Random.Range(0, sfx.Length)];
            SoundManager.Instance.PlayCustomSfx(clip);
            D.GameManager.Instance.currentRoom.SpawnEnemy();
        }
    }

    public static class Utility
    {
        private static readonly System.Random _random = new System.Random();
        // Generic method to get a random enum value
        public static T GetRandom<T>(params T[] exclude) where T : Enum
        {
            // Get all enum values and exclude specified ones
            var values = Enum.GetValues(typeof(T)).Cast<T>().Except(exclude).ToArray();

            // If all values are excluded, throw an exception
            if (values.Length == 0)
                throw new InvalidOperationException("No valid values left after exclusions.");

            return values[_random.Next(values.Length)];
        }
    }
}
