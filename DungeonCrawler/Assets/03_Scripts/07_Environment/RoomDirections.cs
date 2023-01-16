using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomDirections
{
    N,
    E,
    S,
    W
}

public static class RoomDirectionsExtensions
{
    public static RoomDirections Opposite(this RoomDirections direction)
    {
        return (direction < RoomDirections.S) ? direction + 2 : direction - 2;
    }

    public static RoomDirections Next(this RoomDirections direction)
    {
        return (direction < RoomDirections.W) ? direction += 1 : RoomDirections.N;
    }

    public static RoomDirections Previous(this RoomDirections direction)
    {
        return (direction > RoomDirections.N) ? direction -= 1 : RoomDirections.W;
    }

    public static Vector2Int AsCoord(this RoomDirections direction)
    {
        switch (direction)
        {
            case RoomDirections.N: return new Vector2Int(0, 1);

            case RoomDirections.E: return new Vector2Int(1, 0);

            case RoomDirections.S: return new Vector2Int(0, -1);

            case RoomDirections.W: return new Vector2Int(-1, 0);

            default:
                throw new System.NotImplementedException();
        }
    }
}
