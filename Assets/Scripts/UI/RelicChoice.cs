using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class RelicChoice : MonoBehaviour
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

    private void Awake()
    {
        Hide(true);
    }

#if UNITY_EDITOR
    // TODO: remove debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Show();
        }
    }
#endif

    public void Show(bool instant = false)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, 0);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }
        
        rectTransform.DOAnchorPos(targetPos, 1f);

        AddRelics();
    }

    public void Hide(bool instant = false)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, -2000);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }
        
        rectTransform.DOAnchorPos(targetPos, 1f);
    }

    private void AddRelics()
    {
        RemoveAllRelics();

        List<RelicDefinition> relics = Resources.LoadAll<RelicDefinition>("Relics").ToList();
        relicDefinition1 = AddRandomRelicToTransform(relics, relicChoiceTransform1);
        relicDefinition2 = AddRandomRelicToTransform(relics, relicChoiceTransform2);
        relicDefinition3 = AddRandomRelicToTransform(relics, relicChoiceTransform3);
        relicDefinition4 = AddRandomRelicToTransform(relics, relicChoiceTransform4);
    }

    private RelicDefinition AddRandomRelicToTransform(List<RelicDefinition> relics, Transform tr)
    {
        RelicUI relic = Instantiate(RunManager.Instance.relicUIPrefab, tr);
        relic.relicDefinition = relics[Random.Range(0, relics.Count)];
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
