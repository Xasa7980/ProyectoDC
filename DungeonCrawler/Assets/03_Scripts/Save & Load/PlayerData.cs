[System.Serializable]
public class PlayerData
{
    public float[] playerPos = new float[3];
    public float[] playerRot = new float[3];

    public float healthRemaining;
    public float energyRemaining;

    public PlayerData() { }
    public PlayerData(Health stats) 
    {
        healthRemaining = stats.healthRemaining;
        energyRemaining = stats.energyRemaining;

        playerPos[0] = stats.playerTransform.localPosition.x;
        playerPos[1] = stats.playerTransform.localPosition.y;
        playerPos[2] = stats.playerTransform.localPosition.z;

        playerRot[0] = stats.playerTransform.localRotation.x;
        playerRot[1] = stats.playerTransform.localRotation.y;
        playerRot[2] = stats.playerTransform.localRotation.z;
    }
    //public Vector3 GetNearestWaypoint()
    //{
    //    if (waypoints.Count > 0)
    //    {
    //        foreach (Vector3 waypoint in waypoints)
    //        {
    //            if (Vector3.Distance(playerPos, waypoint) > 20 & Vector3.Distance(playerPos, waypoint) < 20) return waypoint;
    //        }
    //        return playerPos;
    //    }
    //    else return playerPos;
    //}
}
