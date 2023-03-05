using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Vector3 playerPos;
    public List<Vector3> waypoints; //Así o con GameObject
    public Quaternion playerRot;
    public Health healthRemaining;
    public Health energyRemaining;

    public PlayerData() { }
    public PlayerData(Vector3 pos, Quaternion rot, Health healthRemain, Health energyRemain, GameObject _waypoints) 
    {
        playerPos = pos;
        waypoints.Add(_waypoints.transform.position);
        playerRot = rot;
        healthRemaining = healthRemain;
        energyRemaining = energyRemain;
    }
    public void SavePlayerData(string fileName, Object obj)
    {
        BinarySerializer serializer = new BinarySerializer();
        serializer.Serializing(fileName, obj);
    }
    public PlayerData LoadPlayerData(string fileName, Object obj)
    {
        BinarySerializer serializer = new BinarySerializer();
        serializer.Deserialize(fileName);
        return (PlayerData)serializer.instance;
    }
    public Vector3 GetNearestWaypoint()
    {
        if(waypoints.Count > 0)
        {
            foreach (Vector3 waypoint in waypoints)
            {
                if (Vector3.Distance(playerPos, waypoint) > 20 & Vector3.Distance(playerPos, waypoint) < 20) return waypoint;
            }
        }
        return playerPos;
    }
}
