using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Linq;

public class PathSection
{
    public CharacterState state = CharacterState.Grounded;
    public int weight = 1;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
}

public class PathContainer : MonoBehaviour
{
    public SplineContainer splineContainer;
    public Spline spline => splineContainer.Spline;

    public List<CharacterState> characterStates = new List<CharacterState>();
    public List<PathSection> sections = new List<PathSection>();


    private void Check()
    {
        if (spline.Count != characterStates.Count - 1)
        {
            Debug.LogError("PathContainer: Spline and characterStates count mismatch", this);
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

    private float Redistribute(float t)
    {
        if (sections == null || sections.Count == 0)
            return t;

        int totalWeight = sections.Sum(s => s.weight);
        float[] cumulativeWeights = new float[sections.Count];
        float cumulativeSum = 0f;

        // Compute cumulative distribution
        for (int i = 0; i < sections.Count; i++)
        {
            cumulativeSum += sections[i].weight / (float)totalWeight;
            cumulativeWeights[i] = cumulativeSum;
        }

        // Find the correct section
        for (int i = 0; i < cumulativeWeights.Length; i++)
        {
            if (t <= cumulativeWeights[i])
            {
                float lowerBound = i == 0 ? 0f : cumulativeWeights[i - 1];
                float upperBound = cumulativeWeights[i];
                float sectionFraction = (t - lowerBound) / (upperBound - lowerBound);
                return (i + sectionFraction) / sections.Count;
            }
        }

        return 1f; // Fallback (should never happen due to cumulative sum reaching 1)
    }
}