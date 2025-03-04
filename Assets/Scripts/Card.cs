using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;

    public CardDefinition _cardDefinition;
    public CardDefinition cardDefinition {
        get { return _cardDefinition; }
        set {
            _cardDefinition = value;

            if (_cardDefinition == null && cardVisual != null) {
                Destroy(cardVisual.gameObject);
                cardVisual = null;
                return;
            }

            if (cardVisual == null) {
                CreateVisual();
            }

            cardVisual.Set(this);
        }
    }

    public void AssignActionContainer(ActionContainer actionContainer)
    {
        this.actionContainer = actionContainer;
        actionContainer.transform.SetParent(transform);
        actionContainer.transform.localPosition = Vector3.zero;

        if (CardTypeConfiguration.i != null)
            SetColor(CardTypeConfiguration.i.TypeToColor(actionContainer.cardType));
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
        cardVisual.transform.localPosition = Vector3.zero;
    }
#endregion
}
