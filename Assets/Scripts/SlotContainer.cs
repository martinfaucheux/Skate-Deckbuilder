using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

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

        if (startingCards.Count > 0) {
            AddCards(startingCards);
        }

        int i = 0;
        foreach (var cardSlot in cardSlots) {
            cardSlot.SetSlotContext(i, cardSlots.Count);
            i++;
        }

        AddEmptySlotsIfNeeded();
    }

    private void Update()
    {
        CheckSwap();
    }

    public CardSlot AddEmptySlot(int index = -1)
    {
        CardSlot slot = Instantiate(slotPrefab, transform);
        if (index != -1) {
            slot.transform.SetSiblingIndex(index);
        }
        slot.card = null;

        UpdateSlots();

        return slot;
    }

    public List<CardSlot> AddCards(List<CardDefinition> modules, bool locked = false)
    {
        RemoveAllCards();

        List<CardSlot> slotsRet = new List<CardSlot>();
        foreach (var module in modules) {
            CardSlot slot = Instantiate(slotPrefab, transform);
            slot.card.cardDefinition = module;
            slot.card = slot._card;
            slot.isLocked = locked;
            slotsRet.Add(slot);
        }

        CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
            UpdateSlots();
        });

        return slotsRet;
    }

    public void RemoveAllCards()
    {
        foreach (CardSlot child in gameObject.GetComponentsInChildren<CardSlot>()) {
            Destroy(child.gameObject);
        }
        cardSlots = new();
    }

    public List<CardSlot> GetCards()
    {
        return cardSlots;
    }

    public void AddEmptySlotsIfNeeded()
    {
        if (cardCountMax == -1 || cardSlots.Count == cardCountMax) {
            return;
        }

        // Add empty slots
        while (cardSlots.Count < cardCountMax) {
            cardSlots.Add(AddEmptySlot());
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
            if (cardSlots[i] == null) {
                continue;
            }

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

        if (cardCountMax != -1)
            AddEmptySlot(selectedSlot.index);

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

        // Destroy empty slot if needed
        if (cardCountMax == cardSlots.Count) {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) {
                Destroy(cardSlots.Find((cardSlot) => cardSlot.isEmpty).gameObject);
            }
            else {
                Destroy(cardSlots.FindLast((cardSlot) => cardSlot.isEmpty).gameObject);
            }
        }

        CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
            UpdateSlots();
        });
    }

    public void DeleteSlot(CardSlot slot)
    {
        Destroy(slot.gameObject);
        UpdateSlots();
    }

    public bool CanAddCard()
    {
        return cardCountMax == -1 || cardSlots.Where((cardSlot) => !cardSlot.isEmpty).ToList().Count < cardCountMax;
    }
#endregion
}
