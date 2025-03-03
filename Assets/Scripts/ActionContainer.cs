using UnityEngine;
using System;

public class QTEConfig
{
    public int clickCount;
    public KeyCode keyCode;

    public ActionSequenceChallenge GetChallenge() => new ClickActionSequenceChallenge(
        keyCode, clickCount
    );
}

public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;

    public ActionSequenceChallenge GetChallenge() => (
        qteConfig != null ? qteConfig.GetChallenge() : null
    );
}