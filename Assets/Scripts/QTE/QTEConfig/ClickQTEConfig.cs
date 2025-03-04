using UnityEngine;
[CreateAssetMenu(fileName = "ClickQTEConfig", menuName = "ScriptableObject / QTEConfig / ClickQTEConfig")]
public class ClickQTEConfig : QTEConfig
{
    public int clickCount;

    public override ActionSequenceChallenge GetChallenge(KeyCode keyCode) => new ClickActionSequenceChallenge(
        keyCode, clickCount
    );
}
