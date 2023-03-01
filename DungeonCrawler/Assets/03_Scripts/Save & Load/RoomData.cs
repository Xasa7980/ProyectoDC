using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    [SerializeField] Vector2Int coordinates;
    public Vector2 center;
    public int width;
    public int length;

    public bool cleared;

    [SerializeField] List<EnemyData> enemies;
    public List<EnemyData> defaultEnemies;

    public RoomData (RoomController room)
    {
        coordinates = new Vector2Int (room.room.xCoord, room.room.yCoord);
        cleared = room.cleared;
    }

    public void SetDefaultEnemies(RoomController room)
    {
        defaultEnemies = new List<EnemyData>();
        foreach(Enemy enemy in room.enemies)
        {
            defaultEnemies.Add(EnemyData.ExtractData(enemy));
        }
    }

    public void LoadDefaultEnemies(RoomController room)
    {
        List<Enemy> enemies = new List<Enemy>();

        foreach(EnemyData data in defaultEnemies)
        {
            enemies.Add(data.Load(room));
        }

        room.LoadEnemies(enemies);
    }

    public void Save(RoomController room)
    {
        cleared = room.cleared;
        enemies = new List<EnemyData>();

        foreach(Enemy enemy in room.enemies)
        {
            enemies.Add(EnemyData.ExtractData(enemy));
        }

        //foreach (EnemyData enemy in enemies)
        //{
        //    Enemy e = room.enemies.FirstOrDefault(e => e.guid == enemy.guid);
        //    if (e != null)
        //        enemy.isDead = false;
        //    else
        //        enemy.isDead = true;
        //}
    }

    public void UpdateEnemy(Enemy enemy)
    {
    }

    public void Load(RoomController room)
    {
        if (!cleared)
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < this.enemies.Count; i++)
            {
                enemies.Add(this.enemies[i].Load(room));
            }

            room.AddEnemies(enemies);
        }

        room.cleared = cleared;
    }

    public static RoomData ExtractData(RoomController room)
    {
        RoomData data = new RoomData(room);

        data.enemies = new List<EnemyData>();

        foreach (Enemy enemy in room.enemies)
        {
            data.enemies.Add(EnemyData.ExtractData(enemy));
        }

        return data;
    }
}
