using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public DungeonLibrary arquetype;

    public int xCoord { get; private set; }
    public int yCoord { get; private set; }

    public int width { get; private set; }
    public int length { get; private set; }

    public int height;

    public int difficulty;

    public float area => width * length;

    public Vector2 center { get; private set; }

    List<Room> childs = new List<Room>();

    public TilePresetManager[,] tileMap;

    public List<RoomConnection> connections { get; private set; }

    public Room(Vector2 center, int width, int length)
    {
        this.width = width;
        this.length = length;

        this.center = center;

        this.connections = new List<RoomConnection>();

        this.tileMap = new TilePresetManager[width, length];
    }

    public Room(int xCoord, int yCoord, Vector2 center, int width, int length, int height = 0)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;

        this.width = width;
        this.length = length;
        this.height = height;

        this.center = center;

        this.connections = new List<RoomConnection>();

        this.tileMap = new TilePresetManager[width, length];
    }

    public void SetTile(TilePresetManager tile, float x, float y)
    {
        int xInt = Mathf.FloorToInt(x - center.x + width / 2f);
        int yInt = Mathf.FloorToInt(y - center.y + length / 2f);

        if(tileMap[xInt, yInt] != null)
        {
            GameObject.DestroyImmediate(tileMap[xInt, yInt].gameObject);
        }

        tileMap[xInt, yInt] = tile;
    }

    public bool TryDivide(int minRoomSize, out List<Room> output)
    {
        output = new List<Room>();

        if (width < minRoomSize || length < minRoomSize) return false;

        bool wide = width >= length;

        Vector2 bottomLeftCorner = new Vector2(center.x - width / 2f, center.y - length / 2f);

        if (wide)
        {
            int width1 = Random.Range(minRoomSize, width);
            if (width1 >= minRoomSize)
            {
                Vector2 center1 = new Vector2(bottomLeftCorner.x + width1 / 2f, bottomLeftCorner.y + length / 2f);
                Room room = new Room(center1, width1, length);
                //Connect(room);
                output.Add(room);
            }

            int width2 = width - width1;
            if (width2 >= minRoomSize)
            {
                Vector2 center2 = new Vector2(bottomLeftCorner.x + width1 + width2 / 2f, bottomLeftCorner.y + length / 2f);
                Room room = new Room(center2, width2, length);
                //Connect(room);
                output.Add(room);
            }
        }
        else
        {
            int height1 = Random.Range(minRoomSize, length);
            if (height1 >= minRoomSize)
            {
                Vector2 center1 = new Vector2(bottomLeftCorner.x + width / 2f, bottomLeftCorner.y + height1 / 2f);
                Room room = new Room(center1, width, height1);
                //Connect(room);
                output.Add(room);
            }

            int height2 = length - height1;
            if (height2 >= minRoomSize)
            {
                Vector2 center2 = new Vector2(bottomLeftCorner.x + width / 2f, bottomLeftCorner.y + height1 + height2 / 2f);
                Room room = new Room(center2, width, height2);
                //Connect(room);
                output.Add(room);
            }
        }

        childs = output;

        return true;
    }

    public void DrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(center.x, 0, center.y), new Vector3(width - 2, 0, length - 2));

        Vector2 bottomLeft = new Vector2(center.x - (width-1) / 2f, center.y - (length-1) / 2f);

        for(int x = 0; x < width; x++)
        {
            Vector2 point1 = new Vector2(bottomLeft.x + x, bottomLeft.y);
            Vector2 point2 = new Vector2(bottomLeft.x + x, bottomLeft.y + length - 1);
            Gizmos.DrawCube(new Vector3(point1.x, 0, point1.y), new Vector3(1, 0, 1));
            Gizmos.DrawCube(new Vector3(point2.x, 0, point2.y), new Vector3(1, 0, 1));
        }

        for (int y = 0; y < length; y++)
        {
            Vector2 point1 = new Vector2(bottomLeft.x, bottomLeft.y + y);
            Vector2 point2 = new Vector2(bottomLeft.x + width - 1, bottomLeft.y + y);
            Gizmos.DrawCube(new Vector3(point1.x, 0, point1.y), new Vector3(1, 0, 1));
            Gizmos.DrawCube(new Vector3(point2.x, 0, point2.y), new Vector3(1, 0, 1));
        }
    }

    public void DrawConnectionsTree()
    {
        Gizmos.color = Color.green;
        foreach (Room c in childs)
        {
            Gizmos.DrawLine(new Vector3(center.x, 0, center.y), new Vector3(c.center.x, 0, c.center.y));
            c.DrawConnectionsTree();
        }
    }

    public void Connect(Room room, bool skipDrawing = false)
    {
        connections.Add(new RoomConnection(this, room, skipDrawing));
    }

    public void Connect(Vector2 startPoint, Vector2 endPoint, Room room)
    {
        connections.Add(new RoomConnection(startPoint, this, endPoint, room));
        room.Connect(endPoint, startPoint, this);
    }

    public bool Connected(Room room)
    {
        foreach(RoomConnection c in connections)
        {
            if (c.endRoom == room || c.startRoom == room)
                return true;
        }

        return false;
    }

    public void DrawConnections(RoomNode[,] roomGrid)
    {
        foreach (RoomConnection connection in connections)
        {
            for (int i = 0; i < connection.length + 1; i++)
            {
                int xNext = connection.xCoord + connection.direction.x * i;
                int yNext = connection.yCoord + connection.direction.y * i;
                if (roomGrid[xNext, yNext] == null)
                    roomGrid[xNext, yNext] = new RoomNode(xNext, yNext, this, ElementType.Hall);
                else if (roomGrid[xNext, yNext].elementType == ElementType.Wall)
                    roomGrid[xNext, yNext] = new RoomNode(xNext, yNext, this, ElementType.Door);
            }
        }
    }
}
