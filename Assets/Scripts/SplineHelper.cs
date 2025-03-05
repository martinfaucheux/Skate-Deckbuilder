using UnityEngine;
using UnityEngine.Splines;

public class SplineHelper : MonoBehaviour
{
    public Vector3 positionOffset;
    public float scaler;
    public SplineContainer splineContainer;

    public void Initialize()
    {
        transform.localPosition = positionOffset;
        transform.localScale = scaler * Vector3.one;
    }
}