using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public readonly string guid;

    [SerializeField] Enemy enemyPrefab;

    [SerializeField] Vector3 position;
    [SerializeField] Quaternion rotation;

    public Vector3[] wayPoints;
    [SerializeField] int currentWayPoint;

    public bool isDead = false;

    public EnemyData(string guid)
    {
        this.guid = guid;
    }

    public Enemy Load(RoomController room)
    {
        Enemy enemy = GameObject.Instantiate<Enemy>(enemyPrefab, position, rotation, room.transform);
        enemy.Load(this, room);
        
        PatrolBehaviour patrol = enemy.GetComponentInChildren<PatrolBehaviour>();
        if (patrol != null)
        {
            patrol.SetWayPoints(wayPoints);
            patrol.currentWayPoint = currentWayPoint;
        }

        return enemy;
    }

    public static EnemyData ExtractData(Enemy enemy)
    {
        Vector3[] waypoints;
        PatrolBehaviour patrol = enemy.GetComponentInChildren<PatrolBehaviour>();
        int currentWaypoint = 0;
        if (patrol != null)
        {
            waypoints = patrol.wayPoints;
            currentWaypoint = patrol.currentWayPoint;
        }
        else
            waypoints = null;

        //return new EnemyData(enemy.guid, enemy.prefab, enemy.transform.position, waypoints);
        return new EnemyData(enemy.guid)
        {
            enemyPrefab = enemy.prefab,
            position = enemy.transform.position,
            rotation = enemy.transform.rotation,
            wayPoints = waypoints,
            currentWayPoint = currentWaypoint
            
        };
    }
}