using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour
{
	public GameObject projectile;
	public Transform shotPoint;
	public float timeBetweenShots;

	private float shotTime;
 
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			if (Time.time >= shotTime)
			{
				Instantiate(projectile, shotPoint.position, transform.rotation);
				shotTime = Time.time + timeBetweenShots;
			}
		}
    }

	private void FixedUpdate()
	{
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation  = Quaternion.AngleAxis(angle - 90, Vector3.forward);
		transform.rotation = rotation;
	}
}
