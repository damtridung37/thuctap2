using UnityEngine;

public class Ranged : Enemy
{
    public float stopDistance;

	private float attackTime;

	private Animator anim;

	public Transform shotPoint;

	public override void Start()
	{
		base.Start();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (GameManager.Instance.GameState == GameState.Paused) return;
		if (player != null)
		{
			if (Vector2.Distance(transform.position,player.position) > stopDistance)
			{
				transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
			}
			if (player.position.x < transform.position.x)
			{
				// Người chơi bên trái -> Quái vật quay trái (lật trục X)
				transform.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				// Người chơi bên phải -> Quái vật quay phải (bình thường)
				transform.localScale = new Vector3(1, 1, 1);
			}
			if (Time.time >= attackTime)
			{
				attackTime = Time.time + timeBetweenAttacks;
				anim.SetTrigger("attack");
			}

		}
	}
	public void RangedAttack()
	{
		if(player == null) return;
		Vector2 direction = player.position - shotPoint.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

		shotPoint.rotation = rotation;

		EnemyBullet temp = CacheDataManager.Instance.enemyBulletPool.Pull(shotPoint.position,shotPoint.rotation);
	}
}
