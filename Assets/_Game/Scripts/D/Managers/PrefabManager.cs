using System;
using System.Collections.Generic;
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
            return;
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
            return expPickUp;
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
        
        [Header("Enemy Data")]
        [SerializeField] private EnemyData[] enemyData;
        private int enemyDataTotalRate;
        private List<Enemy> activeEnemy = new List<Enemy>();
        Dictionary<EnemyType, ObjectPool<Enemy>> enemyPools = new Dictionary<EnemyType, ObjectPool<Enemy>>();
        
        private Enemy GetEnemyFromPool(EnemyType enemyType)
        {
            if (!enemyPools.ContainsKey(enemyType))
            {
                enemyPools.Add(enemyType, 
                    new ObjectPool<Enemy>(
                        enemyData[(int)enemyType].prefab,
                        null,
                        (e) =>
                        {
                            activeEnemy.Remove(e);
                            if(activeEnemy.Count == 0)
                            GlobalEvent<bool>.Trigger("Clear_Enemy", true);
                        }, 
                        new GameObject($"Enemy_{enemyType}_Holder").transform, 
                        10));
            }
            Enemy enemy = enemyPools[enemyType].Pull();
            activeEnemy.Add(enemy);
            return enemy;
        }
        
        public Enemy GetClosestEnemy(Vector3 position)
        {
            if (activeEnemy == null) return null;
            Enemy closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach (var enemy in activeEnemy)
            {
                if (enemy == null) continue;
                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
        
        public void ClearEnemy()
        {
            if (activeEnemy == null) return;
            foreach (var enemy in activeEnemy)
            {
                enemy.gameObject.SetActive(false);
            }
            activeEnemy.Clear();
        }
        
        public void SpawnEnemy(EnemyType enemyType, Vector3 position, int quantity = 1)
        {
            for(int i = 0; i < quantity; i++)
            {
                var enemy = GetEnemyFromPool(enemyType);
                enemy.transform.position = position;
                enemy.gameObject.SetActive(true);
            }
        }
        
        public Enemy GetRandomEnemy()
        {
            int randomNumber = UnityEngine.Random.Range(0, enemyDataTotalRate);
            int rate = 0;
            foreach (var enemy in enemyData)
            {
                rate += enemy.rate;
                if (randomNumber < rate)
                {
                    EnemyType enemyType = enemy.enemyType;
                    return GetEnemyFromPool(enemyType);
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
