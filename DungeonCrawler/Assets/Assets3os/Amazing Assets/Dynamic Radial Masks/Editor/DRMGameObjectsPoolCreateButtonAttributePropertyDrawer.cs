using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using AmazingAssets.DynamicRadialMasks;


namespace AmazingAssets.DynamicRadialMasksEditor
{
    [CustomPropertyDrawer(typeof(DRMGameObjectsPoolCreateButtonAttribute))]
    public class DRMGameObjectsPoolCreateButtonAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 18;

            DRMGameObjectsPool t = (property.serializedObject.targetObject as DRMGameObjectsPool);


            using (new EditorGUIHelper.GUIEnabled(false))
            {
                if (t.DRMController != null)
                {
                    using (new EditorGUIHelper.GUIBackgroundColor(t.DRMGameObjects.Count > t.DRMController.count ? Color.red : Color.white))
                    {
                        EditorGUI.LabelField(position, "Supported Count", t.DRMController.count.ToString(), EditorStyles.objectField);
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, "Supported Count", "N/A", EditorStyles.objectField);
                }
            }


            position.xMin += UnityEditor.EditorGUIUtility.labelWidth;
            position.y += 20;
            position.height = 18;
            
            if (GUI.Button(position, "Create"))
            {
                if (t.DRMGameObjects == null)
                    t.DRMGameObjects = new List<DRMGameObject>();



                string objectName = "DRM GameObject (1)";
                if (t.DRMGameObjects.Count > 0)
                {
                    List<string> names = new List<string>();
                    foreach (var item in t.DRMGameObjects)
                    {
                        if (item != null && string.IsNullOrEmpty(item.name) == false)
                            names.Add(item.name);
                    }

                    objectName = UnityEditor.ObjectNames.GetUniqueName(names.ToArray(), "DRM GameObject (1)");
                }

                var go = new GameObject(objectName);
                Undo.RegisterCreatedObjectUndo(go, "Created go");

                DRMGameObject DRMObject = go.AddComponent<DRMGameObject>();

                if (t.DRMController == null)
                {
                    DRMObject.displayAllProperties = true;
                }
                else
                {
                    DRMObject.displayAllProperties = false;
                    DRMObject.maskShape = t.DRMController.shape;
                }


                //Find index of null element in the array and replace it.
                int index = t.DRMGameObjects.FindIndex(c => c == null);

                if (index != -1)
                    t.DRMGameObjects[index] = DRMObject;
                else
                    t.DRMGameObjects.Add(DRMObject);


                //Force controller update
                t.UpdateController();

                if (t.DRMController != null)
                {
                    t.DRMController.UpdateShaderData();

                    SceneView.RepaintAll();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40;
        }
    }
}
