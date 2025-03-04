using UnityEngine;

public abstract class QTEConfig : ScriptableObject
{
    public KeyCode keyCode;

    public abstract ActionSequenceChallenge GetChallenge();
}



