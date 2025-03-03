using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActionPlayer : Singleton<ActionPlayer>
{
    public Transform skaterTransform;
    public float speed = 3f;
    private bool _isPlaying = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isPlaying)
        {
            Play();
        }
    }

    public void Play()
    {
        if (BoardManager.i.CanAddCard())
        {
            Debug.LogWarning("cannot play if not all cards are placed");
            return;
        }

        List<Vector3> path = new List<Vector3>();
        foreach (Card card in BoardManager.i.cards)
        {
            path.Add(card.actionContainer.startTransform.position);
            path.Add(card.actionContainer.endTransform.position);
        }

        Sequence sequence = DOTween.Sequence();

        Vector3 startPosition = path[0];
        skaterTransform.position = startPosition;
        for (int pathIdx = 1; pathIdx < path.Count; pathIdx++)
        {
            Vector3 endPosition = path[pathIdx];
            float distance = Vector3.Distance(startPosition, endPosition);
            float duration = distance / speed;

            sequence.Append(skaterTransform.DOMove(endPosition, duration).SetEase(Ease.Linear));
            startPosition = endPosition;
        }

        sequence.OnComplete(() => _isPlaying = false);
        _isPlaying = true;
        sequence.Play();

    }
}