using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    [SerializeField] private Image expBar;

    private void OnEnable()
    {
        GlobalEvent<ExpData>.Subscribe("PlayerExpChanged", OnExpChanged);
    }

    private void OnDisable()
    {
        GlobalEvent<ExpData>.Unsubscribe("PlayerExpChanged", OnExpChanged);
    }

    public void OnExpChanged(ExpData data)
    {
        expBar.fillAmount = data.GetExpFillAmount();
    }
}

public struct ExpData
{
    public int currentExp;
    public int currentRequiredExp;

    public float GetExpFillAmount()
    {
        return (float)currentExp / currentRequiredExp;
    }
}