using UnityEngine;

public abstract class ActionSequenceChallenge
{
    public virtual bool IsSuccess() => false;    // Returns true if challenge succeeded
    public virtual bool IsFailed() => false;   // Returns true if challenge failed
    public virtual void Start() { }
    public abstract void Update();
    public abstract void Reset();
}

public class ClickActionSequenceChallenge : ActionSequenceChallenge
{

    public KeyCode keyCode;
    public int requiredKeyCount { get; private set; }
    public int clickCount { get; private set; }

    public ClickActionSequenceChallenge(KeyCode keyCode, int requiredKeyCount)
    {
        this.keyCode = keyCode;
        this.requiredKeyCount = requiredKeyCount;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            clickCount++;
        }
    }

    public override bool IsSuccess() => clickCount >= requiredKeyCount;
    public override bool IsFailed() => false;
    public override void Reset() => clickCount = 0;
}