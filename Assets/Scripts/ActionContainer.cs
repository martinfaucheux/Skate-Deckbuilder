using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;
    public ChallengeDisplay challengeDisplay;
    public CardType cardType;

    [Tooltip("If the player doesn't have enough energy when entering the sequence, they will lose the round.")]
    public int energyCost = 0;
    [Tooltip("If the player completes the sequence, they will gain this much energy.")]
    public int energyGain = 0;

    void Awake()
    {
        // here so we are sure it is always defined
        cardType = CardTypeConfiguration.GetRandomType();
    }

    public ActionSequenceChallenge CreateChallenge()
    {
        ActionSequenceChallenge challenge = (
            qteConfig != null ? qteConfig.GetChallenge(CardTypeConfiguration.i.TypeToKey(cardType)) : null
        );

        if (challenge != null && challengeDisplay != null)
        {
            challengeDisplay.AssignChallenge(challenge);
        }

        return challenge;
    }
}