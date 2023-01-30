using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonController : MonoBehaviour
{
    DungeonGenerator generator;

    [SerializeField] Animator roomAnim;

    public List<RoomController> rooms = new List<RoomController>();

    public event System.Action<RoomController> onRoomCleared = delegate { };
    public UnityEvent onRoomClearedUnityEvent = new UnityEvent();
    public event System.Action onPlayerDetected = delegate { };

    private void Start()
    {
        CreateDungeon();
        DrawDungeon();
    }

    public void CreateDungeon()
    {
        generator = GetComponent<DungeonGenerator>();

        generator.Generate();
    }

    void DrawDungeon()
    {
        foreach (Room r in generator.floor)
        {
            if (r == null) continue;

            RoomController room = r.arquetype.DrawRoom(r, this.transform);
            room.ConfigureRoom(r);
            rooms.Add(room);

            foreach (RoomConnection rc in r.connections)
            {
                if (rc == null) continue;

                if (rc.skipDrawing) continue;

                generator.defaultLibrary.DrawConnection(rc, this.transform);
            }
        }
        generator.SpawnProps();
        SpawnEnemies();

        generator.defaultLibrary.PlaceEntry(generator.startRoom, this.transform);

        LevelManager.current.SpawnPlayer();
    }

    public void SpawnEnemies()
    {
        foreach (RoomController room in rooms)
        {
            if (room == null) continue;

            room.AddEnemies(generator.defaultLibrary.PlaceTurrets(room.room, generator.turretsSpawnProbability, generator.maxDificulty));
        }
    }

    public void CloseRoom()
    {
        onPlayerDetected();
    }

    public void OnRoomCleared(RoomController room)
    {
        onRoomCleared(room);
        onRoomClearedUnityEvent.Invoke();

        roomAnim.SetTrigger("RoomCleared");

        Debug.Log("Room CLeared");
    }
}