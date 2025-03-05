using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class SequenceManager : Singleton<SequenceManager>
{
    public Rigidbody2D characterRigidbody;
    private Queue<ActionSequence> _sequences = new Queue<ActionSequence>();
    public UnityEvent OnSequenceStart;
    public UnityEvent OnSequenceComplete;
    public float baseSpeed = 3f;
    public float actionBaseDuration = 2f;
    public bool isPlaying { get; private set; } = false;
    private ActionSequence _currentSequence;
    private float _characterZ;
    [Tooltip("Event invoked when the player doesn't have enough energy to play the next sequence.")]
    public UnityEvent onInsufficientEnergy;

    void Start()
    {
        Vector3 characterPosition = characterRigidbody.transform.position;
        _characterZ = characterPosition.z;
        characterRigidbody.MovePosition(characterPosition);
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
            _currentSequence.Update();
        }
    }

    public void AddSequences(List<Card> cards)
    {
        int cardCount = cards.Count();
        for (int cardIdx = 0; cardIdx < cardCount; cardIdx++)
        {
            Card card = cards[cardIdx];
            ActionContainer actionContainer = card.actionContainer;
            CardDefinition cardDef = card.cardDefinition;
            Vector3 startPos = actionContainer.startTransform.position;
            startPos.z = _characterZ;

            Vector3 endPos = actionContainer.endTransform.position;
            endPos.z = _characterZ;


            _sequences.Enqueue(new ActionSequence(
                startPos,
                endPos,
                characterRigidbody,
                actionBaseDuration,
                cardDef.energyCost,
                cardDef.energyGain,
                actionContainer.GetPositionFunction,
                actionContainer.CreateChallenge()
            ));

            if (cardIdx < cardCount - 1)
            {
                Vector3 nextStartPos = cards[cardIdx + 1].actionContainer.startTransform.position;
                nextStartPos.z = _characterZ;

                _sequences.Enqueue(new ActionSequence(
                    endPos,
                    nextStartPos,
                    characterRigidbody,
                    0
                ));
            }
        }
    }

    public void Play()
    {
        _sequences = new Queue<ActionSequence>();
        AddSequences(BoardManager.i.cardSlots.Select(cardSlot => cardSlot.card).ToList());
        isPlaying = true;

        OnSequenceStart?.Invoke();
    }
}
