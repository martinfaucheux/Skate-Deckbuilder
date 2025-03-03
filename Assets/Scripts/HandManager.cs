using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandManager : Singleton<HandManager>
{
    public Transform cardsHolder;
    public float cardSpacing;

    public int startHandCount = 5;
    public GameObject cardPrefab;

    public List<Card> cards { get; private set; } = new List<Card>();

    protected override void Awake()
    {
        base.Awake();
        // clean existing cards
        foreach (Transform child in cardsHolder.Cast<Transform>().ToList())
        {
            Destroy(child.gameObject);
        }
    }

    void Start()
    {


    }

    public void AddManyCards(List<Card> cards)
    {
        this.cards = cards.ToList();
        foreach (Card card in cards)
        {
            card.transform.SetParent(cardsHolder);
        }
        UpdateCardsPosition();

    }

    public bool MoveCardToHand(Card card)
    {
        if (cards.Contains(card) || !BoardManager.i.cards.Contains(card))
            return false;

        cards.Add(card);
        card.transform.SetParent(cardsHolder);
        UpdateCardsPosition();
        BoardManager.i.RemoveCard(card);
        return true;
    }

    public bool TryMoveCardToBoard(Card card)
    {
        if (
            BoardManager.i.cards.Contains(card)
            || !cards.Contains(card)
            || !BoardManager.i.CanAddCard()
        )
            return false;

        cards.Remove(card);
        BoardManager.i.PlaceCard(card);
        UpdateCardsPosition();
        return true;
    }

    private void UpdateCardsPosition()
    {
        int cardCount = cards.Count;
        float offset = -(cardCount - 1) * cardSpacing / 2;
        for (int cardIdx = 0; cardIdx < cardCount; cardIdx++)
        {
            Card card = cards[cardIdx];
            Vector3 cardPosition = transform.position + new Vector3(offset + cardIdx * cardSpacing, 0, 0);
            card.transform.position = cardPosition;
        }
    }

}
