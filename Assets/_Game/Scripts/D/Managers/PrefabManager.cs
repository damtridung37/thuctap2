using System;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Health Drop")]
    [SerializeField]
    private GameObject healthPickUp;
    public void SpawnHealthPickUp(Vector3 position)
    {
        Instantiate(healthPickUp, position, Quaternion.identity);
    }
    
    [Header("Exp Drop")]
    [SerializeField]
    private ExpPickUp expPickUp;
    private ObjectPool<ExpPickUp> expPickUpPool;
    private float expPickUpChanceWeight = 1 + 4 + 15 + 30 + 50;
    // rate increase the chance of getting higher level exp pick up
    public ExpPickUp SpawnExpPickUp(Vector3 position, float rate = 1f)
    {
        var expPickUp = expPickUpPool.Pull(position);
        float randomNumber = UnityEngine.Random.Range(0f, expPickUpChanceWeight) / rate;
        if (randomNumber <= 1)
        {
            expPickUp.SetExpLevel(5);
        }
        else if (randomNumber <= 5)
        {
            expPickUp.SetExpLevel(4);
        }
        else if (randomNumber <= 20)
        {
            expPickUp.SetExpLevel(3);
        }
        else if (randomNumber <= 50)
        {
            expPickUp.SetExpLevel(2);
        }
        else
        {
            expPickUp.SetExpLevel(1);
        }
        return expPickUpPool.Pull(position);
    }
    
    [Header("Death Effect")]
    [SerializeField]
    private GameObject deathEffect;
    public void SpawnDeathEffect(Vector3 position)
    {
        Instantiate(deathEffect, position, Quaternion.identity);
    }
    
    [Header("Damage Text")]
    [SerializeField] private DamageText damageTextPrefab;
    private ObjectPool<DamageText> damageTextPool;
    
    public void ShowDamageText(float damage, Vector3 position, bool isCritical = false)
    {
        var damageText = damageTextPool.Pull();
        damageText.transform.position = position;
        damageText.SetDamageText(damage,isCritical);
    }

    private void Awake()
    {
        damageTextPool = new ObjectPool<DamageText>(damageTextPrefab, new GameObject("Damage_Text_Holder").transform, 10);
        expPickUpPool = new ObjectPool<ExpPickUp>(expPickUp, new GameObject("Exp_Holder").transform, 10);
    }
}
