using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_UI_Manager : MonoBehaviour
{
    public static Dungeon_UI_Manager current { get; private set; }

    public TargetPointer targetPointer;

    private void Awake()
    {
        current = this;
    }
}
