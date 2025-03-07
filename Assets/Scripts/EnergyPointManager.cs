using System;
using UnityEngine;

public class EnergyPointManager : Singleton<EnergyPointManager>
{
    public int currentPoints { get; private set; } = 0;
    public int initValue = 0;
    public event Action onPointsChanged;

    void Start()
    {
        ResetValue();
    }

    public void ResetValue()
    {
        int addedValue = 0;
        if (RunManager.Instance.HasRelic("Energy Wheel")) {
            addedValue = 2;
        }

        SetValue(initValue + addedValue);
    }

    public void Add(int value)
    {
        SetValue(currentPoints + value);
    }

    private void SetValue(int value)
    {
        currentPoints = Mathf.Max(0, value);
        onPointsChanged?.Invoke();
    }
}
