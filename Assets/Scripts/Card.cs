using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI energyRewardText;

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

            AssignActionContainer(Instantiate(cardDefinition.actionContainerPrefab, transform));
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

        if (CardTypeConfiguration.i != null)
            SetColor(CardTypeConfiguration.i.TypeToColor(actionContainer.cardType));

        int cost = actionContainer.energyCost;
        int reward = actionContainer.energyGain;
        if (cost > 0)
        {
            energyCostText.text = $"-{cost}";
        }
        else
        {
            if (reward > 0)
                energyRewardText.text = $"+{reward}";
            else
                energyRewardText.text = "";
        }
        energyRewardText.text = "";
        HideQTE();

        // TODO: ugly code
        actionContainer.SetArrowSprite(CardTypeConfiguration.i.TypeToKey(this.actionContainer.cardType));
        cardVisual.AddInfoBottom(actionContainer.arrowSpriteTransform);
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
