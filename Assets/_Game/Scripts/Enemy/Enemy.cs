using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health;

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


	public virtual void Start()
	{
        player = CacheDataManager.instance.player.transform;
	}
    
	public void TakeDamage(int damageAmount)
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
            
            ExpPickUp temp = Instantiate(CacheDataManager.instance.ExpPickUpPrefab,transform.position,Quaternion.identity);
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
            Destroy(gameObject);

        }
    }
}
