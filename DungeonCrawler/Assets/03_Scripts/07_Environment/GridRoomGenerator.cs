using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridRoomGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct DungeonArquetype
    {

        public DungeonLibrary arquetype;

        public int maxCount;

        [Range(0, 1)] public float spawnProbability;
    }

    public DungeonLibrary defaultLibrary;
    public DungeonArquetype[] roomArquetypes;

    
    public bool randomSeed = false;
    public int seed = 1234567;

    public enum RoomConnectionMethod { Random, Path }

    [Min(3)] public int floorWidth = 3;
    [Min(3)] public int floorLength = 3;
    [Min(0)] public int roomSpacing = 0;

    [Min(0)] public int floorHeight = 0;
    [Range(0, 1)] public float irregularity = 0.5f;

    public RoomConnectionMethod roomConnectionMethod = RoomConnectionMethod.Path;
    [Range(0, 1)] public float connectionProbability = 0.5f;

    public int roomWidthMin = 6;
    public int roomWidthMax = 12;
    public int roomHeightMin = 6;
    public int roomHeightMax = 12;

    [Range(0 ,1)] public float turretsSpawnProbability = 0.2f;
    [Range(0, 1)] public float bruteRobotsProbability = 0.2f;
    [Range(0, 1)] public float gunnerRobotsProbability = 0.2f;

    int maxDificulty;
    Gradient difficultyGradientVisualization = new Gradient()
    {
        colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.blue, 0),
            new GradientColorKey(Color.red, 1)
        },
        alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1,0),
            new GradientAlphaKey(1,1)
        }
    };

    [Range(0, 1)] public float spawnProbability = 1;

    Room[,] floor;

    Room startRoom;
    Room endRoom;

    private void Start()
    {
        ClearDungeon();
        Generate();
        Populate();
        PlaceEnemies();
        LevelManager.current.SpawnPlayer();
    }

    public void Generate()
    {
        if (randomSeed)
            seed = Random.Range(-10000, 10000);
        
        Random.InitState(seed);

        floor = new Room[floorWidth, floorLength];

        for(int x = 0; x < floorWidth; x++)
        {
            for(int y = 0; y < floorLength; y++)
            {
                int roomWidth = Random.Range(roomWidthMin, roomWidthMax);
                int roomLength = Random.Range(roomHeightMin, roomHeightMax);

                roomWidth += (roomWidth % 2 == 0) ? 1 : 0;
                roomLength += (roomLength % 2 == 0) ? 1 : 0;

                Vector2 roomCenter = new Vector2(x * roomWidthMax + roomWidthMax / 2 + roomSpacing * x, y * roomHeightMax + roomWidthMax / 2 + roomSpacing * y);
                floor[x, y] = new Room(x, y, roomCenter, roomWidth, roomLength);
            }
        }

        startRoom = floor[Random.Range(0, floorWidth), 0];
        endRoom = floor[Random.Range(0, floorWidth), Random.Range(Mathf.RoundToInt(floorLength * 0.75f), floorLength)];

        switch (roomConnectionMethod)
        {
            case RoomConnectionMethod.Random:
                CreateConnections_Random();
                break;

            case RoomConnectionMethod.Path:
                CreateConnections_Path();
                break;
        }

        ClearIsolatedRooms();
        
        foreach(Room r in floor)
        {
            if (r == null) continue;

            defaultLibrary.DrawRoom(r, this.transform);

            foreach(RoomConnection rc in r.connections)
            {
                if (rc.skipDrawing) continue;

                defaultLibrary.DrawConnection(rc, this.transform);
            }
        }

        defaultLibrary.PlaceEntry(startRoom, this.transform);

        PlaceArquetypes();
    }

    void CreateConnections_Random()
    {
        foreach(Room room in floor)
        {
            if (Random.value <= connectionProbability)
            {
                if (room.xCoord - 1 >= 0)
                {
                    room.Connect(floor[room.xCoord - 1, room.yCoord]);
                    floor[room.xCoord - 1, room.yCoord].Connect(room);
                }
            }

            if (Random.value <= connectionProbability)
            {
                if (room.xCoord + 1 < floorWidth)
                {
                    room.Connect(floor[room.xCoord + 1, room.yCoord]);
                    floor[room.xCoord + 1, room.yCoord].Connect(room);
                }
            }

            if (Random.value <= connectionProbability)
            {
                if (room.yCoord - 1 >= 0)
                {
                    room.Connect(floor[room.xCoord, room.yCoord - 1]);
                    floor[room.xCoord, room.yCoord - 1].Connect(room);
                }
            }

            if (Random.value <= connectionProbability)
            {
                if (room.yCoord + 1 < floorLength)
                {
                    room.Connect(floor[room.xCoord, room.yCoord + 1]);
                    floor[room.xCoord, room.yCoord + 1].Connect(room);
                }
            }
        }
    }

    void CreateConnections_Path()
    {
        Room currentRoom = startRoom;

        int currentDifficulty = 1;

        Vector2Int[] possibleDirections = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        while (currentRoom != endRoom)
        {
            List<Room> validRooms = new List<Room>();

            for (int i = 0; i < 4; i++)
            {
                Vector2Int dir = possibleDirections[i];

                int xNext = currentRoom.xCoord + dir.x;
                int yNext = currentRoom.yCoord + dir.y;

                if (xNext < 0 || xNext >= floorWidth || yNext < 0 || yNext >= floorLength)
                    continue;

                Room nextRoom = floor[xNext, yNext];

                if (nextRoom.connections.Count == 4) continue;

                validRooms.Add(nextRoom);
            }

            if (validRooms.Count == 0) break;

            Room selectedRoom = validRooms[Random.Range(0, validRooms.Count)];

            if (!currentRoom.Connected(selectedRoom))
            {
                currentRoom.Connect(selectedRoom);
                selectedRoom.Connect(currentRoom, true);
            }

            currentRoom.difficulty = currentDifficulty;
            currentDifficulty++;

            currentRoom = selectedRoom;
        }

        maxDificulty = currentDifficulty;
    }

    void PlaceArquetypes()
    {
        if (roomArquetypes.Length == 0) return;

        List<Room> rooms = new List<Room>();
        foreach(Room r in floor)
        {
            if(r!=null)
                rooms.Add(r);
        }
        rooms.Remove(startRoom);
        rooms.Remove(endRoom);

        foreach(DungeonArquetype arquetype in roomArquetypes)
        {
            for (int i = 0; i < arquetype.maxCount; i++)
            {
                Room selectedRoom = rooms[Random.Range(0, rooms.Count)];

                arquetype.arquetype.DrawRoom(selectedRoom, this.transform);

                rooms.Remove(selectedRoom);
            }
        }
    }

    public void Populate()
    {
        TilePresetManager[] tiles = GetComponentsInChildren<TilePresetManager>();
        foreach(TilePresetManager tile in tiles)
        {
            tile.Populate(spawnProbability);
        }
    }

    public void PlaceEnemies()
    {
        foreach (Room room in floor)
        {
            if (room == null) continue;

            defaultLibrary.PlaceTurrets(room, turretsSpawnProbability, maxDificulty);
        }
    }

    void ClearIsolatedRooms()
    {
        for(int x =0; x < floorWidth; x++)
        {
            for(int y =0; y < floorLength; y++)
            {
                if (floor[x, y].connections.Count == 0)
                    floor[x, y] = null;
            }
        }
    }

    public void ClearDungeon()
    {
        while (this.transform.childCount > 0)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (floor != null)
        {
            foreach (Room r in floor)
            {
                if (r == null) continue;

                Gizmos.color = Color.red;
                foreach (RoomConnection c in r.connections)
                {
                    Vector3 startPos = new Vector3(c.startRoom.center.x, 0, c.startRoom.center.y);
                    Vector3 endPos = new Vector3(c.endRoom.center.x, 0, c.endRoom.center.y);
                    Gizmos.DrawLine(startPos * 5, endPos * 5);
                }
            }

            foreach (Room r in floor)
            {
                if (r == null) continue;

                Gizmos.color = difficultyGradientVisualization.Evaluate((float)r.difficulty / maxDificulty);

                Gizmos.DrawCube(new Vector3(r.center.x, 0, r.center.y) * 5, new Vector3(r.width, 0, r.length) * 5);
            }


#if UNITY_EDITOR
            Gizmos.color = Color.cyan;
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.Label(new Vector3(startRoom.center.x, 5, startRoom.center.y) * 5, "Entry Room");
            Gizmos.DrawLine(new Vector3(startRoom.center.x, 0, startRoom.center.y) * 5, new Vector3(startRoom.center.x, 5, startRoom.center.y) * 5);
            UnityEditor.Handles.Label(new Vector3(endRoom.center.x, 5, endRoom.center.y) * 5, "Boss Room");
            Gizmos.DrawLine(new Vector3(endRoom.center.x, 0, endRoom.center.y) * 5, new Vector3(endRoom.center.x, 5, endRoom.center.y) * 5);
#endif
        }
    }
}
