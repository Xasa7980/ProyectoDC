using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Upgrade))]
public class UpgradeEditor : Editor
{
    Upgrade upgrade;

    private void OnEnable()
    {
        upgrade = (Upgrade)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_displayName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_mainImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_target"));

        if (upgrade.target != Upgrade.Target.Weapon && upgrade.target != Upgrade.Target.Automatic)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_mode"));

            if (upgrade.mode == Upgrade.Mode.Absolute)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_value"));
            else
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_rangedValue"));
        }
        else if (upgrade.target == Upgrade.Target.Weapon)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rangedValue"));
        }
        else if (upgrade.target == Upgrade.Target.Automatic)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rangedValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_timeInterval"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("useEffect"));

        serializedObject.ApplyModifiedProperties();
    }
}
