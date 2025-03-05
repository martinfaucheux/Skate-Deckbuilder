using UnityEngine;

public enum ActionSequenceState { Idle, Running, Interrupted, Completed }

public class ActionSequence
{
    public Vector3 startPosition { get; private set; }
    public Vector3 endPosition { get; private set; }
    public ActionSequenceChallenge challenge { get; private set; }
    public ActionSequenceState state = ActionSequenceState.Idle;
    public float speed = 3f;
    private Transform _characterTransform;
    public int energyCost { get; private set; }
    public int energyGain { get; private set; }
    public int scoreGain { get; private set; }

    public ActionSequence(
        Vector3 start,
        Vector3 end,
        float speed,
        Transform characterTransform,
        int energyCost = 0,
        int energyGain = 0,
        int scoreGain = 0,
        ActionSequenceChallenge challenge = null
    )
    {
        startPosition = start;
        endPosition = end;
        this.speed = speed;
        this.challenge = challenge;
        _characterTransform = characterTransform;
        this.energyCost = energyCost;
        this.energyGain = energyGain;
        this.scoreGain = scoreGain;
        state = ActionSequenceState.Idle;
    }

    public void Start()
    {
        state = ActionSequenceState.Running;
        challenge?.Reset();
        challenge?.RegisterOnEvents(OnWinChallenge, OnFailChallenge);
        challenge?.Start();
        _characterTransform.position = startPosition;
    }

    public void Update(float deltaTime)
    {
        if (state != ActionSequenceState.Running) return;

        _characterTransform.position = Vector3.MoveTowards(
            _characterTransform.position, endPosition, deltaTime * speed
        );

        if (challenge != null && challenge.state == ActionSequenceChallengeState.Running)
        {
            challenge.Update();
        }

        if (Vector3.Distance(_characterTransform.position, endPosition) < 0.01f)
        {
            challenge?.End();
            state = ActionSequenceState.Completed;
            if (energyGain > 0)
                EnergyPointManager.i.Add(energyGain);
            if (scoreGain > 0)
                RunManager.Instance.score += scoreGain;
        }
    }

    private void OnWinChallenge()
    {
        // TODO: use this hook to display some feedback
        Debug.Log("Challenge succeeded");
    }


    private void OnFailChallenge()
    {
        // TODO: use this hook to display some feedback
        Debug.Log("Challenge failed");
    }

    public void Interrupt()
    {
        // NOTE: not useful yet
        state = ActionSequenceState.Interrupted;
    }
}
