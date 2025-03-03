using System;
using UnityEngine;

public class EnergyPointManager : Singleton<StylePointManager>
{
    public int currentPoints { get; private set; } = 0;
    public int initValue = 0;
    public event Action onPointsChanged;

    void Start()
    {
        SetValue(initValue);
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
