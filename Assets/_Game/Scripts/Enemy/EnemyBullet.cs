using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IPoolable<EnemyBullet>
{
	private Vector2 targetPosition;

	public float speed;
	public int damage;

	public GameObject effect;
	
	private Action<EnemyBullet> returnAction;
	
	public void Initialize(Action<EnemyBullet> returnAction)
	{
		this.returnAction = returnAction;
	}
	
	public void ReturnToPool()
	{
		this.returnAction?.Invoke(this);
	}
	

	private void OnEnable()
	{
		targetPosition = CacheDataManager.Instance.player.transform.position;

	}
	private void Update()
	{
		if (GameManager.Instance.GameState == GameState.Paused) return;
		if (Vector3.Distance(transform.position, targetPosition) < .1f)
		{
			Instantiate(effect, transform.position, Quaternion.identity);
			ReturnToPool();
			gameObject.SetActive(false);
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			CacheDataManager.Instance.player.TakeDamage(damage);
			HitTextManager.Instance.ShowDamageText(damage, transform.position);
			ReturnToPool();
			gameObject.SetActive(false);

		}
	}
}
