using UnityEngine;
public class ClickActionChallengeDisplay : ChallengeDisplay
{
    public ClickActionSequenceChallenge challenge;
    public SpriteRenderer[] tapIconRenderers;
    public Color winColor;
    public Color waitColor;
    public Color lostColor;

    // TODO: at some point the ActionContainer will be responsible
    // for instantiating its own corresponding ChallengeDisplay (from QTEConfig)

    void Update()
    {
        if (challenge == null)
            return;

        if (challenge.state == ActionSequenceChallengeState.Failed)
        {
            for (int iconIdx = 0; iconIdx < challenge.requiredKeyCount; iconIdx++)
            {
                tapIconRenderers[iconIdx].color = lostColor;
            }
            return;
        }

        for (int iconIdx = 0; iconIdx < challenge.requiredKeyCount; iconIdx++)
        {
            if (iconIdx < challenge.clickCount)
            {
                tapIconRenderers[iconIdx].color = winColor;
            }
            else if (iconIdx >= challenge.clickCount - 1)
            {
                tapIconRenderers[iconIdx].color = waitColor;
            }
        }
    }

    public override void AssignChallenge(ActionSequenceChallenge challenge)
    {
        this.challenge = (ClickActionSequenceChallenge)challenge;
    }
}