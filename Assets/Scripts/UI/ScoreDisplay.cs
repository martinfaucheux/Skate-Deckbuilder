using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreDisplay : CoduckStudio.Utils.Singleton<ScoreDisplay>
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public Image imagePanel;

    public Button exitButton;
    public Button restartButton;
    public Button continueButton;

    public GameObject scoreNeededObj;
    public TMP_Text scoreNeededText;
    public GameObject currentScoreObj;
    public TMP_Text currentScoreText;
    public TMP_Text winLoseText;
    
    private Action callback;

    private void Awake()
    {
        Hide(true);
    }

    public void Show(bool instant = false, Action callback = null)
    {
        if (RunManager.Instance.roundIndex < 0) {
            callback?.Invoke();
            return;
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Vector2 targetPos = new Vector2(0, rectTransform.anchoredPosition.y);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }
        
        HandManager.i.Hide();
        BoardManager.i.Hide();
        rectTransform.DOAnchorPos(targetPos, 0.5f).SetDelay(0.2f).SetEase(Ease.InOutQuad).OnComplete(() => {
            this.callback = callback;

            DisplayScore();
        });

        HideEverything();
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

    private void HideEverything()
    {
        exitButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        scoreNeededObj.SetActive(false);
        currentScoreObj.SetActive(false);
        winLoseText.gameObject.SetActive(false);

        imagePanel.color = Color.white;
    }

    private void DisplayScore()
    {
        bool hasWon = RunManager.Instance.score >= RunManager.Instance.runDefinition.rounds[RunManager.Instance.roundIndex].scoreGoal;

        float delay = 0.5f;
        CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
            scoreNeededText.text = RunManager.Instance.runDefinition.rounds[RunManager.Instance.roundIndex].scoreGoal.ToString();
            scoreNeededObj.SetActive(true);

            CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
                currentScoreText.text = RunManager.Instance.score.ToString();
                currentScoreObj.SetActive(true);

                CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
                    if (hasWon) {
                        ColorUtility.TryParseHtmlString("#55C555", out Color winColor);
                        imagePanel.DOColor(winColor, 0.5f);
                        winLoseText.text = "You won!";
                    }
                    else {
                        winLoseText.text = "You lost!";
                        ColorUtility.TryParseHtmlString("#E27F7F", out Color loseColor);
                        imagePanel.DOColor(loseColor, 0.5f);
                    }

                    winLoseText.gameObject.SetActive(true);

                    CoduckStudio.Utils.Async.Instance.WaitForSeconds(delay, () => {
                        if (hasWon) {
                            continueButton.gameObject.SetActive(true);
                        }
                        else {
                            exitButton.gameObject.SetActive(true);
                            restartButton.gameObject.SetActive(true);
                        }
                    });
                });
            });
        });
    }

    public void OnClick_ExitButton()
    {
        FindFirstObjectByType<CoduckStudio.Fade>().HideGame(false, () => {
            SceneManager.LoadScene("MenuScene");
        });
    }

    public void OnClick_RestartButton()
    {
        canvasGroup.interactable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClick_ContinueButton()
    {
        Hide();
    }
}
