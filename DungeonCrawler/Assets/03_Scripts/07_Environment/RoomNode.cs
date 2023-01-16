using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Tile,
    Wall,
    Corner,
    Door,
    Hall
}

public class RoomNode
{
    public Room parentRoom { get; private set; }
    public ElementType elementType { get; private set; }

    public readonly int xCoord;
    public readonly int yCoord;

    public RoomNode(int xCoord, int yCoord, Room parentRoom, ElementType elementType)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.parentRoom = parentRoom;
        this.elementType = elementType;
    }
}
