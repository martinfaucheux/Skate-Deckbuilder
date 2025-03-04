using UnityEngine;
using UnityEngine.UI;
public class BarActionChallengeDisplay : ChallengeDisplay
{
    private BarSequenceChallenge _challenge;
    public RectTransform gaugeContainer;
    public RectTransform topRedZone;
    public RectTransform bottomRedZone;
    public Slider fillSlider;
    public CanvasGroup canvasGroup;

    // TODO: at some point the ActionContainer will be responsible
    // for instantiating its own corresponding ChallengeDisplay (from QTEConfig)


    void Update()
    {
        if (_challenge == null)
            return;

        UpdatePositions();
    }

    public override void AssignChallenge(ActionSequenceChallenge challenge)
    {
        _challenge = (BarSequenceChallenge)challenge;
        UpdatePositions();

    }

    private void UpdatePositions()
    {

        float bottomRatio = _challenge.validRange.x;
        bottomRedZone.sizeDelta = new Vector2(
            bottomRedZone.sizeDelta.x,
            gaugeContainer.sizeDelta.y * bottomRatio
        );

        float topRatio = _challenge.validRange.y;
        topRedZone.sizeDelta = new Vector2(
            topRedZone.sizeDelta.x,
            gaugeContainer.sizeDelta.y * (1 - topRatio)
        );

        fillSlider.value = _challenge.currentValue;
    }

}