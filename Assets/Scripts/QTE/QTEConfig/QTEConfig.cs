using UnityEngine;

public abstract class QTEConfig : ScriptableObject
{
    public abstract ActionSequenceChallenge GetChallenge(KeyCode keyCode);
}



