using UnityEngine;

public class PreciseClickSequenceChallenge : ActionSequenceChallenge
{
    public float tolerance { get; private set; } = 0.2f;
    public float actionTargetTime { get; private set; }
    private float maxActionTime;

    public PreciseClickSequenceChallenge(
        KeyCode keyCode,
        float _tolerance,
        float maxTime
    )
    {
        this.tolerance = _tolerance;
        this.keyCode = keyCode;
        this.maxActionTime = maxTime;
        actionTargetTime = maxTime * Random.Range(0.2f, 1);
    }

    public override void Update()
    {
        base.Update();
        if (state != ActionSequenceChallengeState.Running) return;

        if (Input.GetKeyDown(keyCode))
        {
            if (IsInTimeWindow())
            {
                MarkAsSuccess();
            }
            else
            {
                MarkAsFailed();
            }
        }
    }
    public override void ResetValues()
    {
    }


    public bool IsInTimeWindow()
    {
        Vector2 timeWindow = GetTimeWindow();
        return timeSinceStart >= timeWindow.x && timeSinceStart <= timeWindow.y;
    }

    public Vector2 GetTimeWindow() => new Vector2(
        Mathf.Clamp(actionTargetTime - tolerance, 0, maxActionTime),
        Mathf.Clamp(actionTargetTime + tolerance, 0, maxActionTime)
    );

}