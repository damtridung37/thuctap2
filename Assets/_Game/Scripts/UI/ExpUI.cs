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

    private void OnExpChanged(ExpData data)
    {
        expBar.fillAmount = data.GetExpFillAmount();
    }
}

public struct ExpData
{
    public int currentExp;
    public int currentRequiredExp;
    public int currentLevel;

    public float GetExpFillAmount()
    {
        return (float)currentExp / currentRequiredExp;
    }
}
