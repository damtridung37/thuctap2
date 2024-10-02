using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
	player playerScript;
	public int healAmount;

	private void Start()
	{
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			playerScript.Heal(healAmount);
			Destroy(gameObject);
		}
	}
}
