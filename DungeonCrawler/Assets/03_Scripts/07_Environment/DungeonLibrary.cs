using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dungeon Library", menuName = "Top Down Engine/Dungeon Library")]
public class DungeonLibrary : ScriptableObject
{
    [SerializeField] TilePresetManager[] entries;
    [SerializeField] TilePresetManager[] floorTiles;
    [SerializeField] TilePresetManager[] wallTiles;
    [SerializeField] TilePresetManager[] cornerTiles;
    [SerializeField] TilePresetManager[] hallTiles;
    [SerializeField] TilePresetManager[] doorTiles;
    [SerializeField] Enemy[] turrets;

    public void PlaceEntry(Room entryRoom, Transform dungeonContainer)
    {
        Ray borderRay = new Ray(new Vector3(entryRoom.center.x, 0.5f, entryRoom.center.y) * 5, Vector3.back);
        if (Physics.Raycast(borderRay, out RaycastHit hit))
        {
            TilePresetManager tile = hit.collider.GetComponentInParent<TilePresetManager>();
            if (tile != null)
            {
                Vector3 position = tile.transform.position + Vector3.back * 5;
                Quaternion rotation = Quaternion.LookRotation(-tile.transform.forward);
                TilePresetManager entry = GameObject.Instantiate(entries[Random.Range(0, entries.Length)], position, rotation);
                entry.transform.parent = tile.transform.parent;

                Vector3 doorPosition = tile.transform.position;
                Quaternion doorRotation = tile.transform.rotation;
                TilePresetManager door = Instantiate(doorTiles[Random.Range(0, doorTiles.Length)], doorPosition, doorRotation);
                door.transform.parent = tile.transform.parent;
                tile.GetComponentInParent<RoomController>().AddDoor(door.GetComponent<Door>(), RoomDirections.S, true);
                door.Init();

                DestroyImmediate(tile.gameObject);
            }
        }
    }

    public RoomController DrawRoom(Room room, Transform dungeonContainer)
    {
        Vector3 bottomLeftCorner = new Vector3(room.center.x - (room.width - 1) / 2f, 0, room.center.y - (room.length - 1) / 2f);
        Transform roomContainer = new GameObject("Room " + room.center.x + "; " + room.center.y).transform;
        roomContainer.parent = dungeonContainer;
        roomContainer.position = new Vector3(room.center.x, 0, room.center.y) * 5;

        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceiling.transform.parent = roomContainer;
        ceiling.transform.localPosition = Vector3.up * 4;
        ceiling.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        ceiling.transform.localScale = new Vector3(room.width * 5 * 0.1f, 1, room.length * 5 * 0.1f);
        DestroyImmediate(ceiling.GetComponent<Collider>());

        //DrawCorners
        //Bottom Left Corner
        TilePresetManager bottomLeftCornerTile = Instantiate(cornerTiles[Random.Range(0, cornerTiles.Length)], bottomLeftCorner * 5, Quaternion.identity);
        bottomLeftCornerTile.transform.parent = roomContainer;
        room.SetTile(bottomLeftCornerTile, bottomLeftCorner.x, bottomLeftCorner.z);
        bottomLeftCornerTile.Init();

        //Top Left Corner
        Vector3 tlp = (bottomLeftCorner + new Vector3(0, 0, room.length - 1));
        TilePresetManager topLeftCorner = Instantiate(cornerTiles[Random.Range(0, cornerTiles.Length)], tlp * 5, Quaternion.Euler(Vector3.up * 90));
        topLeftCorner.transform.parent = roomContainer;
        room.SetTile(topLeftCorner, tlp.x, tlp.z);
        topLeftCorner.Init();

        //Top Right Corner
        Vector3 trp = (bottomLeftCorner + new Vector3(room.width - 1, 0, room.length - 1));
        TilePresetManager topRigtCorner = Instantiate(cornerTiles[Random.Range(0, cornerTiles.Length)], trp * 5, Quaternion.Euler(Vector3.up * 180));
        topRigtCorner.transform.parent = roomContainer;
        room.SetTile(topRigtCorner, trp.x, trp.z);
        topRigtCorner.Init();

        //Bottom Right Corner
        Vector3 brp = (bottomLeftCorner + new Vector3(room.width - 1, 0, 0));
        TilePresetManager bottomRigtCorner = Instantiate(cornerTiles[Random.Range(0, cornerTiles.Length)], brp * 5, Quaternion.Euler(Vector3.up * 270));
        bottomRigtCorner.transform.parent = roomContainer;
        room.SetTile(bottomRigtCorner, brp.x, brp.z);
        bottomRigtCorner.Init();

        //Draw Walls
        //Horizontal Walls
        for (int x = 1; x < room.width - 1; x++)
        {
            Vector3 point1 = new Vector3(bottomLeftCorner.x + x, 0, bottomLeftCorner.z);
            //Instantiate prefab
            TilePresetManager bottomWall = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], point1 * 5, Quaternion.identity);
            bottomWall.transform.parent = roomContainer;
            room.SetTile(bottomWall, point1.x, point1.z);
            bottomWall.Init();

            Vector3 point2 = new Vector3(bottomLeftCorner.x + x, 0, bottomLeftCorner.z + room.length - 1);
            //Instantiate prefab
            TilePresetManager topWall = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], point2 * 5, Quaternion.Euler(Vector3.up * 180));
            topWall.transform.parent = roomContainer;
            room.SetTile(topWall, point2.x, point2.z);
            topWall.Init();
        }

        //Vertical Walls
        for (int y = 1; y < room.length - 1; y++)
        {
            Vector3 point1 = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.z + y);
            //Instantiate prefab
            TilePresetManager leftWall = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], point1 * 5, Quaternion.Euler(Vector3.up * 90));
            leftWall.transform.parent = roomContainer;
            room.SetTile(leftWall, point1.x, point1.z);
            leftWall.Init();

            Vector3 point2 = new Vector3(bottomLeftCorner.x + room.width - 1, 0, bottomLeftCorner.z + y);
            //Instantiate prefab
            TilePresetManager righttWall = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], point2 * 5, Quaternion.Euler(Vector3.up * 270));
            righttWall.transform.parent = roomContainer;
            room.SetTile(righttWall, point2.x, point2.z);
            righttWall.Init();
        }

        for (int x = 1; x < room.width - 1; x++)
        {
            for (int y = 1; y < room.length - 1; y++)
            {
                Vector3 position = (bottomLeftCorner + new Vector3(x, 0, y));
                TilePresetManager floorTile = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], position * 5, Quaternion.identity);
                floorTile.transform.parent = roomContainer;
                room.SetTile(floorTile, position.x, position.z);
                floorTile.Init();
            }
        }

        RoomController controller = roomContainer.gameObject.AddComponent<RoomController>();

        DrawDoors(room, controller);

        roomContainer.transform.position += Vector3.up * room.height * 2.5f;

        return controller;
    }

    void DrawDoors(Room room, RoomController container)
    {
        for(RoomDirections direction=RoomDirections.N;direction<=RoomDirections.W;direction++)
        {
            RoomConnection connection = room.connections[(int)direction];

            if (connection == null) continue;

            Vector3 center = new Vector3(connection.startRoom.center.x, 0, connection.startRoom.center.y);
            Vector3 dirV3 = new Vector3(connection.direction.x, 0, connection.direction.y);

            Ray ray = new Ray((center + Vector3.up * 0.5f) * 5, dirV3);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TilePresetManager tile = hit.collider.GetComponentInParent<TilePresetManager>();
                if (tile)
                {
                    Vector3 position = tile.transform.position;
                    Quaternion rotation = tile.transform.rotation;
                    DestroyImmediate(tile.gameObject);
                    TilePresetManager door = Instantiate(doorTiles[Random.Range(0, doorTiles.Length)], position, rotation);
                    door.transform.parent = container.transform;
                    room.SetTile(door, position.x / 5, position.z / 5);
                    door.Init();

                    container.AddDoor(door.GetComponent<Door>(), direction);
                }
            }
        }
    }

    public void DrawConnection(RoomConnection connection, Transform dungeonContainer)
    {
        Quaternion rotation = Quaternion.LookRotation(new Vector3(connection.direction.x, 0, connection.direction.y));

        int realLength = 0;
        Vector3 startPoint = Vector3.zero;

        if(connection.direction.x == 0)
        {
            realLength = connection.length - connection.startRoom.length / 2 - connection.endRoom.length / 2;
            startPoint = new Vector3(connection.startRoom.center.x, 0, connection.startRoom.center.y + connection.startRoom.length / 2 * connection.direction.y);
        }

        if (connection.direction.y == 0)
        {
            realLength = connection.length - connection.startRoom.width / 2 - connection.endRoom.width / 2;
            startPoint = new Vector3(connection.startRoom.center.x + connection.startRoom.width / 2 * connection.direction.x, 0, connection.startRoom.center.y);
        }

        realLength = Mathf.Abs(realLength);

        Transform hallContainer = new GameObject("Hall").transform;
        hallContainer.position = startPoint * 5;
        hallContainer.parent = dungeonContainer;
        connection.tileMap = new TilePresetManager[realLength - 1];

        Vector3 direction = new Vector3(connection.direction.x, 0, connection.direction.y);

        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceiling.transform.parent = hallContainer;
        ceiling.transform.localRotation = Quaternion.identity;
        ceiling.transform.localPosition = direction * realLength / 2f * 5 + Vector3.up * 4;
        ceiling.transform.localScale = direction * Mathf.Abs(realLength - 1) * 5 * 0.1f + new Vector3(direction.z, 0, direction.x) * 5 * 0.1f;
        ceiling.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        DestroyImmediate(ceiling.GetComponent<Collider>());

        for (int i = 1; i < realLength; i++)
        {
            Vector3 position = startPoint + direction * i;
            TilePresetManager hallTile = Instantiate(hallTiles[Random.Range(0, hallTiles.Length)], position * 5, rotation);
            hallTile.transform.parent = hallContainer;
            connection.tileMap[i - 1] = hallTile;
            hallTile.Init();
        }
    }

    public List<Enemy> PlaceTurrets(Room room, float turretProbability, int maxDificulty)
    {
        List<TilePresetManager> emptyTiles = new List<TilePresetManager>();
        foreach(TilePresetManager tile in room.tileMap)
        {
            if(tile.empty && tile.alowdPropsInstancing)
                emptyTiles.Add(tile);
        }

        List<Enemy> turrets = new List<Enemy>();
        float normalizedDifficulty = (float)room.difficulty / maxDificulty;

        foreach(TilePresetManager tile in emptyTiles)
        {
            if(tile == null)
            {
                Debug.Log("Null tile. skipping!!!!");
                continue;
            }

            float value = Random.value;
            if (value <= normalizedDifficulty && value <= turretProbability)
            {
                Vector3 position = tile.transform.position;
                Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90);
                Enemy turret = Instantiate(this.turrets[Random.Range(0, this.turrets.Length)], position, rotation);
                turret.transform.parent = tile.transform.parent;
                turrets.Add(turret);
            }
        }

        return turrets;
    }
}
