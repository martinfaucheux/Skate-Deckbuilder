using UnityEngine;
using TMPro;

public class EnergyPointDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    private EnergyPointManager manager => EnergyPointManager.i;

    private void Start()
    {
        manager.onPointsChanged += OnValueChanged;
        OnValueChanged();
    }

    private void OnDestroy()
    {
        manager.onPointsChanged -= OnValueChanged;
    }


    private void OnValueChanged()
    {
        text.text = manager.currentPoints.ToString();
    }
}
