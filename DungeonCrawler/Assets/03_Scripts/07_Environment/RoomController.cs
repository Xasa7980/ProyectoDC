using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class RoomController : MonoBehaviour, ISaveLoad
{
    DungeonController controller;

    public bool cleared { get; private set; }

    public Room room { get; private set; }

    List<Vector3> _freePoints = new List<Vector3>();
    public List<Vector3> freeSpots => _freePoints;

    NavMeshSurface navSurface;

    Door[] doors = new Door[4];

    AlarmLight_VFX[] alarms;

    public List<Enemy> enemies { get; private set; }
    public int enemyCount => enemies.Count;

    public bool playerInside { get; private set; }

    #region Save & Load variables
    public RoomData data;
    #endregion

    #region Construction functions
    public void ConfigureRoom(Room room)
    {
        this.room = room;

        this.navSurface = gameObject.AddComponent<NavMeshSurface>();
        this.navSurface.collectObjects = CollectObjects.Volume;
        this.navSurface.size = new Vector3(room.width * 5, 10, room.length * 5);
        this.navSurface.useGeometry = UnityEngine.AI.NavMeshCollectGeometry.PhysicsColliders;
        this.navSurface.minRegionArea = 6;
        this.navSurface.BuildNavMesh();
        this.enemies = new List<Enemy>();

        alarms = GetComponentsInChildren<AlarmLight_VFX>();

        BoxCollider boundaries = gameObject.AddComponent<BoxCollider>();
        boundaries.size = new Vector3(room.width - 1, 2, room.length - 1) * 5;
        boundaries.isTrigger = true;

        gameObject.layer = LayerMask.NameToLayer("Containers");

        controller = GetComponentInParent<DungeonController>();

        data = new RoomData(this);
    }

    public void AddFreePoints(List<Vector3> points) => freeSpots.AddRange(points);

    public void AddEnemies(List<Enemy> enemies)
    {
        enemies.ForEach(e => e.Init(this));

        this.enemies.AddRange(enemies);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemyCount == 0)
        {
            CLearRoom();
        }
        else if (enemyCount <= 5)
        {
            Dungeon_UI_Manager.current.targetPointer.Enable(enemies);
            Dungeon_UI_Manager.current.targetPointer.RemoveTarget(enemy);
        }
    }

    public void AddDoor(Door door, RoomDirections direction, bool updateAlarms = false)
    {
        doors[(int)direction] = door;
        if (updateAlarms)
            UpdateAlarms();
    }

    void UpdateAlarms()
    {
        alarms = GetComponentsInChildren<AlarmLight_VFX>();
    }
    #endregion

    #region Gameplay Functions
    public void OpenRoom()
    {
        foreach (Door door in doors)
        {
            if (door == null) continue;
            door.Unlock();
        }

        foreach (AlarmLight_VFX alarm in alarms)
        {
            alarm.TurnOff();
        }

        Dungeon_UI_Manager.current.targetPointer.Disable();
    }

    public void CloseRoom()
    {
        if (!playerInside) return;

        controller.CloseRoom();

        foreach (Door door in doors)
        {
            if(door == null) continue;

            door.Close();
            door.Lock();
        }

        foreach (AlarmLight_VFX alarm in alarms)
        {
            alarm.TurnOn();
        }
    }

    public void CLearRoom()
    {
        OpenRoom();
        controller.OnRoomCleared(this);
        cleared = true;
    }

    public void ResetRoom()
    {
        foreach(Enemy e in enemies)
        {
            Destroy(e.gameObject);
        }

        enemies.Clear();
        data.LoadDefaultEnemies(this);
        OpenRoom();
        cleared = false;
    }

    public void LoadEnemies(List<Enemy> enemies)
    {
        this.enemies.Clear();
        this.enemies.AddRange(enemies);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInside = true;
            if (enemyCount == 0 && !cleared)
                CLearRoom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInside = false;
        }
    }

    public void Save()
    {
        data.Save(this);
    }

    public void Load()
    {

    }
}
