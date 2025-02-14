using System;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private StatDictionary statDictionary;
        
        protected Rigidbody2D rb;
        protected Animator anim;
        
        protected float currentHealth;
        protected float attackTime;
        
        protected Dictionary<StatType, StatBuffData> statBuffs;
        
        public Dictionary<StatType, StatBuffData> StatBuffs => statBuffs;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            InitStats();
            InitEvent();
        }
        
        public virtual void InitStats()
        {
            currentHealth = statDictionary.TryGetValue(StatType.Health, out var health) ? health : 0;
            statBuffs = new Dictionary<StatType, StatBuffData>();
            foreach (var stat in statDictionary)
            {
                statBuffs.Add(stat.Key,new StatBuffData(stat.Value));
            }
            
            attackTime = statDictionary.TryGetValue(StatType.AttackSpeed, out var attackSpeed) ? 1 / attackSpeed : 1;
        }
        
        protected virtual void InitEvent()
        {
            Debug.Log("Base Init Event");
        }

        public virtual void TakeDamage(float damageAmount)
        {
            
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void Attack()
        {
            
        }

        protected virtual void Move()
        {
            
        }
    }
}

