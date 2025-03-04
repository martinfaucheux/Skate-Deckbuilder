using UnityEngine;
using System;



public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;
    public ChallengeDisplay challengeDisplay;

    public ActionSequenceChallenge CreateChallenge()
    {
        ActionSequenceChallenge challenge = (
            qteConfig != null ? qteConfig.GetChallenge() : null
        );

        if (challenge != null && challengeDisplay != null)
        {
            challengeDisplay.AssignChallenge(challenge);
        }

        return challenge;
    }
}