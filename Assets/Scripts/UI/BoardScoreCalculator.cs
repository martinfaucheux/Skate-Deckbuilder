using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardScoreCalculator : CoduckStudio.Utils.Singleton<BoardScoreCalculator>
{
    public class Score
    {
        public int value;
        public TMP_Text text;
        public bool isFail = false;
        public bool isOutOfEnergy = false;
    }
    public List<Score> scores = new List<Score>();
    public TMP_Text scorePrefab;

    void Awake()
    {
        Hide(true);
    }

    #region DuringPlay
    public void AddScore(int scoreToAdd, bool isFail, bool isOutOfEnergy)
    {
        Score score = new Score() {
            value = scoreToAdd,
            text = Instantiate(scorePrefab, BoardManager.i.slotContainer.GetCards()[scores.Count].card.cardVisual.infoTop.transform),
            isFail = isFail,
            isOutOfEnergy = isOutOfEnergy
        };
        score.text.transform.position = new Vector3(score.text.transform.position.x, score.text.transform.position.y + 1.5f, score.text.transform.position.z);
        
        if ((isOutOfEnergy || isFail) && RunManager.Instance.HasRelic("Lifebelt Wheel")) {
            score.value = 10;
        }

        ColorUtility.TryParseHtmlString("#E27F7F", out Color loseColor);
        if (isFail) {
            score.text.text = "FAIL";
            score.text.color = loseColor;
        }
        else if (isOutOfEnergy) {
            score.text.text = "OUT OF ENERGY";
            score.text.color = loseColor;
        }
        else {
            score.text.text = $"+{scoreToAdd.ToString()} <sprite=1>";
        }

        scores.Add(score);
    }

    public void RemoveAllScores()
    {
        scores = new();
    }
#endregion

#region AfterPlay
    public GameObject descriptionAndAmountPrefab;
    public GameObject iconAndAmountPrefab;
    public Transform descriptionsTransform;
    
    public void ShowAndCalculateScore()
    {
        Show(() => {
            CalculateScore();
        });
    }

    int totalScore = 0;

    public void CalculateScore()
    {
        while (scores.Count < BoardManager.i.slotContainer.GetCards().Count) {
            AddScore(0, false, true);
        }

        totalScore = 0;

        int i = 0;
        foreach (var score in scores) {
            int savedIndex = i;
    
            GameObject descriptionAndAmount = Instantiate(descriptionAndAmountPrefab, descriptionsTransform);
            descriptionAndAmount.transform.GetChild(0).GetComponent<TMP_Text>().text = $"#{savedIndex + 1}";
            descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "";

            // score.text.transform.SetParent(null, true);
            score.text.transform.DOJump(Camera.main.ScreenToWorldPoint(descriptionAndAmount.transform.GetChild(1).position), 5, 1, 1f)
            .SetEase(Ease.InOutQuad)
            .SetDelay((savedIndex + 1) * 0.3f)
            .OnComplete(() => {
                totalScore += scores[savedIndex].value;

                ColorUtility.TryParseHtmlString("#E27F7F", out Color loseColor);
                if (scores[savedIndex].isFail) {
                    if (RunManager.Instance.HasRelic("Lifebelt Wheel")) {
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = $"<s><color=#E27F7F>FAIL</color></s> +{scores[savedIndex].value} <sprite=1>";
                    }
                    else {
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "FAIL";
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().color = loseColor;
                    }
                }
                else if (scores[savedIndex].isOutOfEnergy) {
                    if (RunManager.Instance.HasRelic("Lifebelt Wheel")) {
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = $"<s><color=#E27F7F>OUT OF ENERGY</color></s> +{scores[savedIndex].value} <sprite=1>";
                    }
                    else {
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "OUT OF ENERGY";
                        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().color = loseColor;
                    }
                }
                else {
                    descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = $"+{scores[savedIndex].value} <sprite=1>";
                }
                Destroy(score.text.gameObject);

                if (savedIndex >= scores.Count - 1) {
                    AddOthers();
                }
            });
            
            i++;
        }

        UpdateLayoutDisplay();
    }

    private void AddOthers()
    {
        UpdateLayoutDisplay();

        float duration = 0.3f;
        float delay = 0;

        // Test color wheels
        for (int i = 0; i < 4; i++) {
            CardType cardType = (CardType)i;
            RelicDefinition relicFound = RunManager.Instance.GetRelics().Find((v) => v.name == $"{cardType.ToString()} Wheel");
            if (relicFound == null) {
                continue;
            }

            delay += duration;

            CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
                bool success = BoardManager.i.cardSlots.FindAll((v) => v.card.cardDefinition.cardType == cardType).Count >= ((float)BoardManager.i.cardSlots.Count / 2);
                string text = "+0 <sprite=1>";
                if (success) {
                    text = "+50 <sprite=1>";
                    totalScore += 50;
                }

                GameObject iconAndAmount = Instantiate(iconAndAmountPrefab, descriptionsTransform);
                iconAndAmount.transform.GetChild(0).GetComponentInChildren<Image>().sprite = relicFound.sprite;
                iconAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
            });
        }

        delay += duration;
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
            AddMultipliers();
        });
    }

    private void AddMultipliers()
    {
        UpdateLayoutDisplay();

        float duration = 0.3f;
        float delay = 0;
        
        RelicDefinition rainbowWheelFound = RunManager.Instance.GetRelics().Find((v) => v.name == "Rainbow Wheel");
        if (rainbowWheelFound != null) {
            delay += duration;
            CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
                GameObject iconAndAmount = Instantiate(iconAndAmountPrefab, descriptionsTransform);
                iconAndAmount.transform.GetChild(0).GetComponentInChildren<Image>().sprite = rainbowWheelFound.sprite;

                bool success = true;
                List<CardSlot> cardSlots = BoardManager.i.slotContainer.GetCards();
                for (int i = 0; i < cardSlots.Count - 1; i++) {
                    if (cardSlots[i].card.cardDefinition.cardType == cardSlots[i + 1].card.cardDefinition.cardType) {
                        success = false;
                        break;
                    }
                }

                if (success) {
                    iconAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "x1.5";
                    totalScore = (int)(totalScore * 1.5f);
                }
                else {
                    iconAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "x1";
                }
            });
        }

        delay += duration;
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
            AddTotal();
        });
    }

    private void AddTotal()
    {
        UpdateLayoutDisplay();

        GameObject descriptionAndAmount = Instantiate(descriptionAndAmountPrefab, descriptionsTransform);
        descriptionAndAmount.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = "";

        descriptionAndAmount = Instantiate(descriptionAndAmountPrefab, descriptionsTransform);
        descriptionAndAmount.transform.GetChild(0).GetComponent<TMP_Text>().text = "Total:";
        descriptionAndAmount.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{totalScore} <sprite=1>";

        UpdateLayoutDisplay();

        RunManager.Instance.score += totalScore;

        RunManager.Instance.SetStateReadyToBuild();
    }

    private void UpdateLayoutDisplay()
    {
        CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
            descriptionsTransform.GetComponent<VerticalLayoutGroup>().enabled = false;
            CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
                descriptionsTransform.GetComponent<VerticalLayoutGroup>().enabled = true;

                CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
                    GetComponent<VerticalLayoutGroup>().enabled = false;
                    CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
                        GetComponent<VerticalLayoutGroup>().enabled = true;
                    });
                });
            });
        });
    }

    public void Show(System.Action callback)
    {
        Vector2 targetPos = new Vector2(0, 0);

        GetComponent<RectTransform>().DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() => {
            callback?.Invoke();
        });
    }

    public void Hide(bool instant = false, System.Action callback = null)
    {
        Vector2 targetPos = new Vector2(1500, 0);

        if (instant) {
            GetComponent<RectTransform>().anchoredPosition = targetPos;
            return;
        }

        GetComponent<RectTransform>().DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutQuad);
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.5f, () => {
            foreach (Transform child in descriptionsTransform) {
                Destroy(child.gameObject);
            }
            UpdateLayoutDisplay();

            RemoveAllScores();

            callback?.Invoke();
        });
    }
#endregion
}
