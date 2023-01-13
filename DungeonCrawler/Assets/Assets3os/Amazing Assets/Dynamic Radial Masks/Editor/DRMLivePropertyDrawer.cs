using UnityEngine;
using UnityEditor;


using AmazingAssets.DynamicRadialMasks;

namespace AmazingAssets.DynamicRadialMasksEditor
{
    [CustomPropertyDrawer(typeof(DRMLiveProperty))]
    public class DRMLivePropertyDrawer : PropertyDrawer
    {
        static float propertyHeight = 18;

        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = propertyHeight;

            // Draw label      
            EditorGUI.PropertyField(position, property.FindPropertyRelative("evolutionType"), label);


            using (new EditorGUIHelper.EditorGUIIndentLevel(1))
            {
                switch ((DRMLiveProperty.Enum.AnimationType)property.FindPropertyRelative("evolutionType").enumValueIndex)
                {
                    case DRMLiveProperty.Enum.AnimationType.Constant:
                        {
                            position.y += propertyHeight + 2;
                            DrawValue(property, "startValue", position, "Value");
                        }
                        break;

                    case DRMLiveProperty.Enum.AnimationType.ConstantRange:
                        {
                            position.y += propertyHeight + 2;
                            DrawMinMax(property, "startValue", position, "Value", true, true);
                        }
                        break;

                    case DRMLiveProperty.Enum.AnimationType.Lerp:
                        {
                            position.y += propertyHeight + 2;
                            DrawLerpValue(property, "startValue", "endValue", position, "Value");
                        }
                        break;

                    case DRMLiveProperty.Enum.AnimationType.LerpRange:
                        {
                            bool checkForZero = property.FindPropertyRelative("startValue").vector2Value == property.FindPropertyRelative("endValue").vector2Value ? true : false;

                            position.y += propertyHeight + 2;
                            DrawMinMax(property, "startValue", position, "Start Value", true, checkForZero);

                            position.y += propertyHeight + 2;
                            DrawMinMax(property, "endValue", position, "End Value", true, checkForZero);
                        }
                        break;

                    case DRMLiveProperty.Enum.AnimationType.Curve:
                        {
                            position.y += propertyHeight + 2;
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("curve"), new GUIContent(" "));
                            EditorGUI.LabelField(position, "Value", EditorStyles.miniLabel);
                        }
                        break;
                }
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property, false);
        }

        static public float GetPropertyHeight(SerializedProperty property, bool includeFinalOffset)
        {
            float height = propertyHeight * 2 + 2;

            switch ((DRMLiveProperty.Enum.AnimationType)property.FindPropertyRelative("evolutionType").enumValueIndex)
            {
                case DRMLiveProperty.Enum.AnimationType.LerpRange:
                    height += propertyHeight + 2;
                    break;

                default: break;
            }

            if (includeFinalOffset)
                height += 5;

            return height;
        }


        void DrawValue(SerializedProperty property, string propName, Rect position, string label)
        {
            EditorGUI.LabelField(position, label, EditorStyles.miniLabel);


            Vector2 value = property.FindPropertyRelative(propName).vector2Value;

            Color errorColor = (value.x == 0 ? Color.red : Color.white);



            using (new EditorGUIHelper.GUIBackgroundColor(errorColor))
            {
                EditorGUI.BeginChangeCheck();
                value.x = EditorGUI.FloatField(position, " ", value.x, DRMEditorResources.MiniTextBox);

                if (EditorGUI.EndChangeCheck())
                {
                    property.FindPropertyRelative(propName).vector2Value = value;
                }
            }
        }

        static public void DrawLerpValue(SerializedProperty property, string propStartName, string propEndName, Rect position, string label)
        {
            EditorGUI.LabelField(position, label, EditorStyles.miniLabel);


            Rect dataRect = position;
            dataRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;
            dataRect.width *= 0.5f;


            Vector2 valueStart = property.FindPropertyRelative(propStartName).vector2Value;
            Vector2 valueEnd = property.FindPropertyRelative(propEndName).vector2Value;

            Color errorColor = (valueStart.x == valueEnd.x && valueStart.x == 0) ? Color.red : Color.white;


            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;


            //Start
            Rect minRect = dataRect;
            using (new EditorGUIHelper.EditorGUIUtilityLabelWidth(EditorStyles.label.CalcSize(new GUIContent("Start")).x))     //yes, 'Mim' and not 'Min'. 'm' gives just a little bit more space than 'n'
            {
                EditorGUI.LabelField(minRect, "Start", EditorStyles.miniLabel);
                using (new EditorGUIHelper.GUIBackgroundColor(errorColor))
                {
                    EditorGUI.BeginChangeCheck();
                    valueStart.x = EditorGUI.FloatField(minRect, " ", valueStart.x, DRMEditorResources.MiniTextBox);

                    if (EditorGUI.EndChangeCheck())
                        property.FindPropertyRelative(propStartName).vector2Value = valueStart;
                }
            }


            //End
            Rect maxRect = minRect;
            maxRect.xMin = maxRect.xMax;
            maxRect.width = dataRect.width;

            using (new EditorGUIHelper.EditorGUIUtilityLabelWidth(EditorStyles.label.CalcSize(new GUIContent(" End")).x))
            {
                EditorGUI.LabelField(maxRect, " End", EditorStyles.miniLabel);
                using (new EditorGUIHelper.GUIBackgroundColor(errorColor))
                {
                    EditorGUI.BeginChangeCheck();
                    valueEnd.x = EditorGUI.FloatField(maxRect, " ", valueEnd.x, DRMEditorResources.MiniTextBox);

                    if (EditorGUI.EndChangeCheck())
                        property.FindPropertyRelative(propEndName).vector2Value = valueEnd;

                }
            }



            EditorGUI.indentLevel = indentLevel;
        }

        static public void DrawMinMax(SerializedProperty property, string propName, Rect position, string label, bool miniLabel, bool checkForZero)
        {
            EditorGUI.LabelField(position, label, miniLabel ? EditorStyles.miniLabel : EditorStyles.label);


            Rect dataRect = position;
            dataRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;
            dataRect.width *= 0.5f;


            Vector2 value = property.FindPropertyRelative(propName).vector2Value;

            Color errorColor = checkForZero ? ((value.x == value.y && value.x == 0) ? Color.red : Color.white) : Color.white;


            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;


            //Min
            Rect minRect = dataRect;
            using (new EditorGUIHelper.EditorGUIUtilityLabelWidth(EditorStyles.label.CalcSize(new GUIContent("Mim")).x))     //yes, 'Mim' and not 'Min'. 'm' gives just a little bit more space than 'n'
            {

                EditorGUI.LabelField(minRect, "Min", EditorStyles.miniLabel);
                using (new EditorGUIHelper.GUIBackgroundColor(errorColor))
                {
                    EditorGUI.BeginChangeCheck();
                    value.x = EditorGUI.FloatField(minRect, " ", value.x, DRMEditorResources.MiniTextBox);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.FindPropertyRelative(propName).vector2Value = value;
                    }
                }
            }


            //Max
            Rect maxRect = minRect;
            maxRect.xMin = maxRect.xMax;
            maxRect.width = dataRect.width;
            using (new EditorGUIHelper.EditorGUIUtilityLabelWidth(EditorStyles.label.CalcSize(new GUIContent(" Max")).x))
            {
                EditorGUI.LabelField(maxRect, "  Max", EditorStyles.miniLabel);
                using (new EditorGUIHelper.GUIBackgroundColor(errorColor))
                {
                    EditorGUI.BeginChangeCheck();
                    value.y = EditorGUI.FloatField(maxRect, " ", value.y, DRMEditorResources.MiniTextBox);

                    if (EditorGUI.EndChangeCheck())
                        property.FindPropertyRelative(propName).vector2Value = value;
                }
            }

            EditorGUI.indentLevel = indentLevel;
        }
    }
}
