using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public RoomController room { get; private set; }

    public string guid { get; private set; }

    public Enemy prefab { get; private set; }

    public void SetPrefabReference(Enemy prefab)
    {
        this.prefab = prefab;
    }

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

    public void RemoveFromRoom()
    {
        room.RemoveEnemy(this);
    }

    public void CloseRoom()
    {
        room.CloseRoom();
    }
}