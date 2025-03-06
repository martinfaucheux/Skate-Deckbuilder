using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Diagnostics;

public class RelicChoice : CoduckStudio.Utils.Singleton<RelicChoice>
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public Transform relicChoiceTransform1;
    private RelicDefinition relicDefinition1;
    public Transform relicChoiceTransform2;
    private RelicDefinition relicDefinition2;
    public Transform relicChoiceTransform3;
    private RelicDefinition relicDefinition3;
    public Transform relicChoiceTransform4;
    private RelicDefinition relicDefinition4;
    private Action callback;

    private void Awake()
    {
        Hide(true);
    }

#if UNITY_EDITOR
    // TODO: remove debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Show(false, () => {
                HandManager.i.Show();
                BoardManager.i.Show();
            });
        }
    }
#endif

    public void Show(bool instant = false, Action callback = null)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Vector2 targetPos = new Vector2(0, rectTransform.anchoredPosition.y);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }
        
        HandManager.i.Hide();
        BoardManager.i.Hide();
        RideButton.Instance.Hide();
        rectTransform.DOAnchorPos(targetPos, 0.5f).SetDelay(0.2f).SetEase(Ease.InOutQuad).OnComplete(() => {
            this.callback = callback;
        });

        AddRelics();
    }

    public void Hide(bool instant = false)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Vector2 targetPos = new Vector2(4000, rectTransform.anchoredPosition.y);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }

        rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutQuad);
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.5f, () => {
            callback?.Invoke();
            callback = null;
        });
    }

    private void AddRelics()
    {
        RemoveAllRelics();

        List<CoduckStudio.Utils.WeightedRandom.Weight<RelicDefinition>> weights = new();
        foreach (var relic in Resources.LoadAll<RelicDefinition>("Relics")) {
            int weight = 10;

            // Less chances if he already has the relic
            if (RunManager.Instance.HasRelic(relic.name)) {
                weight /= 2;
            }

            weights.Add(new CoduckStudio.Utils.WeightedRandom.Weight<RelicDefinition> {
                weight = weight,
                data = relic
            });
        }

        List<RelicDefinition> relics = CoduckStudio.Utils.WeightedRandom.GetRandoms(weights, 4, new System.Random()).ToList();

        relicDefinition1 = AddRelicToTransform(relics[0], relicChoiceTransform1);
        relicDefinition2 = AddRelicToTransform(relics[1], relicChoiceTransform2);
        relicDefinition3 = AddRelicToTransform(relics[2], relicChoiceTransform3);
        relicDefinition4 = AddRelicToTransform(relics[3], relicChoiceTransform4);
    }

    private RelicDefinition AddRelicToTransform(RelicDefinition relicDefinition, Transform tr)
    {
        RelicUI relic = Instantiate(RunManager.Instance.relicUIPrefab, tr);
        relic.relicDefinition = relicDefinition;
        return relic.relicDefinition;
    }

    private void RemoveAllRelics()
    {
        foreach (Transform child in relicChoiceTransform1) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in relicChoiceTransform2) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in relicChoiceTransform3) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in relicChoiceTransform4) {
            Destroy(child.gameObject);
        }
    }

    public void OnClick_Relic(int index)
    {
        if (index == 1) {
            RunManager.Instance.AddRelic(relicDefinition1);
        } else if (index == 2) {
            RunManager.Instance.AddRelic(relicDefinition2);
        } else if (index == 3) {
            RunManager.Instance.AddRelic(relicDefinition3);
        } else if (index == 4) {
            RunManager.Instance.AddRelic(relicDefinition4);
        }

        Hide();
    }
}
