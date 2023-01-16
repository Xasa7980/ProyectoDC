using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridRoomGenerator))]
public class GridDungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridRoomGenerator generator = (GridRoomGenerator)target;


        if (GUILayout.Button("Open Editor", GUILayout.Height(30)))
        {
            GridDungeonGeneratorEditorWindow.OpenWindow(generator);
        }

        //if(GUILayout.Button("Generate Dungeon"))
        //{
        //    generator.ClearDungeon();
        //    generator.Generate();
        //}

        //if(GUILayout.Button("Clear Dungeon"))
        //{
        //    generator.ClearDungeon();
        //}
    }
}
