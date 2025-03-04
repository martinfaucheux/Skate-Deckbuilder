using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;
public class SequenceManager : Singleton<SequenceManager>
{
    public Transform characterTransform;
    private Queue<ActionSequence> _sequences = new Queue<ActionSequence>();
    public float baseSpeed = 3f;
    public bool isPlaying { get; private set; } = false;
    private ActionSequence _currentSequence;
    private float _characterZ;
    [Tooltip("Event invoked when the player doesn't have enough energy to play the next sequence.")]
    public UnityEvent onInsufficientEnergy;

    void Start()
    {
        _characterZ = characterTransform.position.z;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            if (BoardManager.i.CanAddCard())
            {
                Debug.LogWarning("cannot play if not all cards are placed");
            }
            else
            {
                Play();
            }
        }

        if (isPlaying)
            UpdateSequence();
    }

    private void UpdateSequence()
    {
        if (_currentSequence == null || _currentSequence.state == ActionSequenceState.Completed)
        {
            // try get and start the next sequence
            if (_sequences.Count > 0)
            {
                ActionSequence nextSequence = _sequences.Dequeue();
                int energyPoints = EnergyPointManager.i.currentPoints;
                if ((nextSequence.challenge != null && energyPoints == 0) || energyPoints < nextSequence.energyCost)
                {
                    isPlaying = false;
                    Debug.Log("Energy has run out");
                    onInsufficientEnergy?.Invoke();
                    return;
                }

                EnergyPointManager.i.Add(-nextSequence.energyCost);
                _currentSequence = nextSequence;
                _currentSequence.Start();
            }
            else
            {
                // all sequences have been played
                isPlaying = false;
            }
        }

        if (_currentSequence != null && _currentSequence.state == ActionSequenceState.Running)
        {
            _currentSequence.Update(Time.deltaTime);
        }
    }

    public void AddSequences(List<ActionContainer> actionContainers)
    {
        int containerCount = actionContainers.Count();
        for (int containerIdx = 0; containerIdx < containerCount; containerIdx++)
        {
            ActionContainer actionContainer = actionContainers[containerIdx];
            Vector3 startPos = actionContainer.startTransform.position;
            startPos.z = _characterZ;

            Vector3 endPos = actionContainer.endTransform.position;
            endPos.z = _characterZ;


            _sequences.Enqueue(new ActionSequence(
                startPos,
                endPos,
                baseSpeed,
                characterTransform,
                actionContainer.energyCost,
                actionContainer.energyGain,
                actionContainer.CreateChallenge()
            ));

            if (containerIdx < containerCount - 1)
            {
                Vector3 nextStartPos = actionContainers[containerIdx + 1].startTransform.position;
                nextStartPos.z = _characterZ;

                _sequences.Enqueue(new ActionSequence(
                    endPos,
                    nextStartPos,
                    baseSpeed,
                    characterTransform,
                    0,
                    0,
                    null
                ));
            }
        }
    }

    public void Play()
    {
        _sequences = new Queue<ActionSequence>();
        AddSequences(BoardManager.i.cards.Select(card => card.actionContainer).ToList());
        isPlaying = true;
    }
}
