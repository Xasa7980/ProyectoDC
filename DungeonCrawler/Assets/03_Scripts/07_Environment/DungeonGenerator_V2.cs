using System.Collections;
using UnityEngine;

public class DungeonGenerator_V2 : Generator
{
    [SerializeField, Min(20)] int roomWidth = 60;
    [SerializeField, Min(20)] int roomHeight = 60;

    [SerializeField, Min(5)] Vector2Int hallWidth;
    [SerializeField, Min(5)] Vector2Int hallHeight;

    [SerializeField] int hallCount = 4;

    [SerializeField] bool generate = false;

    public enum CellType
    {
        Room,
        Intersection
    }

    RoomCell[,] cells;
    dRoom[] rooms;

    private void Start()
    {
        Generate();
    }

    public override void ClearDungeon()
    {
        throw new System.NotImplementedException();
    }

    public override void Generate()
    {
        if (randomSeed)
            seed = Random.Range(-10000, 10000);

        Random.InitState(seed);

        cells = new RoomCell[roomWidth, roomHeight];
        rooms = new dRoom[hallCount];

        for(int i = 0; i < hallCount; i++)
        {
            int width = Random.Range(hallWidth.x, hallWidth.y);
            int height = Random.Range(hallHeight.x, hallHeight.y);

            int xOffset = Random.Range(0, roomWidth - width);
            int yOffset = Random.Range(0, roomHeight - height);

            rooms[i] = new dRoom(new Vector3(xOffset, 0, yOffset), new Vector3(width, 0, height));

            for (int x = xOffset; x < xOffset + width; x++)
            {
                for (int y = yOffset; y < yOffset + height; y++)
                {
                    if (cells[x, y] == null)
                        cells[x, y] = new RoomCell(new Vector3(x, 0, y), CellType.Room);
                    else
                        cells[x, y] = new RoomCell(new Vector3(x, 0, y), CellType.Intersection);
                }
            }
        }
    }

    public override void Generate(int seed)
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnProps()
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        foreach (RoomCell cell in cells)
        {
            if (cell == null) continue;

            if (cell.type == CellType.Room) continue;

            Color color = cell.type == CellType.Room ? Color.green : Color.yellow;

            Gizmos.color = color;
            Gizmos.DrawCube(cell.position, new Vector3(1, 0, 1));
            //Gizmos.color = Color.magenta;
            //Gizmos.DrawWireCube(cell.position, new Vector3(1, 0, 1));
        }

        //foreach(dRoom r in rooms)
        //{
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawWireCube(r.position + r.size / 2 - new Vector3(0.5f, 0, 0.5f), r.size);
        //}
    }

    private void OnValidate()
    {
        if(generate)
        {
            Generate();
            generate = false;
        }
    }

    public class RoomCell
    {
        public readonly Vector3 position;
        public readonly CellType type;

        public RoomCell (Vector3 position, CellType type)
        {
            this.position = position;
            this.type = type;
        }
    }

    public class dRoom
    {
        public readonly Vector3 position;
        public readonly Vector3 size;

        public dRoom(Vector3 position, Vector3 size)
        {
            this.position = position;
            this.size = size;
        }
    }
}