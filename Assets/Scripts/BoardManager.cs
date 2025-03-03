using System.Linq;
using UnityEngine;
using System;

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
        card.transform.localPosition = Vector3.zero;
        return true;
    }

    public bool RemoveCard(Card card)
    {
        int cardIdx = Array.IndexOf(cards, card);
        if (cardIdx < 0)
            return false;

        cards[cardIdx] = null;
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

}
