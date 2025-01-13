using System.Collections.Generic;
using UnityEngine;

public class StatBuffData
{
    private List<float> rawValues = new List<float>();
    private List<float> percentageValues = new List<float>();
    
    private float baseValue;
    private float thresholdValue;
    private float totalPercentage;
    private float currentCalculatedValue;
    
    public void AddRawValue(float value)
    {
        rawValues.Add(value);
        CalculateValue();
    }
    
    public void AddPercentageValue(float value)
    {
        percentageValues.Add(value);
        CalculateValue();
    }
    
    public void CalculateValue()
    {
        currentCalculatedValue = baseValue;
        foreach (var rawValue in rawValues)
        {
            currentCalculatedValue += rawValue;
        }

        totalPercentage = 0;
        foreach (var percentageValue in percentageValues)
        {
            totalPercentage += percentageValue;
        }
        
        totalPercentage /= 100;
        
        if(thresholdValue >= 0)
            totalPercentage = Mathf.Min(totalPercentage, thresholdValue);
        
        currentCalculatedValue += baseValue * totalPercentage;
    }
    
    public float GetValue()
    {
        return currentCalculatedValue;
    }
    
    public StatBuffData(float initBaseValue, float thresholdValue = -1)
    {
        this.baseValue = initBaseValue;
        this.thresholdValue = thresholdValue;
        CalculateValue();
    }
}
