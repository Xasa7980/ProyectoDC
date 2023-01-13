using UnityEngine;
using UnityEditor;


using AmazingAssets.DynamicRadialMasks;

namespace AmazingAssets.DynamicRadialMasksEditor
{
    [CustomEditor(typeof(DRMGameObject))]
    [CanEditMultipleObjects]
    public class DRMGameObjectEditor : Editor
    {
        SerializedProperty displayAllProperties;
        SerializedProperty maskShape;        

        SerializedProperty radius;
        SerializedProperty intensity;
        SerializedProperty noiseStrength;
        SerializedProperty edgeSize;
        SerializedProperty ringCount;
        SerializedProperty frequency;
        SerializedProperty phaseSpeed;
        SerializedProperty smooth;


        private void OnEnable()
        {
            displayAllProperties = serializedObject.FindProperty("displayAllProperties");
            maskShape = serializedObject.FindProperty("maskShape");            

            radius = serializedObject.FindProperty("radius");
            intensity = serializedObject.FindProperty("intensity");
            noiseStrength = serializedObject.FindProperty("noiseStrength");
            edgeSize = serializedObject.FindProperty("edgeSize");
            ringCount = serializedObject.FindProperty("ringCount");
            frequency = serializedObject.FindProperty("frequency");
            phaseSpeed = serializedObject.FindProperty("phaseSpeed");
            smooth = serializedObject.FindProperty("smooth");
        }
                

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            serializedObject.Update();

            #region Draw Property popup
            Rect position = EditorGUILayout.GetControlRect();
            position.height = 16;
            DynamicRadialMasks.Enum.MaskShape mShape = (DynamicRadialMasks.Enum.MaskShape)maskShape.enumValueIndex;

            displayAllProperties.boolValue = EditorGUI.ToggleLeft(new Rect(position.xMin, position.yMin, UnityEditor.EditorGUIUtility.labelWidth, position.height), DRMEditorResources.guiContentAllProperties, displayAllProperties.boolValue);


            Rect enumDrawRect = position;
            enumDrawRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;
            if (displayAllProperties.boolValue)
            {
                using (new EditorGUIHelper.GUIEnabled(false))
                {
                    GUI.Button(enumDrawRect, "All Properties", EditorStyles.popup);
                }
            }
            else
            {
                if (GUI.Button(enumDrawRect, mShape.ToString(), EditorStyles.popup))
                {

                    PopupWindow.Show(enumDrawRect, new DRMShapesEnumPopupWindow((int)mShape, Callback));
                }
            }
            #endregion

            EditorGUILayout.Space(5);
            if (displayAllProperties.boolValue)
            {
                EditorGUILayout.PropertyField(radius);
                EditorGUILayout.PropertyField(intensity);                
                EditorGUILayout.PropertyField(edgeSize);
                EditorGUILayout.PropertyField(ringCount);
                EditorGUILayout.PropertyField(frequency);
                EditorGUILayout.PropertyField(phaseSpeed);

                DrawAdvancedLabel();

                EditorGUILayout.PropertyField(smooth);
                EditorGUILayout.PropertyField(noiseStrength);
            }
            else
            {
                switch (mShape)
                {
                    case DynamicRadialMasks.Enum.MaskShape.Torus:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);                        
                        EditorGUILayout.PropertyField(edgeSize);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Tube:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(edgeSize);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.HeightField:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(edgeSize);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Dot:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Shockwave:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(edgeSize);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Sonar:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(edgeSize);
                        EditorGUILayout.PropertyField(ringCount);
                        
                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Rings:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(edgeSize);
                        EditorGUILayout.PropertyField(ringCount);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Ripple:
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(intensity);
                        EditorGUILayout.PropertyField(frequency);
                        EditorGUILayout.PropertyField(phaseSpeed);

                        DrawAdvancedLabel();

                        EditorGUILayout.PropertyField(smooth);
                        EditorGUILayout.PropertyField(noiseStrength);
                        break;


                    default:
                        break;
                }
            }


            if(serializedObject.hasModifiedProperties)
            {
                SceneView.RepaintAll();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            DRMGameObject t = (target as DRMGameObject);

            EditorGUI.BeginChangeCheck();
            t.radius = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.radius);
            if (EditorGUI.EndChangeCheck())
            {
                UnityEditor.EditorUtility.SetDirty(t);
            }
        }

        void DrawAdvancedLabel()
        {
            GUILayout.Space(10);
            using (new EditorGUIHelper.GUIEnabled(false))
            {
                EditorGUILayout.LabelField("Advanced", EditorStyles.miniLabel);
            }
            GUILayout.Space(-2);
        }

        void Callback(int value)
        {
            maskShape.enumValueIndex = value;

            serializedObject.ApplyModifiedProperties();
        }
    }
}