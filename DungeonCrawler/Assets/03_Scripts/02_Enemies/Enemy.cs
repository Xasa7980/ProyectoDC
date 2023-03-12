using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public RoomController room { get; private set; }

    public string guid { get; private set; }

    public Enemy prefab { get; private set; }

    [SerializeField] Enemy nextLevel;

    public int xp { get; private set; }

    public void Init(RoomController room, string guid = "")
    {
        this.room = room;

        PatrolBehaviour patrol = GetComponentInChildren<PatrolBehaviour>();
        if (patrol != null)
            patrol.InitWayPoints();

        if (guid == "")
            this.guid = System.Guid.NewGuid().ToString();
        else
            this.guid = guid;
    }

    public void Load(EnemyData data, RoomController room)
    {
        this.room = room;

        PatrolBehaviour patrol = GetComponentInChildren<PatrolBehaviour>();
        if (patrol != null)
            patrol.SetWayPoints(data.wayPoints);

        this.guid = data.guid;
    }

    public void IncreaseXP(int xp)
    {
        this.xp = xp;
    }

    public void IncreaseLevel()
    {
        Debug.Log("Level increased");
    }

    public void SetPrefabReference(Enemy prefab)
    {
        this.prefab = prefab;
    }

    public void RemoveFromRoom()
    {
        room.RemoveEnemy(this);
    }

    public void CloseRoom()
    {
        room.CloseRoom();
    }
}