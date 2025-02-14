using System;
using UnityEngine;

namespace D
{
    
    public class PrefabManager : Singleton<PrefabManager>
    {
        [Header("Health Drop")]
        [SerializeField]
        private GameObject healthPickUp;
        public void SpawnHealthPickUp(Vector3 position)
        {
            Instantiate(healthPickUp, position, Quaternion.identity);
        }
        
        [Header("Exp Drop")]
        [SerializeField]
        private ExpPickUp expPickUp;
        private ObjectPool<ExpPickUp> expPickUpPool;
        private float expPickUpChanceWeight = 1 + 4 + 15 + 30 + 50;
        // rate increase the chance of getting higher level exp pick up
        public ExpPickUp SpawnExpPickUp(Vector3 position, float rate = 1f)
        {
            var expPickUp = expPickUpPool.Pull(position);
            float randomNumber = UnityEngine.Random.Range(0f, expPickUpChanceWeight) / rate;
            if (randomNumber <= 1)
            {
                expPickUp.SetExpLevel(5);
            }
            else if (randomNumber <= 5)
            {
                expPickUp.SetExpLevel(4);
            }
            else if (randomNumber <= 20)
            {
                expPickUp.SetExpLevel(3);
            }
            else if (randomNumber <= 50)
            {
                expPickUp.SetExpLevel(2);
            }
            else
            {
                expPickUp.SetExpLevel(1);
            }
            return expPickUpPool.Pull(position);
        }
        
        [Header("Death Effect")]
        [SerializeField]
        private GameObject deathEffect;
        public void SpawnDeathEffect(Vector3 position)
        {
            Instantiate(deathEffect, position, Quaternion.identity);
        }
        
        [Header("Damage Text")]
        [SerializeField] private DamageText damageTextPrefab;
        private ObjectPool<DamageText> damageTextPool;
        
        public void ShowDamageText(float damage, Vector3 position, bool isCritical = false)
        {
            var damageText = damageTextPool.Pull();
            damageText.transform.position = position;
            damageText.SetDamageText(damage,isCritical);
        }
        
        [Header("Summon enemy")]
        [SerializeField] private D.Enemy enemyToSummon;
        public void SummonEnemy(Vector3 position, int quantityPerSummon = 1)
        {
            for (int i = 0; i < quantityPerSummon; i++)
            {
                float randomX = UnityEngine.Random.Range(-1f, 1f);
                float randomY = UnityEngine.Random.Range(-1f, 1f);
                Vector2 randomPosition = position + new Vector3(randomX, randomY);
                Instantiate(enemyToSummon, randomPosition, Quaternion.identity).gameObject.SetActive(true);
            }
        }
        
        [Header("Enemy Data")]
        [SerializeField] private EnemyData[] enemyData;
        private int enemyDataTotalRate;
        
        public Enemy GetRandomEnemy()
        {
            int randomNumber = UnityEngine.Random.Range(0, enemyDataTotalRate);
            int rate = 0;
            foreach (var enemy in enemyData)
            {
                rate += enemy.rate;
                if (randomNumber < rate)
                {
                    return Instantiate(enemy.prefab);
                }
            }
            return null;
        }

        private void Awake()
        {
            foreach(var enemy in enemyData)
            {
                enemyDataTotalRate += enemy.rate;
            }
            damageTextPool = new ObjectPool<DamageText>(damageTextPrefab, new GameObject("Damage_Text_Holder").transform, 10);
            expPickUpPool = new ObjectPool<ExpPickUp>(expPickUp, new GameObject("Exp_Holder").transform, 10);
        }
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyType enemyType;
        public Enemy prefab;
        public int rate;
    }
}
