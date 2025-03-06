using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RideButton : CoduckStudio.Utils.Singleton<RideButton>
{
    public Button rideButton;
    
    public enum State
    {
        Ride,
        Continue
    }
    private State state;

    private void Update()
    {
        if (state == State.Ride) {
            rideButton.interactable = !SequenceManager.i.isPlaying && !RunManager.Instance.isRunOver && !BoardManager.i.CanAddCard();
        }
        else if (state == State.Continue) {
            rideButton.interactable = !SequenceManager.i.isPlaying && RunManager.Instance.state == RunManager.State.ReadyToBuild;
        }

        if (RunManager.Instance.state == RunManager.State.Building && state == State.Continue) {
            SetState(State.Ride);
        }
        else if (RunManager.Instance.state == RunManager.State.Playing && state == State.Ride) {
            SetState(State.Continue);
        }
    }

    public void OnClick_Ride()
    {
        if (RunManager.Instance.state == RunManager.State.Building) {
            RunManager.Instance.PlayHand();
        }
        else if (RunManager.Instance.state == RunManager.State.ReadyToBuild) {
            RunManager.Instance.OnEndHand();
        }
    }

    public void SetState(State state)
    {
        this.state = state;
        rideButton.GetComponentInChildren<TMPro.TMP_Text>().text = state.ToString();
    }

    public RectTransform rectTransform;

    private void Awake()
    {
        Hide(true);
    }

    public void Show(bool instant = false)
    {
        Vector2 targetPos = new Vector2(0, rectTransform.anchoredPosition.y);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }

        rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void Hide(bool instant = false)
    {
        Vector2 targetPos = new Vector2(1000, rectTransform.anchoredPosition.y);

        if (instant) {
            rectTransform.anchoredPosition = targetPos;
            return;
        }

        rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutQuad);
    }
}
