using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnection
{
    public readonly Room startRoom;
    public readonly Room endRoom;

    public readonly int xCoord;
    public readonly int yCoord;

    public readonly int length;
    public readonly Vector2Int direction;

    public readonly bool skipDrawing;

    public TilePresetManager[] tileMap;

    public RoomConnection (Room startRoom, Room endRoom, bool skipDrawing = false)
    {
        this.startRoom = startRoom;
        this.endRoom = endRoom;

        this.xCoord = Mathf.FloorToInt(startRoom.center.x);
        this.yCoord = Mathf.FloorToInt(startRoom.center.y);

        this.length = Mathf.RoundToInt(Vector2.Distance(startRoom.center, endRoom.center));
        this.direction = Vector2Int.RoundToInt((endRoom.center - startRoom.center).normalized);

        this.skipDrawing = skipDrawing;
    }

    public RoomConnection(Vector2 startPoint, Room startRoom, Vector2 endPoint, Room endRoom)
    {
        this.startRoom = startRoom;
        this.endRoom = endRoom;

        this.xCoord = Mathf.FloorToInt(startPoint.x);
        this.yCoord = Mathf.FloorToInt(startPoint.y);

        this.length = Mathf.RoundToInt(Vector2.Distance(startPoint, endPoint));
        this.direction = Vector2Int.RoundToInt((endPoint - startPoint).normalized);
    }
}
