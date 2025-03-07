using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class CardChoice : CoduckStudio.Utils.Singleton<CardChoice>
{
    public SlotContainer slotContainer;
    public int cardCount = 5;
    public GameObject selectButtonCanvasPrefab;
    private Action callback;
    private int count = 1;

    private void Awake()
    {
        Hide(true);
    }

#if UNITY_EDITOR
    // TODO: remove debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            Show(false, () => {
                HandManager.i.Show();
                BoardManager.i.Show();
            });
        }
    }
#endif

    public void Show(bool instant = false, Action callback = null, int count = 1)
    {
        this.count = count;

        Vector3 targetPos = new Vector3(0, transform.position.y, transform.position.z);

        if (instant) {
            transform.position = targetPos;
            return;
        }

        slotContainer.cardCountMax = cardCount;
        slotContainer.RemoveAllCards();

        HandManager.i.Hide();
        BoardManager.i.Hide();
        transform.DOMove(targetPos, 0.5f).SetDelay(0.5f).SetEase(Ease.InOutQuad).OnComplete(() => {
            this.callback = callback;
            AddCards();
        });

    }

    public void Hide(bool instant = false)
    {
        DisableAll();

        Vector3 targetPos = new Vector3(-50, transform.position.y, transform.position.z);

        if (instant) {
            transform.position = targetPos;
            return;
        }
        
        transform.DOMove(targetPos, 0.5f).SetEase(Ease.InOutQuad);
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.5f, () => {
            callback?.Invoke();
            callback = null;
        });
    }

    private void DisableAll()
    {
        foreach (var cardSlot in slotContainer.GetCards()) {
            if (!cardSlot.isEmpty && cardSlot?.card?.cardVisual) {
                cardSlot.card.cardVisual.GetComponentInChildren<Button>().interactable = false;
            }
        }
    }

    private void AddCards()
    {
        List<CardDefinition> allCards = Resources.LoadAll<CardDefinition>("Cards").ToList();

        List<CoduckStudio.Utils.WeightedRandom.Weight<CardDefinition>> weights = new();
        foreach (var card in Resources.LoadAll<CardDefinition>("Cards")) {
            int weight = 10;

            weights.Add(new CoduckStudio.Utils.WeightedRandom.Weight<CardDefinition> {
                weight = weight,
                data = card
            });
        }

        List<CardDefinition> cardsToAdd = CoduckStudio.Utils.WeightedRandom.GetRandoms(weights, cardCount, new System.Random()).ToList();

        List<CardSlot> cardSlots = slotContainer.AddCards(cardsToAdd, true);

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.1f, () => {
            foreach (var cardSlot in cardSlots) {
                GameObject selectButtonCanvas = Instantiate(selectButtonCanvasPrefab, cardSlot.transform);
                cardSlot.card.cardVisual.AddInfoBottom(selectButtonCanvas.transform, true);
                
                CardDefinition cardDefinition = cardSlot.card.cardDefinition;
                selectButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    RunManager.Instance.inventory.Add(cardDefinition);

                    CardChosenAnimation(() => {
                        if (count > 0) {
                            AddCards();
                        }
                        else {
                            Hide();
                        }
                    });
                });
            }
        });
    }

    public void CardChosenAnimation(Action callback)
    {
        DisableAll();
        count--;

        // Reduce all cards size
        slotContainer.forceCardBigHeight = false;
        foreach (var cardSlot in slotContainer.GetCards()) {
            if (!cardSlot.isEmpty && cardSlot?.card?.cardVisual) {
                cardSlot.card.cardVisual.SetHeight(CardVisual.Height.Small);
            }
        }

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.3f, () => {
            slotContainer.RemoveAllCards();

            CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.3f, () => {
                slotContainer.forceCardBigHeight = true;
                
                callback?.Invoke();
            });
        });
    }
}
