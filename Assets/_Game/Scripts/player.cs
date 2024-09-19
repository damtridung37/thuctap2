using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
	[SerializeField] private float speed;
	private Rigidbody2D rb;
	private Animator anim;
	Vector2 movement;
	public float health;
	


	// Start is called before the first frame update
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
		
		anim.SetFloat("Horizontal",movement.x);
		anim.SetFloat("Vertical", movement.y);
		anim.SetFloat("Speed", movement.sqrMagnitude);
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
	}

	public void TakeDamage(int damageAmount)
	{
		health -= damageAmount;
		if (health <= 0)
		{
			Destroy(gameObject);

		}
	}
}
