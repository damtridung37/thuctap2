using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossProjectile : MonoBehaviour
{
	public float speed;
	public float lifeTime;

	public GameObject explosion;
	public GameObject soundObject;

	private Vector2 moveDirection = Vector2.up; // Hướng di chuyển mặc định
	private Vector2 dir = Vector2.up;

	public int damage;
	// Start is called before the first frame update
	void Start()
	{
		Invoke("DestroyProjectile", lifeTime);
		Instantiate(soundObject, transform.position, transform.rotation);
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.Instance.GameState == GameState.Paused) return;

		transform.Translate(dir * speed * Time.deltaTime);
	}
	void DestroyProjectile()
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<Player>().TakeDamage(damage);
			DestroyProjectile();
		}
	}
	public void Init(float angle)
	{
		// Chuyển đổi góc từ độ sang vector
		float angleRad = angle * Mathf.Deg2Rad;
		moveDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

		// Xoay viên đạn để nó trông như đang bắn đúng hướng
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
