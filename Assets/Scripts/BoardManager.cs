using System.Linq;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class BoardManager : Singleton<BoardManager>
{
    public int cardCount = 5;
    public GameObject cardHolderPrefab;
    public float cardSpacing = 1.5f;
    public Card[] cards;
    public Transform[] cardHolders;

    protected override void Awake()
    {
        base.Awake();
        cards = new Card[cardCount];
        cardHolders = new Transform[cardCount];
    }

    void Start()
    {
        foreach (Transform child in transform.Cast<Transform>().ToList())
        {
            Destroy(child.gameObject);
        }

        float offset = -(cardCount - 1) * cardSpacing / 2;
        for (int cardHolderIdx = 0; cardHolderIdx < cardCount; cardHolderIdx++)
        {
            Vector3 position = transform.position + new Vector3(offset + cardHolderIdx * cardSpacing, 0, 0);
            GameObject cardHolder = Instantiate(cardHolderPrefab, position, Quaternion.identity, transform);
            cardHolders[cardHolderIdx] = cardHolder.transform;
        }
    }

    public bool PlaceCard(Card card)
    {
        int cardIdx = GetCardIdx();
        if (cardIdx < 0)
        {
            return false;
        }

        cards[cardIdx] = card;
        card.transform.SetParent(cardHolders[cardIdx]);
        Vector3 position = card.transform.parent.position;
        card.transform.position = position;
        UpdateCardsYAxis();
        return true;
    }

    private void UpdateCardsYAxis()
    {
        for (int cardIdx = 0; cardIdx < cardCount; cardIdx++)
        {
            if (cards[cardIdx] != null)
            {
                Vector3 position = cardHolders[cardIdx].position;
                position.y = GetCardY(cards[cardIdx], cardIdx > 0 ? cards[cardIdx - 1] : null);
                cards[cardIdx].transform.position = position;
            }
        }
    }

    public bool RemoveCard(Card card)
    {
        int cardIdx = Array.IndexOf(cards, card);
        if (cardIdx < 0)
            return false;

        cards[cardIdx] = null;
        UpdateCardsYAxis();
        return true;
    }

    private int GetCardIdx()
    {
        for (int cardIdx = 0; cardIdx < cardCount; cardIdx++)
        {
            if (cards[cardIdx] == null)
            {
                return cardIdx;
            }
        }
        return -1;
    }

    public bool CanAddCard() => cards.Any(card => card == null);


    private float GetCardY(Card card, Card previousCard)
    {
        Vector3 localOffset = card.actionContainer.startTransform.localPosition;
        // card actoinContainer start point must be same as previous card actionContainer end point
        if (previousCard == null)
        {
            return -localOffset.y;
        }

        Vector3 previousPosition = previousCard.actionContainer.endTransform.position;
        return (previousPosition - localOffset).y;

    }

}
