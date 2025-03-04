using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;

    // do stuff when clicked
    void OnMouseDown()
    {
        if (IsInHand())
        {
            HandManager.i.TryMoveCardToBoard(this);
        }
        else if (IsOnBoard())
        {
            HandManager.i.MoveCardToHand(this);
        }
    }

    public bool IsInHand() => HandManager.i.cards.Contains(this);
    public bool IsOnBoard() => BoardManager.i.cards.Contains(this);

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
}
