using UnityEngine;

public class projectile : MonoBehaviour
{
	public float speed;
	public float lifeTime;

	public GameObject explosion;

	public GameObject soundObject;

	private Vector2 dir = Vector2.up;
	
	public float scaleDamage;
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
		Instantiate(explosion,transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			(float finalDamage, bool isCrit) = CalculateDamage();
			CacheDataManager.Instance.GetEnemy(collision).TakeDamage(finalDamage);
			HitTextManager.Instance.ShowDamageText(finalDamage, transform.position,isCrit);
			DestroyProjectile();
		}

		if (collision.tag == "Boss")
		{
			(float finalDamage, bool isCrit) = CalculateDamage();
			collision.GetComponent<Boss>().TakeDamage(finalDamage);
			HitTextManager.Instance.ShowDamageText(finalDamage, transform.position,isCrit);
			DestroyProjectile();
		}
	}
	public void Init(float angle  )
	{
		Quaternion rotation = Quaternion.Euler(0, 0, angle);
		dir = rotation * Vector2.up;
		dir = dir.normalized;
	}
	
	private (float,bool) CalculateDamage()
	{
		Player player = CacheDataManager.Instance.player;
		float playerDamage = player.GetStat(StatType.Damage);
		float playerCritChance = player.GetStat(StatType.CritChance);
		float playerCritDamage = player.GetStat(StatType.CritDamage);
		
		bool isCrit = Random.Range(0, 101) <= playerCritChance;
		float finalDamage = playerDamage * scaleDamage;
		if (isCrit)
		{
			finalDamage += finalDamage * (playerCritDamage / 100);
		}
		
		return (finalDamage,isCrit);
	}
}
