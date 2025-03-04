using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    private List<CardSlot> cardSlots = new List<CardSlot>();
    public delegate void OnCardSlotsChanged(List<CardSlot> cardSlots);
    public OnCardSlotsChanged onCardSlotsChanged;
    [HideInInspector] public bool isHovering = false;
    
    [Header("References")]
    public CardSlot slotPrefab;
    public Card cardPrefab;

    [Header("Settings")]
    public List<CardDefinition> startingCards = new List<CardDefinition>();
    public int cardCountMax = 5;
    public bool forceCardBigHeight = false;
    public bool allowCardSlotYOffset = false;

    private void Awake()
    {
        cardSlots = GetComponentsInChildren<CardSlot>().ToList();

        AddCards(startingCards);

        int i = 0;
        foreach (var cardSlot in cardSlots) {
            cardSlot.SetSlotContext(i, cardSlots.Count);
            i++;
        }
    }

    private void Update()
    {
        CheckSwap();
    }

    public CardSlot AddCard(CardDefinition card)
    {
        CardSlot slot = Instantiate(slotPrefab, transform);
        slot.card.cardDefinition = card;

        UpdateSlots();

        return slot;
    }

    public List<CardSlot> AddCards(List<CardDefinition> modules)
    {
        RemoveAllModules();

        List<CardSlot> slotsRet = new List<CardSlot>();
        foreach (var module in modules) {
            CardSlot slot = Instantiate(slotPrefab, transform);
            slot.card.cardDefinition = module;
            slotsRet.Add(slot);
        }

        UpdateSlots();

        return slotsRet;
    }

    public void RemoveAllModules()
    {
        foreach (CardSlot child in gameObject.GetComponentsInChildren<CardSlot>()) {
            Destroy(child.gameObject);
        }

        UpdateSlots();
    }

#region DragAndDrop
    [HideInInspector] public CardSlot selectedSlot;
    public void BeginDrag(CardSlot slot)
    {
        selectedSlot = slot;
    }

    public void EndDrag(CardSlot slot)
    {
        if (selectedSlot == null)
            return;

        UpdateSlots();

        selectedSlot = null;
    }

    private void CheckSwap()
    {
        if (selectedSlot == null)
            return;

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (selectedSlot.transform.position.x > cardSlots[i].transform.position.x)
            {
                if (selectedSlot.transform.GetSiblingIndex() < cardSlots[i].transform.GetSiblingIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedSlot.transform.position.x < cardSlots[i].transform.position.x)
            {
                if (selectedSlot.transform.GetSiblingIndex() > cardSlots[i].transform.GetSiblingIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        selectedSlot.transform.SetSiblingIndex(index);
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        cardSlots = GetComponentsInChildren<CardSlot>().ToList();

        int i = 0;
        foreach (var slot in cardSlots) {
            slot.SetSlotContext(i, cardSlots.Count);
            i++;
        }

        onCardSlotsChanged?.Invoke(cardSlots);
    }

    public void SwapSlotContainer(SlotContainer other)
    {
        if (selectedSlot == null || !other.CanAddCard())
            return;

        other.AddSlot(selectedSlot, true);
        EndDrag(selectedSlot);
        UpdateSlots();
    }

    public void AddSlot(CardSlot slot, bool isDragging)
    {
        slot.transform.SetParent(transform);
        slot.currentContainer = this;

        if (isDragging) {
            BeginDrag(slot);
        }

        if (forceCardBigHeight)
            slot.card.cardVisual.SetHeight(CardVisual.Height.Big);

        UpdateSlots();
    }

    public void DeleteSlot(CardSlot slot)
    {
        Destroy(slot.gameObject);
        UpdateSlots();
    }

    public bool CanAddCard()
    {
        return cardCountMax == -1 || cardSlots.Count < cardCountMax;
    }
#endregion
}
