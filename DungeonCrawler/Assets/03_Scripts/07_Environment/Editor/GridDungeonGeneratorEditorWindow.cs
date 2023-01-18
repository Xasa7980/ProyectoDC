using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridDungeonGeneratorEditorWindow : EditorWindow
{
    [SerializeField] DungeonGenerator generator;
    [SerializeField] SerializedObject serializedObject;

    Vector2 scroll;
    bool floorSettingsFoldout = true;
    bool roomSettingsFoldout = true;
    bool enemiesFoldout = true;

    public static void OpenWindow(DungeonGenerator generator)
    {
        GridDungeonGeneratorEditorWindow window = (GridDungeonGeneratorEditorWindow)EditorWindow.GetWindow(typeof(GridDungeonGeneratorEditorWindow), false, "Dungeon Editor");

        window.generator = generator;
        window.serializedObject = new SerializedObject(generator);
        window.minSize = new Vector2(300, 300);
        
        window.Show();
    }

    private void OnGUI()
    {
        if (generator == null)
            generator = FindObjectOfType<DungeonGenerator>();

        if (generator == null)
            return;
        else
            serializedObject = new SerializedObject(generator);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("randomSeed"));
        generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultLibrary"));
        EditorGUILayout.LabelField("Props Spawning");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnProbability"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("roomArquetypes"));

        GUILayout.Space(15);

        GUILayout.BeginVertical("Box");
        floorSettingsFoldout = EditorGUILayout.Foldout(floorSettingsFoldout, "Floor Settings");

        if (floorSettingsFoldout)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floorWidth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floorLength"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floorHeight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("irregularity"));

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomSpacing"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomConnectionMethod"));
            if (generator.roomConnectionMethod == DungeonGenerator.RoomConnectionMethod.Random)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("connectionProbability"));
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        GUILayout.Space(15);

        GUILayout.BeginVertical("Box");
        roomSettingsFoldout = EditorGUILayout.Foldout(roomSettingsFoldout, "Room Settings");

        if (roomSettingsFoldout)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomWidthMin"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomWidthMax"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomHeightMin"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roomHeightMax"));

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        GUILayout.Space(15);

        GUILayout.BeginVertical("Box");
        enemiesFoldout = EditorGUILayout.Foldout(enemiesFoldout, "Enemies");

        if (enemiesFoldout)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("turretsSpawnProbability"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bruteRobotsProbability"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gunnerRobotsProbability"));

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Dungeon", GUILayout.Width(150), GUILayout.Height(25)))
        {
            generator.ClearDungeon();
            generator.Generate();
        }

        if (GUILayout.Button("Clear Dungeon", GUILayout.Width(150), GUILayout.Height(25)))
        {
            generator.ClearDungeon();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Populate Dungeon", GUILayout.Width(150), GUILayout.Height(25)))
        {
            generator.SpawnProps();
        }

        //if (GUILayout.Button("Place Enemies", GUILayout.Width(150), GUILayout.Height(25)))
        //{
        //    generator.SpawnEnemies();
        //}
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }
}
