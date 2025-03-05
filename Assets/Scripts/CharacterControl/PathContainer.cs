using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
public class PathContainer : MonoBehaviour
{
    public SplineContainer splineContainer;
    public Spline spline => splineContainer.Spline;

    public List<CharacterState> characterStates = new List<CharacterState>();

    private void Check()
    {
        if (spline.Count != characterStates.Count - 1)
        {
            Debug.LogError("PathContainer: Spline and characterStates count mismatch");
        }
    }

    public PathData Evaluate(float t)
    {
        t = Mathf.Clamp01(t);
        // what is the current node index
        int knotIndex = (int)spline.ConvertIndexUnit(t, PathIndexUnit.Knot);
        CharacterState state = characterStates[Mathf.Min(knotIndex, characterStates.Count - 1)];

        spline.Evaluate(t, out float3 _position, out float3 _tangent, out float3 _);
        Vector3 tangent = state == CharacterState.Grounded ? _tangent : Vector3.right;
        Vector3 position = (Vector3)_position + transform.position;
        position.z = transform.position.z;

        return new PathData
        {
            position = position,
            tangent = tangent,
            characterState = state
        };
    }
}