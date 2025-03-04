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

    public ActionSequence(Vector3 start, Vector3 end, float speed, Transform characterTransform, ActionSequenceChallenge challenge = null)
    {
        startPosition = start;
        endPosition = end;
        this.speed = speed;
        this.challenge = challenge;
        _characterTransform = characterTransform;
        state = ActionSequenceState.Idle;
    }

    public void Start()
    {
        state = ActionSequenceState.Running;
        challenge?.Reset();
        challenge?.RegisterOnSuccess(OnWinChallenge);
        challenge?.Start();
        _characterTransform.position = startPosition;
    }

    public void Update(float deltaTime)
    {
        if (state != ActionSequenceState.Running) return;

        _characterTransform.position = Vector3.MoveTowards(
            _characterTransform.position, endPosition, deltaTime * speed
        );

        if (challenge != null)
        {
            challenge.Update();
        }

        if (Vector3.Distance(_characterTransform.position, endPosition) < 0.01f)
        {
            challenge?.End();
            state = ActionSequenceState.Completed;
        }
    }

    private void OnWinChallenge()
    {
        Debug.Log("Challenge succeeded");
    }

    public void Interrupt()
    {
        // NOTE: not useful yet
        state = ActionSequenceState.Interrupted;
    }
}
