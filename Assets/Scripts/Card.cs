using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;
    public CardDefinition _cardDefinition;
    public CardDefinition cardDefinition
    {
        get { return _cardDefinition; }
        set
        {
            _cardDefinition = value;

            if (_cardDefinition == null)
            {
                if (cardVisual != null)
                {
                    Destroy(cardVisual.gameObject);
                    cardVisual = null;
                }
                return;
            }

            if (cardVisual == null)
            {
                CreateVisual();
            }

            cardVisual.Set(this);

            if (cardDefinition.actionContainerPrefab != null) {
                AssignActionContainer(Instantiate(cardDefinition.actionContainerPrefab, transform));
            }
        }
    }

    public void AssignActionContainer(ActionContainer actionContainer)
    {
        if (this.actionContainer.gameObject != null)
        {
            Destroy(this.actionContainer.gameObject);
            this.actionContainer = null;
        }

        this.actionContainer = actionContainer;
        actionContainer.transform.SetParent(transform);
        actionContainer.transform.localPosition = Vector3.zero;

        actionContainer.cardType = cardDefinition.cardType;

        if (CardTypeConfiguration.i != null)
            SetColor(CardTypeConfiguration.i.TypeToColor(actionContainer.cardType));

        HideQTE();

        // TODO: ugly code
        this.actionContainer.SetArrowSprite(CardTypeConfiguration.i.TypeToKey(this.actionContainer.cardType));
        cardVisual.AddInfoBottom(actionContainer.arrowSpriteTransform);
        actionContainer.SetCard(this);
    }


    public void ShowQTE()
    {
        actionContainer.ShowQTERenderer();
    }

    public void HideQTE()
    {
        actionContainer.HideQTERenderer();
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
        renderers = new List<SpriteRenderer>() { cardVisual.cardSpriteRenderer }.ToArray();
    }
    #endregion
}
