//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class BSPRoomGenerator : MonoBehaviour
//{
//    [SerializeField] DungeonLibrary library;

//    [SerializeField] bool placeAssets = true;
//    [SerializeField] int width = 30;
//    [SerializeField] int height = 30;
//    [SerializeField] int interactions = 3;
//    [SerializeField] int minRoomSize = 5;
//    [SerializeField] float inset = 1;

//    [SerializeField] bool generate;

//    Room masterRoom;

//    Room[] rooms;
//    //Vector3[] centers;
//    RoomNode[,] nodes;

//    private void Start()
//    {
//        Generate();
//    }

//    void Generate()
//    {
//        Queue<Room> openSet = new Queue<Room>();
//        List<Room> closedSet = new List<Room>();

//        masterRoom = new Room(new Vector2(width / 2f, height / 2f), width, height);
//        openSet.Enqueue(masterRoom);

//        for (int i = 0; i < interactions; i++)
//        {
//            while (openSet.Count > 0)
//            {
//                Room currenRoom = openSet.Dequeue();
//                if (currenRoom.TryDivide(minRoomSize, out List<Room> output))
//                {
//                    closedSet.AddRange(output);
//                }
//            }

//            openSet = new Queue<Room>(closedSet);
//            closedSet.Clear();
//        }

//        rooms = openSet.ToArray();
//        //centers = openSet.Select(r => new Vector3(r.center.x, 0, r.center.y)).ToArray();

//        nodes = new RoomNode[width, height];
//        foreach (Room room in rooms)
//        {
//            library.DrawRoom(room, nodes, this.transform, placeAssets);

//            //int bottomLeftX = Mathf.RoundToInt(room.center.x - room.width / 2f);
//            //int bottomLeftY = Mathf.RoundToInt(room.center.y - room.height / 2f);

//            //for (int x = bottomLeftX; x < bottomLeftX + room.width; x++)
//            //{
//            //    for (int y = bottomLeftY; y < bottomLeftY + room.height; y++)
//            //    {
//            //        nodes[x, y] = new RoomNode(x, y, room);
//            //    }
//            //}

//            //foreach(RoomConnection connection in room.connections)
//            //{
//            //    for(int i = 0; i < connection.length; i++)
//            //    {
//            //        int xNext = connection.xCoord + connection.direction.x * i;
//            //        int yNext = connection.yCoord + connection.direction.y * i;
//            //        nodes[xNext, yNext] = new RoomNode(xNext, yNext, room);
//            //    }
//            //}
//        }

//        //foreach (Room room in rooms)
//        //{
//        //    LookForCruxConnections(room);
//        //    room.DrawConnections(nodes);
//        //}

//        //masterRoom.DrawConnections(nodes);
//    }

//    void LookForCruxConnections(Room room)
//    {
//        for (int y = 0; y < height; y++)
//        {
//            int x = Mathf.FloorToInt(room.center.x);

//            int yUp = Mathf.FloorToInt(room.center.y + y);
//            Vector2 startPoint = new Vector2(Mathf.FloorToInt(room.center.x), Mathf.FloorToInt(room.center.y));

//            if (yUp < height)
//            {
//                if (nodes[x, yUp] != null)
//                {
//                    if (nodes[x, yUp].elementType == ElementType.Corner)
//                        break;

//                    if (nodes[x, yUp].parentRoom != room && !nodes[x, yUp].parentRoom.Connected(room))
//                    {
//                        Vector2 endPoint = new Vector2(x, yUp);
//                        room.Connect(startPoint, endPoint, nodes[x, yUp].parentRoom);
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                break;
//            }
//        }

//        for (int y = 0; y < height; y++)
//        {
//            int x = Mathf.FloorToInt(room.center.x);

//            int yDown = Mathf.FloorToInt((room.center.y - y));
//            Vector2 startPoint = new Vector2(Mathf.FloorToInt(room.center.x), Mathf.FloorToInt(room.center.y));

//            if (yDown >= 0)
//            {
//                if (nodes[x, yDown] != null)
//                {
//                    if (nodes[x, yDown].elementType == ElementType.Corner)
//                        break;

//                    if (nodes[x, yDown].parentRoom != room && !nodes[x, yDown].parentRoom.Connected(room))
//                    {
//                        Vector2 endPoint = new Vector2(x, yDown);
//                        room.Connect(startPoint, endPoint, nodes[x, yDown].parentRoom);
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                break;
//            }
//        }

//        for (int x = 0; x < width; x++)
//        {
//            int y = Mathf.FloorToInt(room.center.y);

//            int xRight = Mathf.FloorToInt(room.center.x + x);
//            Vector2 startPoint = new Vector2(Mathf.FloorToInt(room.center.x), Mathf.FloorToInt(room.center.y));

//            if (xRight < width - 1)
//            {
//                if (nodes[xRight, y] != null)
//                {
//                    if (nodes[xRight, y].elementType == ElementType.Corner)
//                        break;

//                    if (nodes[xRight, y].parentRoom != room && !nodes[xRight, y].parentRoom.Connected(room))
//                    {
//                        Vector2 endPoint = new Vector2(xRight, y);
//                        room.Connect(startPoint, endPoint, nodes[xRight, y].parentRoom);
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                break;
//            }
//        }

//        for (int x = 0; x < width; x++)
//        {
//            int y = Mathf.FloorToInt(room.center.y);

//            int xLeft = Mathf.FloorToInt(room.center.x - x);
//            Vector2 startPoint = new Vector2(Mathf.FloorToInt(room.center.x), Mathf.FloorToInt(room.center.y));

//            if (xLeft >= 0)
//            {
//                if (nodes[xLeft, y] != null)
//                {
//                    if (nodes[xLeft, y].elementType == ElementType.Corner)
//                        break;

//                    if (nodes[xLeft, y].parentRoom != room && !nodes[xLeft, y].parentRoom.Connected(room))
//                    {
//                        Vector2 endPoint = new Vector2(xLeft, y);
//                        room.Connect(startPoint, endPoint, nodes[xLeft, y].parentRoom);
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                break;
//            }
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        //    Gizmos.color = Color.yellow;
//        //    Gizmos.DrawWireCube(new Vector3(width / 2f, 0, height / 2f), new Vector3(width, 0, height));

//        if (nodes != null)
//        {
//            foreach (RoomNode n in nodes)
//            {
//                if (n == null) continue;

//                Color color = Color.white;
//                switch (n.elementType)
//                {
//                    case ElementType.Wall:
//                        color = Color.yellow;
//                        break;

//                    case ElementType.Tile:
//                        color = Color.white;
//                        break;

//                    case ElementType.Corner:
//                        color = Color.red;
//                        break;

//                    case ElementType.Door:
//                        color = Color.green;
//                        break;

//                    case ElementType.Hall:
//                        color = Color.magenta;
//                        break;
//                }

//                color.a = 0.5f;
//                Gizmos.color = color;
//                Gizmos.DrawCube(new Vector3((n.xCoord + 0.5f) * 5, 5, (n.yCoord + 0.5f) * 5), new Vector3(5, 0, 5));
//            }
//        }

//        if (rooms != null)
//        {
//            Gizmos.color = Color.black;
//            for (int i = 1; i < rooms.Length; i++)
//            {
//                Vector3 start = new Vector3(rooms[i - 1].center.x, 0, rooms[i - 1].center.y);
//                Vector3 end = new Vector3(rooms[i].center.x, 0, rooms[i].center.y);
//                Gizmos.DrawLine(start * 5, end * 5);
//            }
//        }

//        //    if (masterRoom != null)
//        //        masterRoom.DrawConnectionsTree();

//        //    if (rooms != null)
//        //    {
//        //        foreach (Room room in rooms)
//        //        {
//        //            room.DrawGizmos();
//        //        }
//        //    }
//    }

//    private void OnValidate()
//    {
//        if (generate)
//        {
//            Generate();
//            generate = false;
//        }
//    }
//}
