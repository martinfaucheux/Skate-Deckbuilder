using UnityEngine;
using System;



public class ActionContainer : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public QTEConfig qteConfig;

    public ActionSequenceChallenge GetChallenge() => (
        qteConfig != null ? qteConfig.GetChallenge() : null
    );
}