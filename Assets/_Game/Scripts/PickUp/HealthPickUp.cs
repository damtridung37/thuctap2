using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
	
	public int healAmount;

	public GameObject effect;	

	

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Instantiate(effect, transform.position, Quaternion.identity);
			CacheDataManager.instance.player.Heal(healAmount);
			Destroy(gameObject);
		}
	}
}
