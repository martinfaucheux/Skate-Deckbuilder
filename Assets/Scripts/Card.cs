using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public Color color;
    public bool randomColor;
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
