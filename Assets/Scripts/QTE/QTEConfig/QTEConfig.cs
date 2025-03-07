using UnityEngine;

public abstract class QTEConfig : ScriptableObject
{
    public abstract ActionSequenceChallenge GetChallenge(KeyCode keyCode);
    [TextArea] public string description;
}



