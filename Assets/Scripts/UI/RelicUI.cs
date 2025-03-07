using CoduckStudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Diagnostics;
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

    public void AppearsFromPos(Vector2 pos)
    {
        image.color = new Color(1, 1, 1, 0);

        GameObject go = new GameObject("RelicUI Appearing Animation");
        go.transform.SetParent(transform, true);
        Image goImage = go.AddComponent<Image>();
        go.transform.position = new Vector3(pos.x, pos.y - 300, 0); // Weird fix, couldn't find good position so I hardcoded it
        goImage.sprite = image.sprite;
        goImage.rectTransform.sizeDelta = new Vector2(200, 200);
        goImage.preserveAspect = true;

        RectTransform goRectTransform = go.GetComponent<RectTransform>();
        goRectTransform.anchorMin = new Vector2(0, 1);
        goRectTransform.anchorMax = new Vector2(0, 1);
        goRectTransform.pivot = new Vector2(0.5f, 0.5f);

        CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
            goRectTransform.DOJumpAnchorPos(rectTransform.anchoredPosition, 200, 1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
                image.color = Color.white;
                Destroy(go);
            });
        });
    }
}
