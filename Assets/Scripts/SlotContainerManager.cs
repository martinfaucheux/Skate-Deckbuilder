using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotContainerManager : Singleton<SlotContainerManager>
{
    private List<SlotContainer> slotContainers = new List<SlotContainer>();
    protected override void Awake()
    {
        base.Awake();

        slotContainers = FindObjectsByType<SlotContainer>(FindObjectsSortMode.None).ToList();
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        SlotContainer closestSlotContainer = null;
        float closestDistance = float.MaxValue;

        foreach (var slotContainer in slotContainers) {
            float distance = Vector3.Distance(mousePosition, slotContainer.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestSlotContainer = slotContainer;
            }
        }
        
        foreach (var slotContainer in slotContainers)
        {
            slotContainer.isHovering = slotContainer == closestSlotContainer;
        }
    }
}
