using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public Color color;
    public bool randomColor;
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

    void Start()
    {
        Color _color = color;
        if (randomColor)
        {
            _color = new Color(Random.value, Random.value, Random.value);
        }
        foreach (SpriteRenderer spriteRenderer in renderers)
            spriteRenderer.color = _color;

    }

    public bool IsInHand() => HandManager.i.cards.Contains(this);
    public bool IsOnBoard() => BoardManager.i.cards.Contains(this);

    public void AssignActionContainer(ActionContainer actionContainer)
    {
        this.actionContainer = actionContainer;
        actionContainer.transform.SetParent(transform);
        actionContainer.transform.localPosition = Vector3.zero;
    }
}
