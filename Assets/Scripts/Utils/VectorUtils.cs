using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class MathUtils
{
    public static int Modulo(int k, int n)
    {
        int modulo = k %= n;
        return modulo < 0 ? modulo + n : modulo;
    }

    public static float Modulo(float k, float n)
    {
        while (k > n)
            k -= n;
        return k;
    }
}
public static class VectorUtils
{
    public static Vector2Int Rotate(Vector2Int vector, int rotation)
    {
        rotation = MathUtils.Modulo(rotation, 4);
        switch (rotation)
        {
            case 0:
                return vector;
            case 1:
                return new Vector2Int(-vector.y, vector.x);
            case 2:
                return new Vector2Int(-vector.x, -vector.y);
            case 3:
                return new Vector2Int(vector.y, -vector.x);
            default:
                Debug.LogError("Invalid rotation");
                return vector;
        }
    }

    public static Vector2Int ToVector2Int(Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

    public static IEnumerable<Vector2Int> EnumeratePositions(Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
                yield return new Vector2Int(x, y);
        }
    }

    public static float GetDistanceToFarthestBound(Vector2Int position, Vector2Int gridSize)
    {
        List<Vector2Int> corners = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(gridSize.x, 0),
            new Vector2Int(0, gridSize.y),
            gridSize
        };
        return corners.Select(corner => Vector2Int.Distance(position, corner)).Max();
    }

}