using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health;
	public Enemy[] enemies;
	public float spawnOffset;

	private int halfHealth;
	private Animator anim;

	public int damage;

	private Slider healthBar;

	private SceneTransitions sceneTransitions;

	private Vector3 previousPosition;

	private void Start()
	{
		halfHealth = health / 2;
		anim = GetComponent<Animator>();
		previousPosition = transform.position;
		healthBar = FindObjectOfType<Slider>();
		healthBar.maxValue = health;
		healthBar.value = health;
		sceneTransitions = FindObjectOfType<SceneTransitions>();
	}

	private void Update()
	{
		FlipDirection();
		previousPosition = transform.position;
	}

	private void FlipDirection()
	{
		if (transform.position.x > previousPosition.x)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (transform.position.x < previousPosition.x)
		{

			transform.localScale = new Vector3(-1, 1, 1);
		}
	}
	public void TakeDamage(int damageAmount)
	{
		health -= damageAmount;
		healthBar.value = health;
		if (health <= 0)
		{
			Destroy(this.gameObject);
			healthBar.gameObject.SetActive(false);
			sceneTransitions.LoadScene("Win");
		}

		if (health <= halfHealth) 
		{
			anim.SetTrigger("stage2");
		}

		Enemy randomEnemy = enemies[Random.Range(0, enemies.Length)];
		Instantiate(randomEnemy, transform.position + new Vector3(spawnOffset, spawnOffset, 0) , transform.rotation);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<player>().TakeDamage(damage);
		}
	}
}
