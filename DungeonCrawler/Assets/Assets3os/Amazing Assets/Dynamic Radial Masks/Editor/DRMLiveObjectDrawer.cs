using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


using AmazingAssets.DynamicRadialMasks;

namespace AmazingAssets.DynamicRadialMasksEditor
{
    [CustomPropertyDrawer(typeof(DRMLiveObject))]
    public class DRMLiveObjectDrawer : PropertyDrawer
    {
        EditorContext.Context state = EditorContext.Context.None;


        static SerializedObject serializedObject;
        static SerializedProperty serializedProperty;



        static List<GUIStyle> customStyles;
        static string[] customStylesNames;
        int customStyleID;



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CatchEditorContext(property);


            position.height = 18;
            EditorGUI.BeginProperty(position, label, property);
            {
                if(property.isExpanded)
                    GUI.Box(new Rect(position.xMin, position.yMin, position.width, GetPropertyHeight(property, label)), string.Empty, EditorStyles.helpBox);


                //Draw title background
                GUI.Box(position, string.Empty, DRMEditorResources.GuiStyleAdjustmentHeader);


                //Foldout
                property.isExpanded = EditorGUI.Foldout(new Rect(position.xMin + 18, position.yMin + 1, position.width - 80, position.height), property.isExpanded, GUIContent.none, true);
                EditorGUI.LabelField(new Rect(position.xMin + 20, position.yMin, position.width - 80, position.height), label, EditorStyles.boldLabel);


                //Draw menu
                Rect menuRect = new Rect(position.xMax - 20, position.yMin + 2, 16, 16);
                if (GUI.Button(menuRect, DRMEditorResources.IconMenu, DRMEditorResources.GuiStyleIconButton))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Copy"), false, OnContextMenuClick, EditorContext.Context.Copy);
                    if (EditorContext.DRMObject == null)
                        menu.AddDisabledItem(new GUIContent("Paste"));
                    else
                        menu.AddItem(new GUIContent("Paste"), false, OnContextMenuClick, EditorContext.Context.Paste);
                    menu.AddItem(new GUIContent("Reset"), false, OnContextMenuClick, EditorContext.Context.Reset);

                    menu.DropDown(menuRect);
                }



                if (property.isExpanded)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        #region Draw Property popup
                        position.width -= 4;
                        position.y += 26;
                        position.height = 16;
                        DynamicRadialMasks.Enum.MaskShape maskShape = (DynamicRadialMasks.Enum.MaskShape)property.FindPropertyRelative("maskShape").enumValueIndex;
                        bool allProperties = property.FindPropertyRelative("displayAllProperties").boolValue;

                        EditorGUI.BeginChangeCheck();
                        allProperties = EditorGUI.ToggleLeft(new Rect(position.xMin, position.yMin, UnityEditor.EditorGUIUtility.labelWidth, position.height), DRMEditorResources.guiContentAllProperties, allProperties);
                        if (EditorGUI.EndChangeCheck())
                        {
                            property.FindPropertyRelative("displayAllProperties").boolValue = allProperties;
                        }

                        Rect enumDrawRect = position;
                        enumDrawRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;
                        if (allProperties)
                        {
                            using (new EditorGUIHelper.GUIEnabled(false))
                            {
                                GUI.Button(enumDrawRect, "All Properties", EditorStyles.popup);
                            }
                        }
                        else
                        {
                            if (GUI.Button(enumDrawRect, maskShape.ToString(), EditorStyles.popup))
                            {
                                serializedObject = property.serializedObject;
                                serializedProperty = property;

                                PopupWindow.Show(enumDrawRect, new DRMShapesEnumPopupWindow((int)maskShape, Callback));
                            }
                        }
                        #endregion


                        position.y += 6;
                        DrawLifeProperty(ref position, property);


                        position.y += 20;
                        if (allProperties)
                        {
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);                            

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("ringCount"), new GUIContent("Ring Count"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("phaseSpeed"), new GUIContent("Phase Speed"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("phaseSpeed"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("frequency"), new GUIContent("Frequency"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("frequency"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                            EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                            position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                        }
                        else
                        {
                            switch ((DynamicRadialMasks.Enum.MaskShape)property.FindPropertyRelative("maskShape").enumValueIndex)
                            {
                                case DynamicRadialMasks.Enum.MaskShape.Torus:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));                                    
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Tube:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.HeightField:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Dot:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Shockwave:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Sonar:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("ringCount"), new GUIContent("Ring Count"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Rings:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("edgeSize"), new GUIContent("Edge Size"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("ringCount"), new GUIContent("Ring Count"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;

                                case DynamicRadialMasks.Enum.MaskShape.Ripple:
                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intensity"), new GUIContent("Intensity"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("phaseSpeed"), new GUIContent("Phase Speed"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("phaseSpeed"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("frequency"), new GUIContent("Frequency"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("frequency"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("smooth"), new GUIContent("Smooth"));
                                    position.y += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

                                    EditorGUI.PropertyField(position, property.FindPropertyRelative("noise"), new GUIContent("Noise"));
                                    break;


                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 18;


            if (property.isExpanded)
            {
                height += 50;

                height += GetPropertyDrawersHeight(property);
            }

            return height;
        }

        float GetPropertyDrawersHeight(SerializedProperty property)
        {
            float height = 5;

            if (property.FindPropertyRelative("displayAllProperties").boolValue)
            {
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("phaseSpeed"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("frequency"), true);
                height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);

            }
            else
            {
                switch ((DynamicRadialMasks.Enum.MaskShape)property.FindPropertyRelative("maskShape").enumValueIndex)
                {
                    case DynamicRadialMasks.Enum.MaskShape.Torus:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Tube:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                        }
                        break;
                    case DynamicRadialMasks.Enum.MaskShape.HeightField:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Dot:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Shockwave:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Sonar:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Rings:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("edgeSize"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("ringCount"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    case DynamicRadialMasks.Enum.MaskShape.Ripple:
                        {
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("radius"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("intensity"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("noise"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("phaseSpeed"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("frequency"), true);
                            height += DRMLivePropertyDrawer.GetPropertyHeight(property.FindPropertyRelative("smooth"), true);
                        }
                        break;

                    default: break;
                }
            }

            return height;
        }


        float DrawLifeProperty(ref Rect position, SerializedProperty property)
        {
            position.y += 21;
            position.height = 16;


            // Draw label     
            DRMLivePropertyDrawer.DrawMinMax(property, "lifeLength", position, "Life Length", false, true);


            return 18;
        }

        void OnContextMenuClick(object obj)
        {
            state = (EditorContext.Context)obj;
        }

        void CatchEditorContext(SerializedProperty property)
        {
            switch (state)
            {
                case EditorContext.Context.None:
                    break;

                case EditorContext.Context.Copy:
                    EditorContext.Copy(property);
                    break;

                case EditorContext.Context.Paste:
                    EditorContext.Paste(property);
                    break;

                case EditorContext.Context.Reset:
                    EditorContext.Reset(property);
                    break;
            }

            //Reset
            state = EditorContext.Context.None;
        }

        void Callback(int value)
        {
            serializedProperty.FindPropertyRelative("maskShape").enumValueIndex = value;

            serializedObject.ApplyModifiedProperties();
        }


        public static class EditorContext
        {
            public enum Context { None, Copy, Paste, Reset };


            public static DRMLiveObject DRMObject;

            public static void Copy(SerializedProperty property)
            {
                DRMObject = new DRMLiveObject();

                DRMObject.maskShape = (DynamicRadialMasks.Enum.MaskShape)property.FindPropertyRelative("maskShape").enumValueIndex;
                DRMObject.displayAllProperties = property.FindPropertyRelative("displayAllProperties").boolValue;

                DRMObject.lifeLength = property.FindPropertyRelative("lifeLength").vector2Value;

                DRMObject.radius = MemberCopy(property.FindPropertyRelative("radius"));
                DRMObject.intensity = MemberCopy(property.FindPropertyRelative("intensity"));
                DRMObject.noise = MemberCopy(property.FindPropertyRelative("noise"));
                DRMObject.edgeSize = MemberCopy(property.FindPropertyRelative("edgeSize"));
                DRMObject.ringCount = MemberCopy(property.FindPropertyRelative("ringCount"));
                DRMObject.phaseSpeed = MemberCopy(property.FindPropertyRelative("phaseSpeed"));
                DRMObject.frequency = MemberCopy(property.FindPropertyRelative("frequency"));
                DRMObject.smooth = MemberCopy(property.FindPropertyRelative("smooth"));
            }

            public static void Paste(SerializedProperty property)
            {
                if (DRMObject == null || property == null)
                    return;

                property.FindPropertyRelative("maskShape").enumValueIndex = (int)DRMObject.maskShape;
                property.FindPropertyRelative("displayAllProperties").boolValue = DRMObject.displayAllProperties;

                property.FindPropertyRelative("lifeLength").vector2Value = DRMObject.lifeLength;

                MemberCopy(ref property, DRMObject.radius, "radius");
                MemberCopy(ref property, DRMObject.intensity, "intensity");
                MemberCopy(ref property, DRMObject.noise, "noise");
                MemberCopy(ref property, DRMObject.edgeSize, "edgeSize");
                MemberCopy(ref property, DRMObject.ringCount, "ringCount");
                MemberCopy(ref property, DRMObject.phaseSpeed, "phaseSpeed");
                MemberCopy(ref property, DRMObject.frequency, "frequency");
                MemberCopy(ref property, DRMObject.smooth, "smooth");
            }

            public static void Reset(SerializedProperty property)
            {
                if (property == null)
                    return;

                DRMLiveObject obj = new DRMLiveObject();

                property.FindPropertyRelative("maskShape").boolValue = true;
                property.FindPropertyRelative("maskShape").enumValueIndex = (int)obj.maskShape;

                property.FindPropertyRelative("lifeLength").vector2Value = obj.lifeLength;

                MemberCopy(ref property, obj.radius, "radius");
                MemberCopy(ref property, obj.intensity, "intensity");
                MemberCopy(ref property, obj.noise, "noise");
                MemberCopy(ref property, obj.edgeSize, "edgeSize");
                MemberCopy(ref property, obj.ringCount, "ringCount");
                MemberCopy(ref property, obj.phaseSpeed, "phaseSpeed");
                MemberCopy(ref property, obj.frequency, "frequency");
                MemberCopy(ref property, obj.smooth, "smooth");
            }

            static DRMLiveProperty MemberCopy(SerializedProperty property)
            {
                DRMLiveProperty prop = new DRMLiveProperty();

                prop.evolutionType = (DRMLiveProperty.Enum.AnimationType)property.FindPropertyRelative("evolutionType").enumValueIndex;
                prop.startValue = property.FindPropertyRelative("startValue").vector2Value;
                prop.endValue = property.FindPropertyRelative("endValue").vector2Value;
                prop.curve = property.FindPropertyRelative("curve").animationCurveValue;

                return prop;
            }

            static void MemberCopy(ref SerializedProperty property, DRMLiveProperty objectProperty, string name)
            {
                property.FindPropertyRelative(name).FindPropertyRelative("evolutionType").enumValueIndex = (int)objectProperty.evolutionType;
                property.FindPropertyRelative(name).FindPropertyRelative("startValue").vector2Value = objectProperty.startValue;
                property.FindPropertyRelative(name).FindPropertyRelative("endValue").vector2Value = objectProperty.endValue;
                property.FindPropertyRelative(name).FindPropertyRelative("curve").animationCurveValue = objectProperty.curve;
            }
        }
    }
}