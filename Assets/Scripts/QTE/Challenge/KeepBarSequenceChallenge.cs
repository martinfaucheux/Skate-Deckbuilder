using UnityEngine;

public class BarSequenceChallenge : ActionSequenceChallenge
{
    public float onClickMove = 0.2f;
    public float clickedVelocity = 1f;
    public float noClickedVelocity = 0.5f;
    public float currentValue { get; private set; }
    public float validAmplitude { get; private set; } = 0.6f;
    public float rangeMovementAmplitude = 0.3f;
    public float baseMovementPeriod = 2f;
    private float _currentRangeOrigin = 0.5f;
    private float _movementPeriod = 1f;

    public Vector2 validRange => new Vector2(
        Mathf.Clamp(_currentRangeOrigin - validAmplitude / 2f, 0, 1),
        Mathf.Clamp(_currentRangeOrigin + validAmplitude / 2f, 0, 1)
    );

    public BarSequenceChallenge(
        KeyCode keyCode,
        float onClickMove,
        float clickedVelocity,
        float noClickedVelocity,
        float validAmplitude,
        float rangeMovementAmplitude,
        float baseMovementPeriod
    )
    {
        this.keyCode = keyCode;
        this.onClickMove = onClickMove;
        this.clickedVelocity = clickedVelocity;
        this.noClickedVelocity = noClickedVelocity;
        this.validAmplitude = validAmplitude;
        this.rangeMovementAmplitude = rangeMovementAmplitude;
        this.baseMovementPeriod = baseMovementPeriod;
        currentValue = 0.5f;
        _currentRangeOrigin = 0.5f;

        // add random variation on the period
        _movementPeriod = baseMovementPeriod * Random.Range(0.7f, 1.3f) * (Random.value > 0.5f ? 1 : -1);
    }

    public override void Update()
    {
        if (state != ActionSequenceChallengeState.Running) return;

        // move the valid range position center
        UpdateCurrentRangeOrigin();

        // first push
        if (Input.GetKeyDown(keyCode))
        {
            currentValue += onClickMove;
        }
        else
        {
            // when push is maintained
            if (Input.GetKey(keyCode))
            {
                currentValue += clickedVelocity * Time.deltaTime;
            }
            else
            {
                currentValue -= noClickedVelocity * Time.deltaTime;
            }
        }

        if (currentValue < validRange.x || currentValue > validRange.y)
        {
            MarkAsFailed();
        }
    }

    private void UpdateCurrentRangeOrigin()
    {
        float timeSinceStart = Time.time - _startTime;
        _currentRangeOrigin = 0.5f + Mathf.Sin(2 * Mathf.PI * timeSinceStart / _movementPeriod) * rangeMovementAmplitude;
    }

    public override void End()
    {
        if (state != ActionSequenceChallengeState.Failed)
        {
            MarkAsSuccess();
        }
        base.End();
    }

    public override void ResetValues()
    {
        currentValue = 0.5f;
    }

}