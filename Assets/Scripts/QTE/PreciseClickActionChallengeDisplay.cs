using UnityEngine;
using UnityEngine.UI;

public class PreciseClickActionChallengeDisplay : ChallengeDisplay
{
    private PreciseClickSequenceChallenge _challenge;
    public Image goImage;
    private bool _doUpdate = true;
    public Color waitColor;
    public Color goColor;
    public Color successColor;
    public Color failColor;

    void Start()
    {
        goImage.color = waitColor;
    }

    void Update()
    {
        if (_challenge == null || !_doUpdate) return;

        if (_challenge.state == ActionSequenceChallengeState.Success)
        {
            goImage.color = successColor;
            _doUpdate = false;
        }

        if (_challenge.state == ActionSequenceChallengeState.Failed)
        {
            goImage.color = failColor;
            _doUpdate = false;
        }

        goImage.color = _challenge.IsInTimeWindow() ? goColor : waitColor;
    }

    public override void AssignChallenge(ActionSequenceChallenge challenge)
    {
        _challenge = (PreciseClickSequenceChallenge)challenge;
    }
}