using UnityEngine;

public abstract class ActionSequenceChallenge
{
    public abstract void Update();
    public virtual bool IsSuccess() => false;    // Returns true if challenge succeeded
    public virtual bool IsFailed() => false;   // Returns true if challenge failed
}

public class ClickActionSequenceChallenge : ActionSequenceChallenge
{

    public KeyCode keyCode;
    public int requiredKeyCount;
    private int _clickCount;

    public ClickActionSequenceChallenge(KeyCode keyCode, int requiredKeyCount)
    {
        this.keyCode = keyCode;
        this.requiredKeyCount = requiredKeyCount;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            _clickCount++;
        }
    }

    public override bool IsSuccess() => _clickCount >= requiredKeyCount;
    public override bool IsFailed() => false;

}