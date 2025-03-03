using System.Collections.Generic;
using UnityEngine;

public class ActionPlayer : Singleton<ActionPlayer>
{
    public Transform skaterTransform;

    public void Play()
    {
        if (BoardManager.i.CanAddCard())
            return;

        List<Vector3> path = new List<Vector3>();
        foreach (Card card in BoardManager.i.cards)
        {
            path.Add(card.actionContainer.startTransform.position);
            path.Add(card.actionContainer.endTransform.position);
        }



    }
}