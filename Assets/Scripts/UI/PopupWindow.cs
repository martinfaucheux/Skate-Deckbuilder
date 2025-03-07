using System;
using DG.Tweening;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    private Action callback;
    private bool isOpen = false;

    private void Awake()
    {
        Hide(true);
    }

    public void Show(bool instant = false, Action callback = null)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Vector2 targetScale = new Vector2(1, 1);

        if (instant) {
            rectTransform.localScale = targetScale;
            return;
        }

        rectTransform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => {
            this.callback = callback;
            isOpen = true;
        });
    }

    public void Hide(bool instant = false)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Vector2 targetScale = new Vector2(0, 0);

        if (instant) {
            rectTransform.localScale = targetScale;
            return;
        }

        rectTransform.DOScale(targetScale, 0.5f).SetEase(Ease.InQuad).OnComplete(() => {
            callback?.Invoke();
            callback = null;
            isOpen = false;
        });
    }

    public void OnClick_Show()
    {
        Show();
    }

    public void OnClick_Close()
    {
        Hide();
    }

    public void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape)) {
            Hide();
        }
    }
}
