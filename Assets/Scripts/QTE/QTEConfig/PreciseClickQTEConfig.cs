
using UnityEngine;

[CreateAssetMenu(fileName = "PreciseClickQTEConfig", menuName = "ScriptableObject / QTEConfig / PreciseClickQTEConfig")]
public class PreciseClickQTEConfig : QTEConfig
{
    public float tolerance = 0.1f;

    public override ActionSequenceChallenge GetChallenge(KeyCode keyCode)
    {
        return new PreciseClickSequenceChallenge(
            keyCode, tolerance, SequenceManager.i.baseSpeed
        );
    }
}

