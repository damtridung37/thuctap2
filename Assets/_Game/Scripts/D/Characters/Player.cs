using System;
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
        
        public bool IsDead => isDead;
        
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
        
        public static Player Instance { get; private set; }

        protected override void Awake()
        {
            weapon = GetComponentInChildren<Weapon>();
            if(Instance == null)
                Instance = this;
            base.Awake();
        }

        protected override void InitEvent()
        {
            Debug.Log("Player Init Event");
            base.InitEvent();
        }
        
        private void PC_Input()
        {
            if(isDead) return;
            isLeft = Input.GetKey(KeyCode.A);
            isRight = Input.GetKey(KeyCode.D);
            isUp = Input.GetKey(KeyCode.W);
            isDown = Input.GetKey(KeyCode.S);
            
            if(currentDirection != Vector2.zero)
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
                if(lastDirection.x > 0)
                    anim.Play(IDLE_RIGHT);
                else if(lastDirection.x < 0)
                    anim.Play(IDLE_LEFT);
                else if(lastDirection.y > 0)
                    anim.Play(IDLE_TOP);
                else if(lastDirection.y < 0)
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
        
        private void Weapon_LookAtMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            weapon.transform.rotation = rotation;
            
            // Move around the player
            weapon.transform.localPosition = new Vector3(direction.x, direction.y, 0).normalized * 0.2f;
        }
        
        private void Update()
                {
        #if UNITY_EDITOR
                    PC_Input();
        #endif
                    AnimationControl();
                    
                }
        
        private void FixedUpdate()
        {
            //if (GameManager.Instance.GameState == GameState.Paused) return;

            rb.MovePosition(rb.position + currentDirection * (statBuffs[StatType.Speed].GetValue() * Time.fixedDeltaTime));
            Weapon_LookAtMouse();
        }
        
        public override void TakeDamage(float damageAmount)
        {
            if(isDead) return;
            currentHealth -= damageAmount;
            
            Debug.Log("Player Health: " + currentHealth);
            
            if (currentHealth <= 0)
            {
                isDead = true;
            }
        }
    }
}
