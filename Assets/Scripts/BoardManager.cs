using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : Singleton<BoardManager>
{
    public SlotContainer slotContainer;
    public List<CardSlot> cardSlots;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        slotContainer.onCardSlotsChanged += OnCardSlotsChanged;
    }

    private void OnDisable()
    {
        slotContainer.onCardSlotsChanged -= OnCardSlotsChanged;
    }

    private void OnCardSlotsChanged(List<CardSlot> cardSlots)
    {
        this.cardSlots = cardSlots;
        UpdateCardsYAxis();
    }

    private void UpdateCardsYAxis()
    {
        foreach (var cardSlot in cardSlots.Where((cardSlot) => !cardSlot.isEmpty)) {
            cardSlot.card.actionContainer.startTransform.localPosition = new Vector3(-1.5f, cardSlot.card.cardDefinition.groundStartY, 0);
            cardSlot.card.actionContainer.endTransform.localPosition = new Vector3(1.5f, cardSlot.card.cardDefinition.groundEndY, 0);
        }

        for (int cardIdx = 0; cardIdx < cardSlots.Count; cardIdx++)
        {
            if (cardSlots[cardIdx] != null)
            {
                if (!cardSlots[cardIdx].isEmpty) {
                    Vector3 pos = cardSlots[cardIdx].transform.position;
                    pos.y = GetCardY(cardSlots[cardIdx].card, cardIdx > 0 ? cardSlots[cardIdx - 1].card : null);
                    cardSlots[cardIdx].transform.position = pos;
                }
                else {
                    cardSlots[cardIdx].transform.position = new Vector3(cardSlots[cardIdx].transform.position.x, slotContainer.transform.position.y, cardSlots[cardIdx].transform.position.z);
                }
            }
        }

        // Hack to re-put relative to parent (could'nt find a better way)
        for (int cardIdx = 0; cardIdx < cardSlots.Count; cardIdx++) {
            if (!cardSlots[cardIdx].isEmpty) {
                Vector3 pos = cardSlots[cardIdx].transform.position;
                pos.y += slotContainer.transform.position.y;
                cardSlots[cardIdx].transform.position = pos;
            }
        }
    }

    public bool CanAddCard() => slotContainer.CanAddCard();

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
