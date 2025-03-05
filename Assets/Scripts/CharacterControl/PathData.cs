
using System;
using UnityEngine;
public enum CharacterState { Grounded, Airborn, Riding }
public class PathData
{
    public Vector3 position;
    public Vector3 tangent;
    public CharacterState characterState;

    public static Func<float, PathData> GetDefaultFunc(Vector3 start, Vector3 end)
    {
        return (t) =>
        {
            Vector3 dir = (end - start).normalized;
            return new PathData
            {
                position = Vector3.Lerp(start, end, t),
                tangent = dir,
                characterState = CharacterState.Grounded
            };
        };
    }
}