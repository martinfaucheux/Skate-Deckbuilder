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

    private SplineContainer _splineContainer;

    void Awake()
    {
        // TODO: not the best place to randomize the card type
        // this should be done at the card generation and stored in the card object somehow.
        cardType = CardTypeConfiguration.GetRandomType();
    }


    public void SetCard(Card card)
    {
        this.card = card;

        // create and attach spline component
        GameObject splinePrefab = card.cardDefinition.splinePrefab;
        if (_splineContainer == null && splinePrefab != null)
        {
            GameObject splineGameObject = Instantiate(
                splinePrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
            _splineContainer = splineGameObject.GetComponent<SplineContainer>();
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

    public Vector3 GetPositionFunction(float t)
    {
        t = Mathf.Clamp01(t);
        // use the spline to interpolate the position
        // with 0 <= t <= 1
        Spline spline = _splineContainer.Spline;
        Vector3 splinePosition = spline.EvaluatePosition(t);
        // if positiion is Nan, set to the last position of the spline
        Vector3 position = splinePosition + transform.position;
        position.z = transform.position.z;
        return position;
    }
}