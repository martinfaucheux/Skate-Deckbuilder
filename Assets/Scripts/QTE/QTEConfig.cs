using UnityEngine;

public abstract class QTEConfig : ScriptableObject
{
    public KeyCode keyCode;

    public abstract ActionSequenceChallenge GetChallenge();
}

[CreateAssetMenu(fileName = "ClickQTEConfig", menuName = "ScriptableObject / QTEConfig / ClickQTEConfig")]
public class ClickQTEConfig : QTEConfig
{
    public int clickCount;

    public override ActionSequenceChallenge GetChallenge() => new ClickActionSequenceChallenge(
        keyCode, clickCount
    );
}

[CreateAssetMenu(fileName = "BarQTEConfig", menuName = "ScriptableObject / QTEConfig / BarQTEConfig")]
public class BarQTEConfig : QTEConfig
{
    public Vector2 validRange;
    public float onClickMove = 0.2f;
    public float clickedVelocity = 1f;
    public float noClickedVelocity = 0.5f;

    public override ActionSequenceChallenge GetChallenge() => new KeepBarSequenceChallenge(
        keyCode, validRange, onClickMove, clickedVelocity, noClickedVelocity
    );
}

