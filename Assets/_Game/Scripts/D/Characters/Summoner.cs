using UnityEngine;

namespace D
{
    public class Summoner : Enemy
    {
        [Header("Summon settings")]
        [SerializeField] private float timeBetweenSummons;
        private float summonTime;
        [SerializeField] private int quantityPerSummon = 1;

        protected override void Awake()
        {
            summonTime = timeBetweenSummons;
            base.Awake();
        }

        protected override void FixedUpdate()
        {
            if(player.IsDead) return;
            
            attackTime -= Time.fixedDeltaTime;
            if (attackTime < 0) attackTime = 0;
            
            summonTime -= Time.fixedDeltaTime;
            if (summonTime < 0) summonTime = 0;

            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            
            Move();
            Flip();
            Render();
            
            this.Attack();
        }

        protected override void Move()
        {
            if (distanceToPlayer > stopDistance)
            {
                base.Move();
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning",false);
            }
        }

        protected override void Attack()
        {
            if(distanceToPlayer <= .5f)
            {
                base.Attack();
            }
            else
            {
                anim.SetBool("isRunning", false);
                if (summonTime<= 0)
                {
                    summonTime += timeBetweenSummons;
                    anim.SetTrigger("summon");
                }
            }
        }
        
        private void Summon()
        {
            PrefabManager.Instance.SpawnEnemy(EnemyType.SkeletonMinion,transform.position, quantityPerSummon);
        }
    }
}

