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

	public float detectionRadius = 5f; // Bán kính để phát hiện enemy

	private float shotTime;

	void Update()
	{
		GameObject nearestTarget = FindNearestTarget();
		if (nearestTarget != null && Time.time >= shotTime)
		{
			// Xoay về phía mục tiêu gần nhất
			Vector2 direction = (nearestTarget.transform.position - transform.position).normalized;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			transform.rotation = rotation;
		}


			/*if (Input.GetMouseButtonDown(0))
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
			}*/

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

	GameObject FindNearestTarget()
	{
		GameObject nearestTarget = null;
		float nearestDistance = detectionRadius;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemy in enemies)
			{
				float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
				if (distanceToEnemy < nearestDistance)
				{
					nearestDistance = distanceToEnemy;
					nearestTarget = enemy;
				}
			}
			GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
			foreach (GameObject boss in bosses)
			{
				float distanceToBoss = Vector2.Distance(transform.position, boss.transform.position);
				if (distanceToBoss < nearestDistance)
				{
					nearestDistance = distanceToBoss;
					nearestTarget = boss;
				}
			}

			return nearestTarget;
		

		/*private void FixedUpdate()
		{
			Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion rotation  = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			transform.rotation = rotation;
		}*/

	}
}
