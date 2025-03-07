using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[CustomEditor(typeof(PathContainer))]
public class PathContainerEditor : Editor
{
    private PathContainer t;

    public override void OnInspectorGUI()
    {
        t = (PathContainer)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Fill"))
        {
            Fill();
        }
    }

    public void Fill()
    {
        if (t.splineContainer == null)
        {
            t.splineContainer = t.GetComponent<SplineContainer>();
        }

        int diff = t.characterStates.Count - t.spline.Count + 1;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                t.characterStates.RemoveAt(t.characterStates.Count - 1);
            }
        }
        else if (diff < 0)
        {
            for (int i = 0; i < -diff; i++)
            {
                t.characterStates.Add(CharacterState.Grounded);
            }
        }


    }
}