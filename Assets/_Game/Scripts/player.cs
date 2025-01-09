using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using TMPro;
using System;
public class Player : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] private float speed;
	[SerializeField] private float maxHealth;
	[SerializeField] private float armor;
	
	[Header("Container")]
	[SerializeField] private Transform weaponHolder;

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 movementDir;

	[Header("Exp UI")]
	[SerializeField] private Image expBar;
	private float currentHealth;

	private SceneTransitions sceneTransitions;
	
	[Header("Joystick")]
	[SerializeField] private FixedJoystick joystick;

	private int currentExp = 0;
	private int currentRequiredExp = 50;
	private int level = 1;


	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sceneTransitions = FindObjectOfType<SceneTransitions>();
		currentHealth = maxHealth;

		GlobalEvent<HealthData>.Trigger("PlayerHealthChange", new HealthData{currentHealth = currentHealth, maxHealth = maxHealth, isHealing = false});
	}	

	// Update is called once per frame
	void Update()
	{
		UpdateDir();
		
		UpdateAnim();
	}

	private void UpdateDir()
	{
		movementDir = joystick.Direction;
	}

	private void UpdateAnim()
	{
		anim.SetFloat("Horizontal",movementDir.x);
		anim.SetFloat("Vertical", movementDir.y);
		anim.SetFloat("Speed", movementDir.sqrMagnitude);
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movementDir * (speed * Time.deltaTime));
	}

	public void TakeDamage(float damageAmount)
	{
		float postMitigationDamage = damageAmount * ( 100f / (100 + armor));
		currentHealth -= damageAmount;
		GlobalEvent<HealthData>.Trigger("PlayerHealthChange", new HealthData{currentHealth = currentHealth, maxHealth = maxHealth, isHealing = false});
		if (currentHealth <= 0)
		{
			Destroy(this.gameObject);
			sceneTransitions.LoadScene("Lose");
		}
	}

	public void ChangWeapon(weapons weaponToEquip)
	{
		Destroy(GameObject.FindGameObjectWithTag("Weapon"));
		Instantiate(weaponToEquip, weaponHolder.position, Quaternion.identity, weaponHolder);
	}
	

	public void Heal(float healAmount)
	{
		if( currentHealth + healAmount > maxHealth)
		{
			currentHealth = maxHealth;
			
		}
		else
		{
			currentHealth += healAmount;
		}
		GlobalEvent<HealthData>.Trigger("PlayerHealthChange", new HealthData{currentHealth = currentHealth, maxHealth = maxHealth, isHealing = true});

	}
	
	public void AddExp(int expAmount)
    {
        currentExp += expAmount;

        while (currentExp >= currentRequiredExp)
        {
            currentExp -= currentRequiredExp;
            currentRequiredExp *= 2;
            level++;
            // Pick specialization
        }
		GlobalEvent<ExpData>.Trigger("PlayerExpChanged", new ExpData{currentExp = currentExp, currentRequiredExp = currentRequiredExp});
    }
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.gameObject.layer == 10)
		{ 
			ExpPickUp exp = other.GetComponent<ExpPickUp>();
			other.enabled = false;
			LMotion.Create(0f, .75f, 0.5f)
				.WithOnComplete(() =>
				{
					AddExp(exp.GetExpAmount());
					Destroy(exp.gameObject);
				})
				.Bind(other,
					(x, other) =>
						other.transform.position = Vector3.Lerp(other.transform.position, transform.position, x));
		}
	}
}
