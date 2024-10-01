using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour
{
	public projectile projectile;
	public Transform shotPoint;
	public float timeBetweenShots;
	public List<float> angles = new List<float>();

	private float shotTime;
 
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			if (Time.time >= shotTime)
			{
				for (int i = 0; i < angles.Count; i++)
				{
					projectile temp = Instantiate(projectile, shotPoint.position, transform.rotation);
					temp.Init(angles[i]);
				}

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
