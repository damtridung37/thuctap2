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

    public float timeBetweenAttacks;

    public int healthPickUpChance;
    public GameObject healthPickUp;

	public virtual void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            int randomHealth = Random.Range(0, 101);
            if (randomHealth < healthPickUpChance) 
            {
                Instantiate(healthPickUp, transform.position, transform.rotation);
            }
            Destroy(gameObject);

        }
    }
}
