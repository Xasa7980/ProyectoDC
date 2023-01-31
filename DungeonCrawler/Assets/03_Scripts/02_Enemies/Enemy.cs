﻿using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public RoomController room { get; private set; }

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