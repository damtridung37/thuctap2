using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LitMotion;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image subHealthBar;
    
    private MotionHandle healthBarMotionHandle;
    
    private void OnEnable()
    {
        GlobalEvent<HealthData>.Subscribe("PlayerHealthChanged", OnPlayerHealthChanged);
    }
    
    private void OnDisable()
    {
        GlobalEvent<HealthData>.Unsubscribe("PlayerHealthChanged", OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(HealthData data)
    {
        // for (var i = 0; i < hearts.Length; i++)
        // {
        // 	hearts[i].sprite = i<currentHealth ? fullHearts : emptyHearts;
        // }
        float healthBarFillAmount = data.currentHealth / data.maxHealth;
		
        if(!data.isHealing)
        {
            if(healthBarMotionHandle.IsActive()) healthBarMotionHandle.Cancel();
			
            healthBarMotionHandle = LMotion.Create(subHealthBar.fillAmount, healthBarFillAmount, 0.25f)
                .WithEase(Ease.InOutCubic)
                .Bind(this, 
                    (x, player) 
                        => player.subHealthBar.fillAmount = x);
        }
        else
        {
            subHealthBar.fillAmount = healthBarFillAmount;
        }
		
        healthBar.fillAmount = healthBarFillAmount;
        healthText.text = Mathf.FloorToInt(data.currentHealth) + " / " + data.maxHealth;
    }
}

public struct HealthData
{
    public float currentHealth;
    public float maxHealth;
    public bool isHealing;
}
