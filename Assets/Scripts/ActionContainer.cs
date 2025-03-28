using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;
    public ChallengeDisplay challengeDisplay;
    public CardType cardType;
    public Transform arrowSpriteTransform;
    public List<GameObject> QTERenderers;
    public Card card { get; private set; }

    public PathContainer pathContainer { get; private set; }

    public void SetCard(Card card)
    {
        this.card = card;

        // create and attach spline component
        GameObject splinePrefab = card.cardDefinition.splinePrefab;
        if (pathContainer == null && splinePrefab != null)
        {
            GameObject splineGameObject = Instantiate(
                splinePrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
            pathContainer = splineGameObject.GetComponent<PathContainer>();
        }
    }

    public ActionSequenceChallenge CreateChallenge()
    {
        ActionSequenceChallenge challenge = (
            qteConfig != null ? qteConfig.GetChallenge(CardTypeConfiguration.i.TypeToKey(cardType)) : null
        );

        if (challenge != null && challengeDisplay != null)
        {
            challengeDisplay.AssignChallenge(challenge);
            SetArrowSprite(challenge.keyCode);
        }

        return challenge;
    }

    public void SetArrowSprite(KeyCode key)
    {
        float angle = 0;

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

        foreach (Transform child in arrowSpriteTransform)
        {
            child.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void ShowQTERenderer()
    {
        foreach (var go in QTERenderers)
        {
            if (go.TryGetComponent(out SpriteRenderer sr))
            {
                sr.enabled = true;
            }
            else if (go.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 1;
            }
        }
    }

    public void HideQTERenderer()
    {
        foreach (var go in QTERenderers)
        {
            if (go.TryGetComponent(out SpriteRenderer sr))
            {
                sr.enabled = false;
            }
            else if (go.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 0;
            }
        }
    }

}