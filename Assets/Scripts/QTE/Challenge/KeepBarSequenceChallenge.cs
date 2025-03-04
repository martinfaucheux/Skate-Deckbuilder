using UnityEngine;

public class KeepBarSequenceChallenge : ActionSequenceChallenge
{
    public Vector2 validRange;
    public float onClickMove = 0.2f;
    public float clickedVelocity = 1f;
    public float noClickedVelocity = 0.5f;
    public float currentValue { get; private set; }

    public KeepBarSequenceChallenge(KeyCode keyCode, Vector2 validRange, float onClickMove, float clickedVelocity, float noClickedVelocity)
    {
        this.keyCode = keyCode;
        this.validRange = validRange;
        this.onClickMove = onClickMove;
        this.clickedVelocity = clickedVelocity;
        this.noClickedVelocity = noClickedVelocity;
        currentValue = 0.5f;
    }

    public override void Update()
    {
        // if (state != ActionSequenceChallengeState.Running) return;

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