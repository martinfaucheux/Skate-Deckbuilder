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

        HandManager.i.Hide();
        BoardManager.i.Hide();
        transform.DOMove(targetPos, 0.5f).SetDelay(0.5f).SetEase(Ease.InOutQuad).OnComplete(() => {
            this.callback = callback;
        });

        AddCards();
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
        
        List<CardDefinition> cardsToAdd = new List<CardDefinition>();
        for (int i = 0; i < cardCount; i++) {
            cardsToAdd.Add(allCards[UnityEngine.Random.Range(0, allCards.Count)]);
        }

        List<CardSlot> cardSlots = slotContainer.AddCards(cardsToAdd, true);

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.1f, () => {
            foreach (var cardSlot in cardSlots) {
                GameObject selectButtonCanvas = Instantiate(selectButtonCanvasPrefab, cardSlot.transform);
                cardSlot.card.cardVisual.AddInfoBottom(selectButtonCanvas.transform, true);
                
                CardDefinition cardDefinition = cardSlot.card.cardDefinition;
                selectButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    RunManager.Instance.inventory.Add(cardDefinition);

                    DisableAll();
                    count--;

                    if (count > 0) {
                        AddCards();
                    }
                    else {
                        Hide();
                    }
                });
            }
        });
    }
}
