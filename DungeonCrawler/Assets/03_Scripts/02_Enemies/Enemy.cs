using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    RoomController room;

    public void SetRoom(RoomController room)
    {
        this.room = room;
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