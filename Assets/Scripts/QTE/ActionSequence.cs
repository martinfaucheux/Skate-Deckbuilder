using System;
using UnityEngine;

public enum ActionSequenceState { Idle, Running, Interrupted, Completed }

public class ActionSequence
{
    public Vector3 startPosition { get; private set; }
    public Vector3 endPosition { get; private set; }
    public ActionSequenceChallenge challenge { get; private set; }
    public ActionSequenceState state = ActionSequenceState.Idle;
    private Rigidbody2D _characterRigidbody;
    public int energyCost { get; private set; }
    public int energyGain { get; private set; }
    private Func<float, PathData> pathDataFunc;
    private float _startTime;
    public float sequenceDuration { get; private set; }
    public float timeSinceStart => Time.time - _startTime;

    public ActionSequence(
        Vector3 start,
        Vector3 end,
        Rigidbody2D characterRigidbody,
        float sequenceDuration,
        int energyCost = 0,
        int energyGain = 0,
        Func<float, PathData> pathDataFunc = null,
        ActionSequenceChallenge challenge = null
    )
    {
        // if not defined use a linear interpolation
        this.pathDataFunc = (pathDataFunc != null) ? pathDataFunc : PathData.GetDefaultFunc(start, end);

        startPosition = start;
        endPosition = end;
        this.challenge = challenge;
        _characterRigidbody = characterRigidbody;
        this.energyCost = energyCost;
        this.energyGain = energyGain;
        state = ActionSequenceState.Idle;
        this.sequenceDuration = sequenceDuration;
    }

    public void Start()
    {
        state = ActionSequenceState.Running;
        challenge?.Reset();
        challenge?.RegisterOnEvents(OnWinChallenge, OnFailChallenge);
        challenge?.Start();
        _characterRigidbody.MovePosition(startPosition);
        _startTime = Time.time;
    }

    public void Update()
    {
        if (state != ActionSequenceState.Running) return;

        // use the Func to update postion
        float t = sequenceDuration > 0 ? timeSinceStart / sequenceDuration : 0;
        PathData pathData = pathDataFunc(t);
        _characterRigidbody.MovePosition(pathData.position);

        Vector3 tangent = pathData.tangent;
        _characterRigidbody.MoveRotation(Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg);

        if (challenge != null && challenge.state == ActionSequenceChallengeState.Running)
        {
            challenge.Update();
        }

        if (timeSinceStart >= sequenceDuration)
        {
            challenge?.End();
            state = ActionSequenceState.Completed;
            if (energyGain > 0)
                EnergyPointManager.i.Add(energyGain);
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
