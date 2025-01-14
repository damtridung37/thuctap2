using System;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable<DamageText>
{
    [SerializeField] private TMP_Text damageText;
    private Action<DamageText> returnAction;
    
    public void SetDamageText(float damage, bool isCritical, float duration = 1f)
    {
        damageText.text = Mathf.FloorToInt(damage).ToString();
        damageText.color = isCritical ? Color.red : Color.white;
        Invoke(nameof(ReturnToPool), duration);
    }
    
    public void Initialize(Action<DamageText> returnAction)
    {
        this.returnAction = returnAction;
    }
    
    public void ReturnToPool()
    {
        this.returnAction?.Invoke(this);
    }
}
