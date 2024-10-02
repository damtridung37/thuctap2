using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
	public float speed;
	public float lifeTime;

	public GameObject explosion;

	private Vector2 dir = Vector2.up;

	public int damage;
	// Start is called before the first frame update
	void Start()
	{
		Invoke("DestroyProjectile", lifeTime);
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(dir * speed * Time.deltaTime);
	}
	void DestroyProjectile()
	{
		Instantiate(explosion,transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			collision.GetComponent<Enemy>().TakeDamage(damage);
			DestroyProjectile();
		}
	}
	public void Init(float angle  )
	{
		Quaternion rotation = Quaternion.Euler(0, 0, angle);
		dir = rotation * Vector2.up;
		dir = dir.normalized;

	}
}
