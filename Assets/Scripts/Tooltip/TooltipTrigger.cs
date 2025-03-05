using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoduckStudio
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        public string description;
        public Color textColor;
        public bool disableIfImageNotVisible = true;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (disableIfImageNotVisible && TryGetComponent(out Image image) && (!image.gameObject.activeSelf || !image.enabled || image.color.a == 0)) {
                return;
            }

            GenericTooltip.Config config = new GenericTooltip.Config(title, textColor, Color.white);
            config.descriptions.Add(new GenericTooltip.ConfigDescription(description, textColor));

            GenericTooltip.Instance.Show(config);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GenericTooltip.Instance.Hide();
        }
    }
}
