using UnityEngine;

[CreateAssetMenu(fileName = "QTEConfig", menuName = "ScriptableObject / QTEConfig")]
public class QTEConfig : ScriptableObject
{
    public int clickCount;
    public KeyCode keyCode;

    public ActionSequenceChallenge GetChallenge() => new ClickActionSequenceChallenge(
        keyCode, clickCount
    );
}