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
    [Tooltip("How much the bar will move when the player clicks for the first time")]
    [Range(0, 1)]
    public float onClickMove = 0.2f;

    [Tooltip("How much the bar will move when the player holds the click")]
    [Range(0, 5)]
    public float clickedVelocity = 1f;

    [Tooltip("How much the bar will decrease when the player doesn't click")]
    [Range(0, 5)]
    public float noClickedVelocity = 0.5f;

    [Tooltip("The amplitude of the valid bar range")]
    [Range(0, 1)]
    public float validAmplitude = 0.6f;

    [Tooltip("The amplitude of the movement of the bar")]
    [Range(0, 0.5f)]
    public float rangeMovementAmplitude = 0.3f;

    [Tooltip("The period of sine movement of the bar")]
    [Range(0, 10)]
    public float baseMovementPeriod = 2f;

    public override ActionSequenceChallenge GetChallenge() => new BarSequenceChallenge(
        keyCode, onClickMove, clickedVelocity, noClickedVelocity, validAmplitude, rangeMovementAmplitude, baseMovementPeriod
    );
}

