using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;

    public CardDefinition _cardDefinition;
    public CardDefinition cardDefinition {
        get { return _cardDefinition; }
        set {
            _cardDefinition = value;

            if (_cardDefinition == null) {
                if (cardVisual != null) {
                    Destroy(cardVisual.gameObject);
                    cardVisual = null;
                }
                return;
            }

            if (cardVisual == null) {
                CreateVisual();
            }

            cardVisual.Set(this);

            AssignActionContainer(Instantiate(cardDefinition.actionContainerPrefab, transform));
        }
    }

    public void AssignActionContainer(ActionContainer actionContainer)
    {
        if (this.actionContainer.gameObject != null) {
            Destroy(this.actionContainer.gameObject);
            this.actionContainer = null;
        }

        this.actionContainer = actionContainer;
        actionContainer.transform.SetParent(transform);
        actionContainer.transform.localPosition = Vector3.zero;

        if (CardTypeConfiguration.i != null)
            SetColor(CardTypeConfiguration.i.TypeToColor(actionContainer.cardType));

        HideAction();
    }

    public void ShowAction()
    {
        foreach (var canvasGroup in actionContainer.GetComponentsInChildren<CanvasGroup>(true)) {
            canvasGroup.alpha = 1;
        }

        foreach (var sr in actionContainer.GetComponentsInChildren<SpriteRenderer>(true)) {
            sr.gameObject.SetActive(true);
        }
    }

    public void HideAction()
    {
        foreach (var canvasGroup in actionContainer.GetComponentsInChildren<CanvasGroup>(true)) {
            canvasGroup.alpha = 0;
        }

        foreach (var sr in actionContainer.GetComponentsInChildren<SpriteRenderer>(true)) {
            sr.gameObject.SetActive(false);
        }
    }

    private void SetColor(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
            spriteRenderer.color = color;
    }

#region Visual
    public CardVisual cardVisualPrefab;
    public CardVisual cardVisual;
    public CardSlot currentSlot;

    public void CreateVisual()
    {
        cardVisual = Instantiate(cardVisualPrefab, transform.position, Quaternion.identity);
        cardVisual.transform.SetParent(GameObject.Find("CardVisuals").transform);
        renderers = new List<SpriteRenderer>(){ cardVisual.cardSpriteRenderer, cardVisual.backgroundSpriteRenderer }.ToArray();
    }
#endregion
}
