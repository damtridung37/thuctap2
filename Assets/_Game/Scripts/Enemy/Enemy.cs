using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] EnemyType enemyType;
    public EnemyType EnemyType => enemyType;
    
	public int maxHealth;
    
    private float health;

    [HideInInspector]
    public Transform player;

    public float speed;

    public int damage;

    public int pickUpChance;
    public GameObject[] pickups;

    public float timeBetweenAttacks;

    [Header("Health Drop")]
    public int healthPickUpChance;
    public GameObject healthPickUp;

    public GameObject deathEffect;

    private Action<Enemy> returnAction;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void Start()
	{
        player = CacheDataManager.Instance.player.transform;
	}

    public void Initialize(Action<Enemy> returnAction)
    {
        this.returnAction = returnAction;
    }
    
    public void ReturnToPool()
    {
        this.returnAction?.Invoke(this);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber < pickUpChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
               
                Destroy(Instantiate(randomPickup, transform.position, transform.rotation),10f);
                
            }

            randomNumber = Random.Range(0, 101);
            if (randomNumber < healthPickUpChance) 
            {
                Instantiate(healthPickUp, transform.position, transform.rotation);                 
            }
            
            ExpPickUp temp = CacheDataManager.Instance.expPickUpPool.Pull(transform.position);
            randomNumber = Random.Range(0, 101);
            if (randomNumber < 70)
            {
                temp.SetExpLevel(1);
            }
            else
            {
                randomNumber = Random.Range(0, 101);
                if (randomNumber < 70)
                {
                    temp.SetExpLevel(2);
                }
                else
                {
                    randomNumber = Random.Range(0, 101);
                    if (randomNumber < 70)
                    {
                        temp.SetExpLevel(3);
                    }
                    else
                    {
                        randomNumber = Random.Range(0, 101);
                        if (randomNumber < 70)
                        {
                            temp.SetExpLevel(4);
                        }
                        else
                        {
                            temp.SetExpLevel(5);
                        }
                    }
                }
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity);
            ReturnToPool();
        }
    }
}

public enum EnemyType
{
    Slime,
    Summoner,
    Ranger,
    SkeletonMinion,
    Boss
}
