using DG.Tweening;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        canvasGroup.DOFade(1, 2f);
    }
}
