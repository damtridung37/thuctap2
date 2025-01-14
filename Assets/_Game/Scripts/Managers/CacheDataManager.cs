using System.Collections.Generic;
using UnityEngine;

public class CacheDataManager : Singleton<CacheDataManager>
{
    public Player player;
    [Header("Exp Pick Up")]
    [SerializeField]
    private ExpPickUp ExpPickUpPrefab;
    [SerializeField]
    private Transform expPickUpParent;
    public ObjectPool<ExpPickUp> expPickUpPool;
    
    [Header("Enemy Bullet")]
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private Transform enemyBulletParent;
    public ObjectPool<EnemyBullet> enemyBulletPool;
    
    private Dictionary<Collider2D,Enemy> enemyCache = new Dictionary<Collider2D, Enemy>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        expPickUpPool = new ObjectPool<ExpPickUp>(ExpPickUpPrefab,expPickUpParent, 10);
        enemyBulletPool = new ObjectPool<EnemyBullet>(enemyBulletPrefab, enemyBulletParent, 10);
    }
    
    public Enemy GetEnemy(Collider2D enemyCollider)
    {
        if (enemyCache.ContainsKey(enemyCollider))
        {
            return enemyCache[enemyCollider];
        }

        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        enemyCache.Add(enemyCollider, enemy);
        return enemy;
    }
}
