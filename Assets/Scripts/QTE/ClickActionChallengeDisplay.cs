using UnityEngine;
public class ClickActionChallengeDisplay : ChallengeDisplay
{
    public ClickActionSequenceChallenge challenge;
    public SpriteRenderer[] tapIconRenderers;

    // TODO: at some point the ActionContainer will be responsible
    // for instantiating its own corresponding ChallengeDisplay (from QTEConfig)

    void Update()
    {
        if (challenge == null)
            return;

        for (int iconIdx = 0; iconIdx < challenge.requiredKeyCount; iconIdx++)
        {
            tapIconRenderers[iconIdx].enabled = iconIdx >= challenge.clickCount;
        }
    }

    public override void AssignChallenge(ActionSequenceChallenge challenge)
    {
        this.challenge = (ClickActionSequenceChallenge)challenge;
    }
}