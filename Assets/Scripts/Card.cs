using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{

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
        Color color = new Color(Random.value, Random.value, Random.value);
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = color;
        }
    }

    public bool IsInHand() => HandManager.i.cards.Contains(this);
    public bool IsOnBoard() => BoardManager.i.cards.Contains(this);
}
