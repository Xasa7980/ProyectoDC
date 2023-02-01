using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_UI_Manager : MonoBehaviour
{
    public static Dungeon_UI_Manager current { get; private set; }

    public TargetPointer targetPointer;
    public UI_DungeonMap map;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(map.isOpen)
                map.Close();
            else
                map.Open();
        }
    }
}
