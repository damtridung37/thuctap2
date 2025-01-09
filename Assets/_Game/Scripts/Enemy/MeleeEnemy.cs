using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeEnemy : Enemy
{
    public float stopDistance;

	private float attackTime;

	public float meleeAttackSpeed;


	private void Update()
	{
		if (GameManager.Instance.GameState == GameState.Paused) return;
		if (player != null)
		{

			if (player.position.x < transform.position.x)
			{
				// Người chơi bên trái -> Quái vật quay trái (lật trục X)
				transform.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				// Người chơi bên phải -> Quái vật quay phải (bình thường)
				transform.localScale = new Vector3(1, 1, 1);
			}

			if (Vector2.Distance(transform.position, player.transform.position) > stopDistance)
			{
				transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
			}
			else
			{
				if (Time.time > attackTime)
				{
					attackTime = Time.time + timeBetweenAttacks;
					StartCoroutine(Attack());
				}
			}
		}
			
	}
		IEnumerator Attack()
		{
			player.GetComponent<Player>().TakeDamage(damage);
			
			Vector2 originalPosition = transform.position;
			Vector2 targetPosition = player.position;

			float percent = 0;
			while (percent <= 1)
			{
				percent += Time.deltaTime * meleeAttackSpeed;
				float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
				transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
				yield return null;
			}
		}
}
