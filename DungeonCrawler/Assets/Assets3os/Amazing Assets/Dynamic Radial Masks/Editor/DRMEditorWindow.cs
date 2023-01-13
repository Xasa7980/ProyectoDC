using System.IO;

using UnityEngine;
using UnityEditor;


using AmazingAssets.DynamicRadialMasks;

namespace AmazingAssets.DynamicRadialMasksEditor
{
    public class DRMEditorWindow : EditorWindow
    {
        enum FILE_EXTENTION { cginc, shadersubgraph, asset };


        DynamicRadialMasks.Enum.MaskShape maskShape;
        int maskCount;
        DynamicRadialMasks.Enum.MaskType maskType;
        DynamicRadialMasks.Enum.MaskBlendMode maskBlendMode;
        int maskID;
        DynamicRadialMasks.Enum.MaskScope maskScope;
        

         
         
        [MenuItem("Assets/Create/Shader/" + AssetInfo.assetName, false, 1701)]
        static public void ShowWindowFromAsset()
        {
            DRMEditorWindow window = (DRMEditorWindow)EditorWindow.GetWindow(typeof(DRMEditorWindow));
            window.titleContent = new GUIContent(AssetInfo.assetName);

            window.minSize = new Vector2(400, 190);
            window.maxSize = new Vector2(400, 190);

            window.ShowUtility();
        }

        [MenuItem("Window/Amazing Assets/Dynamic Radial Masks", false, 1702)]
        static public void ShowWindowFromMainMenu()
        {
            ShowWindowFromAsset();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            DrawMainWindow();
            EditorGUILayout.EndVertical();
        }


        void DrawMainWindow()
        {
            EditorGUILayout.LabelField("Shape");
            Rect shapesDrawRect = GUILayoutUtility.GetLastRect();
            shapesDrawRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;
            if (GUI.Button(shapesDrawRect, maskShape.ToString(), EditorStyles.popup))
            {
                PopupWindow.Show(shapesDrawRect, new DRMShapesEnumPopupWindow((int)maskShape, Callback));
            }


            maskCount = EditorGUILayout.IntSlider("Count", maskCount, 1, 200);
            maskType = (DynamicRadialMasks.Enum.MaskType)EditorGUILayout.EnumPopup("Type", maskType);
            maskBlendMode = (DynamicRadialMasks.Enum.MaskBlendMode)EditorGUILayout.EnumPopup("Blend Mode", maskBlendMode);
            maskID = EditorGUILayout.IntSlider("ID", maskID, 1, 32);
            maskScope = (DynamicRadialMasks.Enum.MaskScope)EditorGUILayout.EnumPopup("Scope", maskScope);


            GUILayout.Space(10);
            using (new EditorGUIHelper.GUIEnabled(false))
            {
                EditorGUILayout.LabelField(" ", " ", EditorStyles.textField, GUILayout.Height(16));

                EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), "Required Instructions Count", EditorStyles.wordWrappedMiniLabel);

                EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), " ", GetInstructionsCount(maskShape, maskCount, maskType, maskBlendMode).ToString("N0"), EditorStyles.wordWrappedMiniLabel);
            }


            GUILayout.Space(12);
            EditorGUILayout.BeginHorizontal("Toolbar");

            if (GUILayout.Button("Copy Path", EditorStyles.toolbarButton))
            {
                //Make sure main CGINC file exists
                string mainCGINCFilePath = CreateCGINCFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);

                TextEditor te = new TextEditor();
                te.text = "\"" + mainCGINCFilePath + "\"";
                te.text = te.text.Replace(Path.DirectorySeparatorChar, '/');
                te.SelectAll();
                te.Copy();
            }

            if (GUILayout.Button("CGINC", EditorStyles.toolbarButton))
            {
                //Make sure main CGINC file exists
                string mainCGINCFilePath = CreateCGINCFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);

                PingObject(mainCGINCFilePath);
            }

            if (GUILayout.Button("Shader Graph", EditorStyles.toolbarButton))
            {
                //Make sure main CGINC file exists
                CreateCGINCFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);


                string filePath = GetDRMFilePath(FILE_EXTENTION.shadersubgraph, maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);

                if (File.Exists(filePath))
                    PingObject(filePath);
                else
                {
                    CreateSubGraphFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope, FILE_EXTENTION.shadersubgraph);

                    AssetDatabase.Refresh();

                    PingObject(filePath);
                }

            }

            if (GUILayout.Button("Amplify Shader Editor", EditorStyles.toolbarButton))
            {
                //Make sure main CGINC file exists
                CreateCGINCFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);


                string filePath = GetDRMFilePath(FILE_EXTENTION.asset, maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope);

                if (File.Exists(filePath))
                    PingObject(filePath);
                else
                {
                    CreateSubGraphFile(maskShape, maskCount, maskType, maskBlendMode, maskID, maskScope, FILE_EXTENTION.asset);

                    AssetDatabase.Refresh();

                    PingObject(filePath);
                }
            }

            EditorGUILayout.EndHorizontal();

        }

        string GetTemplateFileLocation(FILE_EXTENTION _Extention, DynamicRadialMasks.Enum.MaskShape _MaskShape, DynamicRadialMasks.Enum.MaskType _MaskType, DynamicRadialMasks.Enum.MaskBlendMode _MaskBlendMode)
        {
            string fileID = string.Empty;
            switch (_Extention)
            {
                case FILE_EXTENTION.cginc: fileID = _MaskShape.ToString(); break;
                case FILE_EXTENTION.shadersubgraph: fileID = "UnityShaderGraph"; break;
                case FILE_EXTENTION.asset: fileID = "AmplifyShaderEditor"; break;
            }

            string fileName = string.Format("Template_{0}_{1}_{2}.txt", fileID, _MaskType, _MaskBlendMode);

            string path = Path.Combine(EditorUtilities.GetThisAssetFolderPath(), "Shaders");
            path = Path.Combine(path, "Templates");
            path = Path.Combine(path, fileName);

            return path;
        }
        string GetDRMFilePath(FILE_EXTENTION _Extention, DynamicRadialMasks.Enum.MaskShape _MaskShape, int _ShapeCount, DynamicRadialMasks.Enum.MaskType _MaskType, DynamicRadialMasks.Enum.MaskBlendMode _MaskBlendMode, int _MaskID, DynamicRadialMasks.Enum.MaskScope _MaskScope)
        {
            string fileName = string.Format("DynamicRadialMasks_{0}_{1}_{2}_{3}_ID{4}_{5}.{6}", _MaskShape, _ShapeCount, _MaskType, _MaskBlendMode, _MaskID, _MaskScope, _Extention);

            string subFolderName = string.Empty;
            switch (_Extention)
            {
                case FILE_EXTENTION.cginc: subFolderName = "CGINC"; break;
                case FILE_EXTENTION.shadersubgraph: subFolderName = "Unity Shader Graph"; break;
                case FILE_EXTENTION.asset: subFolderName = "Amplify Shader Editor"; break;
            }

            string path = Path.Combine(EditorUtilities.GetThisAssetFolderPath(), "Shaders");
            path = Path.Combine(path, subFolderName);
            path = Path.Combine(path, _MaskShape.ToString());
            path = Path.Combine(path, fileName);

            return path;
        }


        string CreateCGINCFile(DynamicRadialMasks.Enum.MaskShape _MaskShape, int _ShapeCount, DynamicRadialMasks.Enum.MaskType _MaskType, DynamicRadialMasks.Enum.MaskBlendMode _MaskBlendMode, int _MaskID, DynamicRadialMasks.Enum.MaskScope _MaskScope)
        {
            string filePath = GetDRMFilePath(FILE_EXTENTION.cginc, _MaskShape, _ShapeCount, _MaskType, _MaskBlendMode, _MaskID, _MaskScope);
            if (File.Exists(filePath))
                return filePath;


            string templateFileLocation = GetTemplateFileLocation(FILE_EXTENTION.cginc, _MaskShape, _MaskType, _MaskBlendMode);

            string[] templateFileAllLines = ReadFileAllLines(templateFileLocation);
            if (templateFileAllLines == null || templateFileAllLines.Length == 0)
                return filePath;


            string[] cgincFile = new string[templateFileAllLines.Length];

            for (int i = 0; i < templateFileAllLines.Length; i++)
            {
                if (templateFileAllLines[i].Contains("#FOR_LOOP#"))
                {
                    if (_ShapeCount > 1)
                        templateFileAllLines[i] = templateFileAllLines[i].Replace("#FOR_LOOP#", "[unroll]");
                    else
                        templateFileAllLines[i] = string.Empty;
                }

                cgincFile[i] = templateFileAllLines[i].Replace("#SHAPE_BIG#", _MaskShape.ToString().ToUpper()).
                                                        Replace("#SHAPE_SMALL#", _MaskShape.ToString()).
                                                        Replace("#ARRAY_LENGTH#", _ShapeCount.ToString()).
                                                        Replace("#TYPE_BIG#", _MaskType.ToString().ToUpper()).
                                                        Replace("#TYPE_SMALL#", _MaskType.ToString()).
                                                        Replace("#BLEND_MODE_BIG#", _MaskBlendMode.ToString().ToUpper()).
                                                        Replace("#BLEND_MODE_SMALL#", _MaskBlendMode.ToString()).
                                                        Replace("#ID#", _MaskID.ToString()).
                                                        Replace("#SCOPE_BIG#", _MaskScope == DynamicRadialMasks.Enum.MaskScope.Local ? "LOCAL" : "GLOBAL").
                                                        Replace("#SCOPE_SMALL#", _MaskScope == DynamicRadialMasks.Enum.MaskScope.Local ? "Local" : "Global").
                                                        Replace("#UNIFORM#", _MaskScope == DynamicRadialMasks.Enum.MaskScope.Local ? string.Empty : "uniform ");
            }



            string saveFolder = Path.GetDirectoryName(filePath);
            if (Directory.Exists(saveFolder) == false)
                Directory.CreateDirectory(saveFolder);

            File.WriteAllLines(filePath, cgincFile);

            AssetDatabase.Refresh();

            return filePath;
        }
        void CreateSubGraphFile(DynamicRadialMasks.Enum.MaskShape _MaskShape, int _ShapeCount, DynamicRadialMasks.Enum.MaskType _MaskType, DynamicRadialMasks.Enum.MaskBlendMode _MaskBlendMode, int _MaskID, DynamicRadialMasks.Enum.MaskScope _MaskScope, FILE_EXTENTION _Extention)
        {
            string templateFileLocation = GetTemplateFileLocation(_Extention, _MaskShape, _MaskType, _MaskBlendMode);

            string[] templateFileAllLines = ReadFileAllLines(templateFileLocation);
            if (templateFileAllLines == null || templateFileAllLines.Length == 0)
            {
                Debug.LogWarning("Template file not found: " + templateFileLocation);
                return;
            }

            string cgincFilePath = GetDRMFilePath(FILE_EXTENTION.cginc, _MaskShape, _ShapeCount, _MaskType, _MaskBlendMode, _MaskID, _MaskScope);
            string cgincFileGUID = AssetDatabase.AssetPathToGUID(cgincFilePath);
            if (string.IsNullOrEmpty(cgincFileGUID))
            {
                Debug.LogWarning("CGINC file not found: " + cgincFilePath);
                return;
            }


            string[] subGraphFile = new string[templateFileAllLines.Length];

            for (int i = 0; i < templateFileAllLines.Length; i++)
            {
                subGraphFile[i] = templateFileAllLines[i].Replace("#SHAPE_BIG#", _MaskShape.ToString().ToUpper()).
                                                       Replace("#SHAPE_SMALL#", _MaskShape.ToString()).
                                                       Replace("#ARRAY_LENGTH#", _ShapeCount.ToString()).
                                                       Replace("#TYPE_BIG#", _MaskType.ToString().ToUpper()).
                                                       Replace("#TYPE_SMALL#", _MaskType.ToString()).
                                                       Replace("#BLEND_MODE_BIG#", _MaskBlendMode.ToString().ToUpper()).
                                                       Replace("#BLEND_MODE_SMALL#", _MaskBlendMode.ToString()).
                                                       Replace("#ID#", _MaskID.ToString()).
                                                       Replace("#SCOPE_BIG#", _MaskScope == DynamicRadialMasks.Enum.MaskScope.Local ? "LOCAL" : "GLOBAL").
                                                       Replace("#SCOPE_SMALL#", _MaskScope == DynamicRadialMasks.Enum.MaskScope.Local ? "Local" : "Global").
                                                       Replace("#CGINC_FILE_GUID#", cgincFileGUID);
            }



            string saveFolder = Path.Combine(EditorUtilities.GetThisAssetFolderPath(), "Shaders");
            saveFolder = Path.Combine(saveFolder, _Extention == FILE_EXTENTION.asset ? "Amplify Shader Editor" : "Unity Shader Graph");
            saveFolder = Path.Combine(saveFolder, _MaskShape.ToString());

            if (Directory.Exists(saveFolder) == false)
                Directory.CreateDirectory(saveFolder);


            string saveLocalFileName = string.Format("DynamicRadialMasks_{0}_{1}_{2}_{3}_ID{4}_{5}.{6}", _MaskShape, _ShapeCount, _MaskType, _MaskBlendMode, _MaskID, _MaskScope, _Extention);
            saveLocalFileName = Path.Combine(saveFolder, saveLocalFileName);


            WriteFileAllLines(saveLocalFileName, subGraphFile);
        }


        string[] ReadFileAllLines(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                return null;

            return File.ReadAllLines(filePath);
        }
        void WriteFileAllLines(string filePath, string[] fileData)
        {
            try
            {
                File.WriteAllLines(filePath, fileData);
            }
            catch
            {
                Debug.LogWarning("Can not create file: " + Path.GetFileName(filePath) + "\nReason: Absolute file path length exceeds 259 character limit.\nSolution: Move project closer to the system root directory, making the path shorter.\n");
            }

        }
        void PingObject(string path)
        {
            // Load object
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            if (obj == null)
            {
                //Try folder
                obj = AssetDatabase.LoadAssetAtPath(Path.GetDirectoryName(path), typeof(UnityEngine.Object));

                if (obj == null)
                    return;
            }


            // Select the object in the project folder
            Selection.activeObject = obj;

            // Also flash the folder yellow to highlight it
            UnityEditor.EditorGUIUtility.PingObject(obj);
        }


        void Callback(int value)
        {
            maskShape = (DynamicRadialMasks.Enum.MaskShape)value;

            Repaint();
        }


        int GetInstructionsCount(DynamicRadialMasks.Enum.MaskShape maskShape, int maskCount, DynamicRadialMasks.Enum.MaskType maskType, DynamicRadialMasks.Enum.MaskBlendMode maskBlendMode)
        {
            int iCount = 0;

            switch (maskShape)
            {
                case Enum.MaskShape.Torus:
                    {
                        if(maskType == Enum.MaskType.Advanced)
                        {
                            if (maskBlendMode == Enum.MaskBlendMode.Additive)
                                iCount = 14;
                            else
                                iCount = 16;
                        }
                        else
                        {
                            if (maskBlendMode == Enum.MaskBlendMode.Additive)
                                iCount = 11;
                            else
                                iCount = 13;
                        }
                    }
                    break;

                case Enum.MaskShape.Tube:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 12;
                        else
                            iCount = 14;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 12;
                        else
                            iCount = 14;
                    }
                    break;

                case Enum.MaskShape.HeightField:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 13;
                        else
                            iCount = 15;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 10;
                        else
                            iCount = 12;
                    }
                    break;

                case Enum.MaskShape.Dot:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 6;
                        else
                            iCount = 8;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 6;
                        else
                            iCount = 8;
                    }
                    break;

                case Enum.MaskShape.Shockwave:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 13;
                        else
                            iCount = 15;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 10;
                        else
                            iCount = 12;
                    }
                    break;

                case Enum.MaskShape.Sonar:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 15;
                        else
                            iCount = 17;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 12;
                        else
                            iCount = 14;
                    }
                    break;

                case Enum.MaskShape.Rings:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 17;
                        else
                            iCount = 19;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 14;
                        else
                            iCount = 16;
                    }
                    break;

                case Enum.MaskShape.Ripple:
                    if (maskType == Enum.MaskType.Advanced)
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 20;
                        else
                            iCount = 22;
                    }
                    else
                    {
                        if (maskBlendMode == Enum.MaskBlendMode.Additive)
                            iCount = 17;
                        else
                            iCount = 19;
                    }
                    break;

                default:
                    break;
            }


            return iCount * maskCount;
        }
    }
}