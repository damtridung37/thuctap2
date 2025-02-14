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
        protected float currentHealth;
        protected Animator anim;
        protected Dictionary<StatType, StatBuffData> statBuffs;

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
        }
        
        protected virtual void InitEvent()
        {
            Debug.Log("Base Init Event");
        }

        public virtual void TakeDamage(float damageAmount)
        {
            
        }
    }
}

