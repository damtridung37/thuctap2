using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class player : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private GameObject weaponHolder;

	private Rigidbody2D rb;

	private Animator anim;

	Vector2 movement;

	public int health;

	public Image[] hearts;
	public Sprite fullHearts;
	public Sprite emptyHearts;
	

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
		UpdateHealthUI(health);
		if (health <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	public void ChangWeapon(weapons weaponToEquip)
	{
		Destroy(GameObject.FindGameObjectWithTag("Weapon"));
		Instantiate(weaponToEquip, weaponHolder.transform.position, Quaternion.identity, weaponHolder.transform);
	}
	void UpdateHealthUI(int currentHealth)
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			if (i < currentHealth)
			{
				hearts[i].sprite = fullHearts;
			}
			else
			{
				hearts[i].sprite= emptyHearts;
			}

		}
	}

	public void Heal(int healAmount)
	{
		if( health + healAmount > 5)
		{
			health = 5;
		}
		else
		{
			health += healAmount;
		}
		UpdateHealthUI(health);
	}

}
