using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
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

    public int maxDificulty { get; private set; }
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

    public Room[,] floor;

    public Room startRoom { get; private set; }
    public Room endRoom { get; private set; }

    //private void Start()
    //{
    //    ClearDungeon();
    //    Generate();
    //    Populate();
    //    PlaceEnemies();
    //    LevelManager.current.SpawnPlayer();
    //}

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
                floor[x, y].arquetype = defaultLibrary;
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
        
        //foreach(Room r in floor)
        //{
        //    if (r == null) continue;

        //    defaultLibrary.DrawRoom(r, this.transform);

        //    foreach(RoomConnection rc in r.connections)
        //    {
        //        if (rc == null) continue;

        //        if (rc.skipDrawing) continue;

        //        defaultLibrary.DrawConnection(rc, this.transform);
        //    }
        //}

        //defaultLibrary.PlaceEntry(startRoom, this.transform);

        PlaceArquetypes();
    }

    public void Generate(int seed)
    {
        this.seed = seed;

        Random.InitState(seed);

        floor = new Room[floorWidth, floorLength];

        for (int x = 0; x < floorWidth; x++)
        {
            for (int y = 0; y < floorLength; y++)
            {
                int roomWidth = Random.Range(roomWidthMin, roomWidthMax);
                int roomLength = Random.Range(roomHeightMin, roomHeightMax);

                roomWidth += (roomWidth % 2 == 0) ? 1 : 0;
                roomLength += (roomLength % 2 == 0) ? 1 : 0;

                Vector2 roomCenter = new Vector2(x * roomWidthMax + roomWidthMax / 2 + roomSpacing * x, y * roomHeightMax + roomWidthMax / 2 + roomSpacing * y);
                floor[x, y] = new Room(x, y, roomCenter, roomWidth, roomLength);
                floor[x, y].arquetype = defaultLibrary;
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

        //foreach(Room r in floor)
        //{
        //    if (r == null) continue;

        //    defaultLibrary.DrawRoom(r, this.transform);

        //    foreach(RoomConnection rc in r.connections)
        //    {
        //        if (rc == null) continue;

        //        if (rc.skipDrawing) continue;

        //        defaultLibrary.DrawConnection(rc, this.transform);
        //    }
        //}

        //defaultLibrary.PlaceEntry(startRoom, this.transform);

        PlaceArquetypes();
    }

    void CreateConnections_Random()
    {
        foreach (Room room in floor)
        {
            for (RoomDirections direction = RoomDirections.N; direction <= RoomDirections.W; direction++)
            {
                if (Random.value <= connectionProbability)
                {
                    Vector2Int nextPos = new Vector2Int(room.xCoord, room.yCoord) + direction.AsCoord();

                    if (nextPos.x >= 0 && nextPos.x < floorWidth && nextPos.y >= 0 && nextPos.y < floorHeight)
                    {
                        room.Connect(floor[nextPos.x, nextPos.y], direction);
                    }
                }
            }
        }
    }

    void CreateConnections_Path()
    {
        Room currentRoom = startRoom;

        int currentDifficulty = 1;

        while (currentRoom != endRoom)
        {
            Room[] validRooms = new Room[4];
            List<RoomDirections> validDirections = new List<RoomDirections>();

            for (RoomDirections direction = RoomDirections.N;direction<= RoomDirections.W;direction++)
            {
                Vector2Int dir = direction.AsCoord();

                int xNext = currentRoom.xCoord + dir.x;
                int yNext = currentRoom.yCoord + dir.y;

                if (xNext < 0 || xNext >= floorWidth || yNext < 0 || yNext >= floorLength)
                    continue;

                Room nextRoom = floor[xNext, yNext];

                if (nextRoom.fullConnected) continue;

                validRooms[(int)direction] = nextRoom;
                validDirections.Add(direction);
            }

            if (validDirections.Count == 0) break;

            RoomDirections selectedDirection = validDirections[Random.Range(0, validDirections.Count)];
            Room selectedRoom = validRooms[(int)selectedDirection];

            if (!currentRoom.Connected(selectedRoom))
            {
                currentRoom.Connect(selectedRoom, selectedDirection);
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
                selectedRoom.arquetype = arquetype.arquetype;

                //arquetype.arquetype.DrawRoom(selectedRoom, this.transform);

                rooms.Remove(selectedRoom);
            }
        }
    }

    public void SpawnProps()
    {
        TilePresetManager[] tiles = GetComponentsInChildren<TilePresetManager>();
        foreach(TilePresetManager tile in tiles)
        {
            tile.Populate(spawnProbability);

            RoomController controller = tile.GetComponentInParent<RoomController>();
            if (controller)
                controller.AddFreePoints(tile.freeSpots);
        }
    }

    void ClearIsolatedRooms()
    {
        for (int x = 0; x < floorWidth; x++)
        {
            for (int y = 0; y < floorLength; y++)
            {
                if (floor[x, y].isolated)
                {
                    floor[x, y] = null;
                }
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
                    if (c == null) continue;

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
