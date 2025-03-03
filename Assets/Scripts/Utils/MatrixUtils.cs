using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixUtils
{
    public enum NeighborType { CROSS, EIGHT }

    public static List<Vector2Int> GetNeighborsFromType(NeighborType neighborType)
    {
        switch (neighborType)
        {
            case NeighborType.CROSS:
                return GetNeighbors_4();
            case NeighborType.EIGHT:
                return GetNeighbors_8();
            default:
                throw new System.Exception("Invalid NeighborType");
        }
    }

    public static List<Vector2Int> GetNeighbors_8()
    {
        List<Vector2Int> neighbors = new List<Vector2Int>{
            Vector2Int.up,
            (Vector2Int.down),
            (Vector2Int.left),
            (Vector2Int.right),
            (Vector2Int.up + Vector2Int.left),
            (Vector2Int.up + Vector2Int.right),
            (Vector2Int.down + Vector2Int.left),
            (Vector2Int.down + Vector2Int.right)
         };

        return neighbors;
    }

    public static IEnumerable<Vector2Int> GetNeighborsRadius(int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                yield return new Vector2Int(x, y);
            }
        }
    }

    public static List<Vector2Int> GetNeighbors_4()
    {
        List<Vector2Int> neighbors = new List<Vector2Int>{
            (Vector2Int.up),
            (Vector2Int.down),
            (Vector2Int.left),
            (Vector2Int.right)
         };

        return neighbors;
    }
}

public struct Bounds
{
    public Vector2Int min;
    public Vector2Int max;
    public Bounds(Vector2Int min, Vector2Int max)
    {
        this.min = min;
        this.max = max;
    }

    public Vector2 size { get => new Vector2(1 + max.x - min.x, 1 + max.y - min.y); }
    public Vector2 center { get => new Vector2((max.x + min.x) / 2f, (max.y + min.y) / 2f); }
}