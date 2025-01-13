public class HealthIsWealth : Effect
{
    private void Awake()
    {
        ActivateEffect();
    }

    public override void ActivateEffect()
    {
        GlobalEvent<(StatType,float,bool)>.Trigger("PlayerStatBuff", (StatType.Health, 1000f, true));
    }

    public override void DeactivateEffect()
    {
        
    }
}
