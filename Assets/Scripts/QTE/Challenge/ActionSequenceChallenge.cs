using UnityEngine;
using System;

public enum ActionSequenceChallengeState
{
    NotStarted, Running, Failed, Success
}

public abstract class ActionSequenceChallenge
{
    public ActionSequenceChallengeState state { get; private set; } = ActionSequenceChallengeState.NotStarted;
    public KeyCode keyCode { get; protected set; }

    // hook used by manager to grant reward or display so feedback
    private event Action _onSuccess;
    private event Action _onFail;
    protected float _startTime;
    public float timeSinceStart => Time.time - _startTime;

    // this method holds most of the logic of the challenge
    public abstract void Update();

    public virtual void End()
    {
        // can be used to trigger the Success hook
        // if challenge can only succeed at the end of the sequence
    }

    public void Reset()
    {
        state = ActionSequenceChallengeState.NotStarted;
        _onSuccess = null;
        _onFail = null;
        ResetValues();
    }

    public void Start()
    {
        state = ActionSequenceChallengeState.Running;
        _startTime = Time.time;
    }

    // abstract to remind to implement it for all challenges
    // reset all internal values 
    public abstract void ResetValues();
    protected void MarkAsSuccess()
    {
        state = ActionSequenceChallengeState.Success;
        _onSuccess?.Invoke();
    }

    protected void MarkAsFailed()
    {
        state = ActionSequenceChallengeState.Failed;
        _onFail?.Invoke();
    }

    public void RegisterOnEvents(Action succeededAction, Action failedAction)
    {
        _onSuccess += succeededAction;
        _onFail += failedAction;
    }

}