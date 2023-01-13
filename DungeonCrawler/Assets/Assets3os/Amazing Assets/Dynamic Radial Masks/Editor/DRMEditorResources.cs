using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;

using AmazingAssets.DynamicRadialMasks;


namespace AmazingAssets.DynamicRadialMasksEditor
{
    static public class DRMEditorResources
    {
        static public string[] shapeNames = System.Enum.GetNames(typeof(DynamicRadialMasks.Enum.MaskShape));

        static public GUIContent guiContentAllProperties = new GUIContent("All Properties", "Display all properties or only for selected shape.");


        static GUIStyle guiStyleIconButton;
        static public GUIStyle GuiStyleIconButton
        {
            get
            {
                if (guiStyleIconButton == null)
                {
                    guiStyleIconButton = new GUIStyle("iconButton");
                    guiStyleIconButton.alignment = TextAnchor.MiddleCenter;
                    guiStyleIconButton.fontStyle = FontStyle.Normal;
                }

                return guiStyleIconButton;
            }
        }
        static GUIStyle guiStyleFoldout;
        static public GUIStyle GuiStyleFoldout
        {
            get
            {
                if (guiStyleFoldout == null)
                {
                    guiStyleFoldout = new GUIStyle("ProgressBarBar");
                    guiStyleFoldout.fontStyle = FontStyle.Bold;
                    guiStyleFoldout.alignment = TextAnchor.MiddleLeft;
                }

                return guiStyleFoldout;
            }
        }

        static GUIStyle miniTextBox;
        static public GUIStyle MiniTextBox
        {
            get
            {
                if (miniTextBox == null)
                {
                    miniTextBox = new GUIStyle(EditorStyles.helpBox);

                    miniTextBox.alignment = TextAnchor.MiddleLeft;
                }

                return miniTextBox;
            }
        }

        static GUIStyle labelMiniCenter;
        static public GUIStyle LabelMiniCenter
        {
            get
            {
                if (labelMiniCenter == null)
                {
                    labelMiniCenter = new GUIStyle(EditorStyles.miniLabel);

                    labelMiniCenter.alignment = TextAnchor.MiddleCenter;
                }

                return labelMiniCenter;
            }
        }

        static GUIStyle labelMiniCenterBold;
        static public GUIStyle LabelMiniCenterBold
        {
            get
            {
                if (labelMiniCenterBold == null)
                {
                    labelMiniCenterBold = new GUIStyle(EditorStyles.miniBoldLabel);

                    labelMiniCenterBold.alignment = TextAnchor.MiddleCenter;
                }

                return labelMiniCenterBold;
            }
        }

        static GUIStyle guiStyleHeader;
        static public GUIStyle GuiStyleHeader
        {
            get
            {
                if (guiStyleHeader == null)
                {
                    guiStyleHeader = new GUIStyle(EditorStyles.foldout);
                    guiStyleHeader.fontStyle = FontStyle.Bold;
                }

                return guiStyleHeader;
            }
        }

        static GUIStyle guiStyleAdjustmentHeader;
        static internal GUIStyle GuiStyleAdjustmentHeader
        {
            get
            {
                if (guiStyleAdjustmentHeader == null)
                    guiStyleAdjustmentHeader = new GUIStyle("RL Header");

                return guiStyleAdjustmentHeader;
            }
        }
                      
        static Texture iconMenu;
        static public Texture IconMenu
        {
            get
            {
                if (iconMenu == null)
                    iconMenu = UnityEditor.EditorGUIUtility.IconContent("_Menu").image;

                return iconMenu;
            }
        }

         

        static Texture2D[] iconShapeTextures;
        static public Texture2D IconShapeTexture(int index)
        {
            if(iconShapeTextures == null || iconShapeTextures.Length == 0 || iconShapeTextures.Any(c => c == null))
            {
                iconShapeTextures = new Texture2D[shapeNames.Length];

                for (int i = 0; i < shapeNames.Length; i++)
                {
                    if(iconShapeTextures[i] == null)
                        iconShapeTextures[i] = new Texture2D(2, 2);


                    string iconPath = Path.Combine(EditorUtilities.GetThisAssetFolderPath(), "Editor", "Icons", $"{shapeNames[i]}");

                    iconShapeTextures[i].LoadImage(File.ReadAllBytes(iconPath));
                }
            }

            return iconShapeTextures[index];
        }
    }
}
