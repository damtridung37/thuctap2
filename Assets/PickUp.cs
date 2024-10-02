using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public weapons weaponToEquip;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<player>().ChangWeapon(weaponToEquip);
			Destroy(gameObject);
		}
	}

}
