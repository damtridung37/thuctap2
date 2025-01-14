using UnityEngine;

public class HitTextManager : Singleton<HitTextManager>
{
    [SerializeField] private DamageText damageTextPrefab;
    private ObjectPool<DamageText> damageTextPool;
    
    private void Awake()
    {
        damageTextPool = new ObjectPool<DamageText>(damageTextPrefab, transform, 10);
    }
    
    public void ShowDamageText(float damage, Vector3 position, bool isCritical = false)
    {
        var damageText = damageTextPool.Pull();
        damageText.transform.position = position;
        damageText.SetDamageText(damage,isCritical);
    }
}
