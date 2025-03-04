using System.Linq;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public ActionContainer actionContainer;
    public SpriteRenderer[] renderers;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI energyRewardText;

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
    }

    private void SetColor(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
            spriteRenderer.color = color;
    }
}
