using UnityEngine;

public class ClickActionSequenceChallenge : ActionSequenceChallenge
{

    public int requiredKeyCount { get; private set; }
    public int clickCount { get; private set; }

    public ClickActionSequenceChallenge(KeyCode keyCode, int requiredKeyCount)
    {
        this.keyCode = keyCode;
        this.requiredKeyCount = requiredKeyCount;
    }

    public override void Update()
    {
        base.Update();
        if (state != ActionSequenceChallengeState.Running) return;

        if (Input.GetKeyDown(keyCode))
        {
            clickCount++;
        }

        if (clickCount >= requiredKeyCount)
        {
            MarkAsSuccess();
        }
    }

    public override void ResetValues() => clickCount = 0;
}