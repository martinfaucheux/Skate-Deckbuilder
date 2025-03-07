using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool overrideScale = false;
    public float scale = 1.5f;
    public Selectable selectable;

    public void OnSelect(BaseEventData eventData)
    {
        Scale();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Unscale();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Scale();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unscale();
    }

    public void Scale()
    {
        if (selectable != null && !selectable.interactable)
            return;

        DOTween.Kill(transform);
        transform.DOScale(overrideScale ? scale : 1.5f, 0.2f);
    }

    public void Unscale()
    {
        DOTween.Kill(transform);
        transform.DOScale(1f, 0.2f);
    }
}
