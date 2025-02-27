using LitMotion;
using System.Linq;
using UnityEngine;

namespace D
{
    public class Player : Character
    {
        [Header("Weapon")]
        private Weapon weapon;
        //Flags
        private bool isLeft;
        private bool isRight;
        private bool isUp;
        private bool isDown;
        private bool isDead;

        public bool IsDead { get => isDead; set => isDead = value; }

        private BoxCollider2D boxCollider;

        //Constants
        private const string IDLE = "Idle";
        private const string IDLE_LEFT = "IdleLeft";
        private const string IDLE_RIGHT = "IdleRight";
        private const string IDLE_TOP = "IdleTop";
        private const string WALK = "Walk";
        private const string WALK_LEFT = "WalkLeft";
        private const string WALK_RIGHT = "WalkRight";
        private const string WALK_TOP = "WalkUP";
        private const string DIE = "Die";

        private Vector2 lastDirection;
        private Vector2 currentDirection;

        private int currentLevel = 1;
        private int currentExp;
        private int requiredExp;

        public static Player Instance { get; private set; }

        public void ColliderEnable(bool enable)
        {
            boxCollider.enabled = enable;
        }

        protected override void Awake()
        {
            weapon = GetComponentInChildren<Bow>();
            if (Instance == null)
                Instance = this;
            boxCollider = GetComponent<BoxCollider2D>();
            base.Awake();
        }

        protected override void InitEvent()
        {
            base.InitEvent();
        }

        public override void InitStats()
        {
            ReCalculateStats();
            isDead = false;
            CalculateRequiredExp();
            if (currentHealth <= 0)
            {
                currentHealth = statBuffs[StatType.Health].GetValue();
                Heal(currentHealth);
            }
            Heal(0);
            AddExp(0);
        }

        public void ReCalculateStats()
        {
            StaticConfig staticConfig = GameManager.Instance.staticConfig;
            PlayerData playerData = GameManager.Instance.playerData;
            statDictionary = new StatDictionary();
            foreach (var pair in staticConfig.playerStats)
            {
                statDictionary.Add(pair.Key, pair.Value);
            }
            foreach (var stat in playerData.PlayerBonusStats)
            {
                if (statDictionary.ContainsKey(stat.Key) && staticConfig.scale.ContainsKey(stat.Key))
                    statDictionary[stat.Key] += stat.Value * staticConfig.scale[stat.Key];
            }
            base.InitStats();
            currentLevel = playerData.CurrentLevel;
            currentExp = playerData.CurrentExp;
            currentHealth = playerData.CurrentHealth;
            Heal(0);
        }

        private void PC_Input()
        {
            if (isDead) return;
            isLeft = Input.GetKey(KeyCode.A);
            isRight = Input.GetKey(KeyCode.D);
            isUp = Input.GetKey(KeyCode.W);
            isDown = Input.GetKey(KeyCode.S);

            if (currentDirection != Vector2.zero)
                lastDirection = currentDirection;

            currentDirection = new Vector2Int(isRight ? 1 : isLeft ? -1 : 0, isUp ? 1 : isDown ? -1 : 0);
            currentDirection.Normalize();
        }

        private void AnimationControl()
        {
            if (isDead)
            {
                anim.Play(DIE);
                return;
            }

            if (currentDirection == Vector2.zero)
            {
                if (lastDirection.x > 0)
                    anim.Play(IDLE_RIGHT);
                else if (lastDirection.x < 0)
                    anim.Play(IDLE_LEFT);
                else if (lastDirection.y > 0)
                    anim.Play(IDLE_TOP);
                else if (lastDirection.y < 0)
                    anim.Play(IDLE);
                return;
            }

            if (currentDirection.x > 0)
            {
                anim.Play(WALK_RIGHT);
            }
            else if (currentDirection.x < 0)
            {
                anim.Play(WALK_LEFT);
            }
            else if (currentDirection.y > 0)
            {
                anim.Play(WALK_TOP);
            }
            else if (currentDirection.y < 0)
            {
                anim.Play(WALK);
            }
        }

        private void Weapon_LookAtMouse(Vector3? target)
        {
            Vector3 mousePos = target.HasValue ? target.Value : Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(mousePos.x - weapon.transform.position.x, mousePos.y - weapon.transform.position.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            //weapon.transform.rotation = rotation;
            weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, rotation, .5f);
            // Move around the player
            weapon.transform.localPosition = new Vector3(direction.x, direction.y, 0).normalized * 0.1f;
            weapon.transform.localPosition += new Vector3(0, 0.5f / transform.localScale.y, 0);
        }

        private void Update()
        {
#if UNITY_EDITOR
            PC_Input();
#endif
            AnimationControl();

        }

        protected override void FixedUpdate()
        {
            //if (GameManager.Instance.GameState == GameState.Paused) return;
            if (isDead) return;

            attackTime -= Time.fixedDeltaTime;
            if (attackTime < 0) attackTime = 0;

            Move();

            Enemy enemy = PrefabManager.Instance?.GetClosestEnemy(transform.position);
            if (enemy != null)
            {
                weapon.gameObject.SetActive(true);
                Weapon_LookAtMouse(enemy.transform.position);
                Attack();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }

        protected override void Attack()
        {
            if (attackTime <= 0)
            {
                weapon.Attack(this);
                attackTime += 1 / statBuffs[StatType.AttackSpeed].GetValue();
            }
        }

        protected override void Move()
        {
            rb.MovePosition(rb.position + currentDirection * (statBuffs[StatType.Speed].GetValue() * Time.fixedDeltaTime));
        }

        public override void TakeDamage(float damageAmount)
        {
            if (isDead) return;
            float postMitigationDamage = damageAmount * (100f / (100 + statBuffs[StatType.Armor].GetValue()));
            currentHealth -= postMitigationDamage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            GlobalEvent<HealthData>.Trigger("PlayerHealthChanged", new HealthData
            {
                currentHealth = currentHealth,
                maxHealth = statBuffs[StatType.Health].GetValue(),
                isHealing = false
            });

            Debug.Log("Player Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                isDead = true;
                Invoke(nameof(OnCharacterDead), 1f);
            }
        }

        private void OnCharacterDead()
        {
            GlobalEvent<bool>.Trigger("OnPlayerDead", true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.gameObject.layer == 10)
            {
                ExpPickUp exp = other.GetComponent<ExpPickUp>();
                if (exp == null) return;
                exp.boxCollider.enabled = false;
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

        public void AddExp(int expAmount)
        {
            currentExp += expAmount;

            while (currentExp >= requiredExp)
            {
                currentExp -= requiredExp;
                CalculateRequiredExp();
                GlobalEvent<(int, bool)>.Trigger("On_PlayerStatPointChanged", (GameManager.Instance.staticConfig.STAT_POINTS_PER_LEVEL, true));
                currentLevel++;
            }

            GlobalEvent<ExpData>.Trigger("PlayerExpChanged", new ExpData
            {
                currentExp = currentExp,
                currentRequiredExp = requiredExp,
                currentLevel = currentLevel
            });
        }

        public void CalculateRequiredExp()
        {
            requiredExp = Mathf.FloorToInt(20 * Mathf.Pow(currentLevel, 2f) * Mathf.Log10(currentLevel + 5));
        }

        public void Heal(float healAmount)
        {
            if (currentHealth + healAmount > statBuffs[StatType.Health].GetValue())
            {
                currentHealth = statBuffs[StatType.Health].GetValue();
            }
            else
            {
                currentHealth += healAmount;
            }

            GlobalEvent<HealthData>.Trigger("PlayerHealthChanged", new HealthData
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
    }
}
