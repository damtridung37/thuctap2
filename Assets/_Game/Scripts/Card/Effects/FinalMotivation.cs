using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMotivation :Effect
{
    private void Awake()
    {
        ActivateEffect();
    }

    public override void ActivateEffect()
    {
        GlobalEvent<HealthData>.Subscribe("PlayerHealthChanged", OnPlayerHealthChanged);
		
    }   
    private void OnPlayerHealthChanged(HealthData healthData)
    {
        float x = healthData.currentHealth / healthData.maxHealth;
        if (x <= 0.3f)
        {
            //GlobalEvent<(StatType, float, bool)>.Trigger("PlayerStatBuff", (StatType.Health, healthData.maxHealth * 0.1f, true));
            GlobalEvent<(StatType, float, bool)>.Trigger("PlayerStatBuff", (StatType.Damage, 50, true));
            
           Invoke("DeactivateEffect", 5f);
        }
    }

    public override void DeactivateEffect()
    {
        Destroy(this.gameObject, 1f);       
        GlobalEvent<HealthData>.Unsubscribe("PlayerHealthChanged", OnPlayerHealthChanged);
        GlobalEvent<(StatType, float, bool)>.Trigger("PlayerStatBuff", (StatType.Damage, -50, true));
    }
}
