using CoduckStudio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RelicDefinition _relicDefinition;
    public RelicDefinition relicDefinition {
        get { return _relicDefinition; }
        set {
            _relicDefinition = value;

            image.sprite = _relicDefinition.sprite;
        }
    }

    public Image image;
    public RectTransform rectTransform;

    private void Awake()
    {
        if (_relicDefinition != null) {
            relicDefinition = _relicDefinition;
        }

        rectTransform.sizeDelta = new Vector2(100, 100);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GenericTooltip.Instance.Show(Tooltip.GetRelicConfig(relicDefinition), gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GenericTooltip.Instance.Hide();
    }
}
