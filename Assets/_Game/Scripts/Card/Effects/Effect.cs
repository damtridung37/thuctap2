using UnityEngine;

public class Effect : BaseCard
{
    public virtual void ActivateEffect()
    {
        Debug.Log("Effect Activated");
    }
    
    public virtual void DeactivateEffect()
    {
        Debug.Log("Effect Deactivated");
    }
}
