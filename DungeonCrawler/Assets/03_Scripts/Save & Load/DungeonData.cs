using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dungeon Data", menuName = "DungeonData")]
public class DungeonData : ScriptableObject
{
    public int seed { get; private set; }
    RoomData[] rooms;

    public void Save(int seed, DungeonController dungeon)
    {
        this.seed = seed;
        this.rooms = new RoomData[dungeon.rooms.Count];
        for(int i = 0; i < dungeon.rooms.Count; i++)
        {
            this.rooms[i] = RoomData.ExtractData(dungeon.rooms[i]);
        }
    }

    public void Load(DungeonController dungeon)
    {
        for(int i = 0; i < dungeon.rooms.Count; i++)
        {
            rooms[i].Load(dungeon.rooms[i]);
        }
    }
}
