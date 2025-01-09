using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	private Vector2 targetPosition;

	public float speed;
	public int damage;

	public GameObject effect;
	

	private void Start()
	{
		targetPosition = CacheDataManager.Instance.player.transform.position;

	}
	private void Update()
	{
		if (GameManager.Instance.GameState == GameState.Paused) return;
		if ((Vector2)transform.position == targetPosition)
		{
			Instantiate(effect, transform.position, Quaternion.identity);
			Destroy(gameObject);
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
			Destroy(gameObject);
		}
	}
}
