using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralInfusion : Effect
{
    private void Awake()
    {
        ActivateEffect();
    }

    public override void ActivateEffect()
    {
        float maxHealth = CacheDataManager.Instance.player.GetStat(StatType.Health);
        CacheDataManager.Instance.player.Heal(maxHealth);
        Destroy(this.gameObject,1f);
    }

    public override void DeactivateEffect()
    {
        
    }
}
