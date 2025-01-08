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

	[Header("Health UI")]
	[SerializeField] private TMP_Text healthText;
	[SerializeField] private Image healthBar;
	[SerializeField] private Image subHealthBar;


	[Header("Exp UI")]
	[SerializeField] private Image expBar;
	private float currentHealth;
	private MotionHandle healthBarMotionHandle;

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

		healthBarMotionHandle = LMotion.Create(0, 0, 0).RunWithoutBinding();

		currentHealth = maxHealth;
		UpdateHealthUI(currentHealth,true);
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
		UpdateHealthUI(currentHealth);
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
	
	private void UpdateHealthUI(float currentHealth,bool isHealing = false)
	{
		float healthBarFillAmount = currentHealth / maxHealth;
		
		if(!isHealing)
        {
            if(healthBarMotionHandle.IsActive()) healthBarMotionHandle.Cancel();

            healthBarMotionHandle = LMotion.Create(subHealthBar.fillAmount, healthBarFillAmount, 0.25f)
                .WithEase(Ease.InOutCubic)
                .Bind(this, 
                    (x, player) 
                        => player.subHealthBar.fillAmount = x);
        }
	
	
		// for (var i = 0; i < hearts.Length; i++)
		// {
		// 	hearts[i].sprite = i<currentHealth ? fullHearts : emptyHearts;
		// }

		healthBar.fillAmount = healthBarFillAmount;
		healthText.text = Mathf.FloorToInt(currentHealth) + "/" + maxHealth;
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
		UpdateHealthUI(currentHealth,true);
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
		expBar.fillAmount = (float)currentExp / currentRequiredExp;
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
