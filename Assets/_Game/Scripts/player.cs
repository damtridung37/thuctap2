using UnityEngine;
using LitMotion;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	[Header("Stats")]
	
	[SerializeField] private StatDictionary statDictionary;
	
	[Header("Container")]
	[SerializeField] private Transform weaponHolder;

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 movementDir;
	
	private float currentHealth;

	private SceneTransitions sceneTransitions;
	
	[Header("Joystick")]
	[SerializeField] private FixedJoystick joystick;
	
	private int level = 1;
	private int currentExp = 0;
	private int currentRequiredExp = 50;

	[Header("Popup UI")] 
	[SerializeField] private GameObject skillSelectionPopup;
	
	private Dictionary<StatType,StatBuffData> statBuffs = new Dictionary<StatType, StatBuffData>();
	
	private void OnReceiveStatBuff((StatType,float,bool) data)
	{
		if(!statBuffs.ContainsKey(data.Item1))
		{
			statBuffs.Add(data.Item1,new StatBuffData(statDictionary[data.Item1]));
		}
			
		if(data.Item3)
		{
			statBuffs[data.Item1].AddRawValue(data.Item2);
		}
		else
		{
			statBuffs[data.Item1].AddPercentageValue(data.Item2);
		}
		
		if(data.Item1 == StatType.Health)
			Heal(0f);
	}
	

	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sceneTransitions = FindObjectOfType<SceneTransitions>();
		
		currentHealth = statDictionary[StatType.Health];

		foreach (var stat in statDictionary)
		{
			statBuffs.Add(stat.Key,new StatBuffData(stat.Value));
		}
		
		InitEvent();
	}
	
	public float GetStat(StatType statType)
	{
		if (statBuffs.TryGetValue(statType,out StatBuffData data))
		{
			return data.GetValue();
		}

		return 0;
	}

	private void InitEvent()
	{
		GlobalEvent<HealthData>.Trigger("PlayerHealthChanged",new HealthData
		{
			currentHealth = currentHealth,
			maxHealth = statBuffs[StatType.Health].GetValue(),
			isHealing = false
		});
		
		GlobalEvent<(StatType,float,bool)>.Subscribe("PlayerStatBuff",OnReceiveStatBuff);
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
		rb.MovePosition(rb.position + movementDir * (statBuffs[StatType.Speed].GetValue() * Time.deltaTime));
		GlobalEvent<bool>.Trigger("PlayerMoveStatusChange",movementDir != Vector2.zero);
	}

	public void TakeDamage(float damageAmount)
	{
		float postMitigationDamage = damageAmount * ( 100f / (100 + statBuffs[StatType.Armor].GetValue()));
		currentHealth -= postMitigationDamage;
		
		GlobalEvent<HealthData>.Trigger("PlayerHealthChanged",new HealthData
		{
			currentHealth = currentHealth,
			maxHealth = statBuffs[StatType.Health].GetValue(),
			isHealing = false
		});
		
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
		if( currentHealth + healAmount > statBuffs[StatType.Health].GetValue())
		{
			currentHealth = statBuffs[StatType.Health].GetValue();
		}
		else
		{
			currentHealth += healAmount;
		}
		
		GlobalEvent<HealthData>.Trigger("PlayerHealthChanged",new HealthData
		{
			currentHealth = currentHealth,
			maxHealth = statBuffs[StatType.Health].GetValue(),
			isHealing = true
		});
	}
	
	public void HealPercentage(float percentage)
	{
		float healAmount = statBuffs[StatType.Health].GetValue() * (percentage / 100);
		Heal(healAmount);
	}

	[ContextMenu("Test Add Exp")]
	public void TestAddExp()
	{
		AddExp(10000);
	}
	
	public void AddExp(int expAmount)
	{
		currentExp += expAmount;
		
		while (currentExp >= currentRequiredExp)
		{
			currentExp -= currentRequiredExp;
			currentRequiredExp *= 2;
			level++;
			// Show popup
			skillSelectionPopup.SetActive(true);
		}
		
		GlobalEvent<ExpData>.Trigger("PlayerExpChanged",new ExpData
		{
			currentExp = currentExp,
			currentRequiredExp = currentRequiredExp
		});
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
					exp.ReturnToPool();
				})
				.Bind(other,
					(x, other) =>
						other.transform.position = Vector3.Lerp(other.transform.position, transform.position, x));
		}
	}
}
