using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;
    public ChallengeDisplay challengeDisplay;
    public CardType cardType;
    public SpriteRenderer arrowSpriteRenderer;

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
            SetArrowSprite(challenge);
        }

        return challenge;
    }

    public void SetArrowSprite(ActionSequenceChallenge challenge)
    {
        float angle = 0;

        KeyCode key = challenge.keyCode;
        if (key == KeyCode.RightArrow)
        {
            angle = 0;
        }
        else if (key == KeyCode.UpArrow)
        {
            angle = 90;
        }
        else if (key == KeyCode.LeftArrow)
        {
            angle = 180;
        }
        else if (key == KeyCode.DownArrow)
        {
            angle = 270;
        }

        arrowSpriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}