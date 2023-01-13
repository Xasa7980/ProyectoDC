using System;
using System.Collections.Generic;

using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    public class Enum
    {
        public enum MaskShape { Torus, Tube, HeightField, Dot, Shockwave, Sonar, Rings, Ripple };
        public enum MaskPropertyName { Position, Radius, Intensity, NoiseStrength, EdgeSize, RingCount, Phase, Frequency, Smooth };
        public enum MaskType { Advanced, Simple }
        public enum MaskBlendMode { Additive, Normalized }
        public enum MaskScope { Local, Global }
    }



    [ExecuteAlways]
    public class DRMController : MonoBehaviour
    {
        public class Enum
        {
            public enum ScriptUpdateMethod { Update, FixedUpdate, Custom }
        }


#region Shader Data       
        [HideInInspector] public Vector4[] shaderData_Position;
        [HideInInspector] public float[] shaderData_Radius;
        [HideInInspector] public float[] shaderData_Intensity;
        [HideInInspector] public float[] shaderData_NoiseStrength;
        [HideInInspector] public float[] shaderData_EdgeSize;
        [HideInInspector] public float[] shaderData_RingCount;
        [HideInInspector] public float[] shaderData_Phase;
        [HideInInspector] public float[] shaderData_Frequency;
        [HideInInspector] public float[] shaderData_Smooth;


        int materialPropertyID_Position = 0;
        int materialPropertyID_Radius = 0;
        int materialPropertyID_Intensity = 0;
        int materialPropertyID_NoiseStrength = 0;
        int materialPropertyID_EdgeSize = 0;
        int materialPropertyID_RingCount = 0;
        int materialPropertyID_Phase = 0;
        int materialPropertyID_Frequency = 0;
        int materialPropertyID_Smooth = 0;
#endregion


#region Settings        
        [DRMShapesEnum]
        public DynamicRadialMasks.Enum.MaskShape shape;
        DynamicRadialMasks.Enum.MaskShape previousShape;

        [Range(1, 200)]
        public int count = 16;
        int previousCount;

        public DynamicRadialMasks.Enum.MaskType type;
        DynamicRadialMasks.Enum.MaskType previousType;

        public DynamicRadialMasks.Enum.MaskBlendMode blendMode;
        DynamicRadialMasks.Enum.MaskBlendMode previousBlendMode;

        [Range(1, 32)]
        public int ID = 1;
        int previousID;

        public DynamicRadialMasks.Enum.MaskScope scope;
        DynamicRadialMasks.Enum.MaskScope previousScope;

        [HideInInspector]
        public Material[] materials;

        [Space]
        [HideInInspector] public Enum.ScriptUpdateMethod updateMethod;
        [HideInInspector] public bool drawInEditor = false;
#endregion
       



        private void OnDisable()
        {
            ResetShaderData();

            Update();
        }

        void Start()
        {
            Initialize();
        }

        void Update()
        {            
            //Force update in editor
            if ((Application.isEditor && Application.isPlaying == false) || updateMethod == Enum.ScriptUpdateMethod.Update)
            {
                UpdateShaderData();
            }
        }

        void FixedUpdate()
        {
            if (updateMethod == Enum.ScriptUpdateMethod.FixedUpdate)
            {
                UpdateShaderData();
            }
        }


        public void UpdateShaderData()
        {
            if (previousCount != count ||
                previousShape != shape ||
                previousType != type ||
                previousBlendMode != blendMode ||
                previousID != ID ||
                previousScope != scope)
            {
                ResetShaderData();
                UpdateShaderData(previousScope);

                Initialize();
            }

            UpdateShaderData(scope);
        }
        public void ResetShaderData()
        {
            int arrayLength = shaderData_Position == null ? 0 : shaderData_Position.Length;

            Array.Clear(shaderData_Position, 0, arrayLength);
            Array.Clear(shaderData_Radius, 0, arrayLength);
            Array.Clear(shaderData_Intensity, 0, arrayLength);
            Array.Clear(shaderData_EdgeSize, 0, arrayLength);
            Array.Clear(shaderData_RingCount, 0, arrayLength);
            Array.Clear(shaderData_Phase, 0, arrayLength);
            Array.Clear(shaderData_Frequency, 0, arrayLength);
            Array.Clear(shaderData_Smooth, 0, arrayLength);
            Array.Clear(shaderData_NoiseStrength, 0, arrayLength);
        }
        public float GetMaskValue(Vector3 position)
        {
            switch (shape)
            {
                case DynamicRadialMasks.Enum.MaskShape.Torus:
                    return DRMUtility.CalculateValue_Torus(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);

                case DynamicRadialMasks.Enum.MaskShape.Tube:
                    return DRMUtility.CalculateValue_Tube(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize);

                case DynamicRadialMasks.Enum.MaskShape.HeightField:
                    return DRMUtility.CalculateValue_HeightField(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);

                case DynamicRadialMasks.Enum.MaskShape.Dot:
                    return DRMUtility.CalculateValue_Dot(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity);

                case DynamicRadialMasks.Enum.MaskShape.Shockwave:
                    return DRMUtility.CalculateValue_Shockwave(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);

                case DynamicRadialMasks.Enum.MaskShape.Sonar:
                    return DRMUtility.CalculateValue_Sonar(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize, shaderData_RingCount, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);

                case DynamicRadialMasks.Enum.MaskShape.Rings:
                    return DRMUtility.CalculateValue_Rings(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_EdgeSize, shaderData_RingCount, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);

                case DynamicRadialMasks.Enum.MaskShape.Ripple:
                    return DRMUtility.CalculateValue_Ripple(position, count, type, blendMode, shaderData_Position, shaderData_Radius, shaderData_Intensity, shaderData_Phase, shaderData_Frequency, shaderData_Smooth, type == DynamicRadialMasks.Enum.MaskType.Simple);


                default:
                    return 0;
            }
        }


        void Initialize()
        {
            if (count < 1)
                count = 1;


            shaderData_Position = new Vector4[count];
            shaderData_Radius = new float[count];
            shaderData_Intensity = new float[count];
            shaderData_NoiseStrength = new float[count];
            shaderData_EdgeSize = new float[count];
            shaderData_RingCount = new float[count];
            shaderData_Phase = new float[count];
            shaderData_Frequency = new float[count];
            shaderData_Smooth = new float[count];

            ResetShaderData();


            previousCount = count;
            previousShape = shape;
            previousType = type;
            previousBlendMode = blendMode;
            previousScope = scope;
            previousID = ID;


            GenerateShaderPropertyIDs();
        }
        void GenerateShaderPropertyIDs()
        {
            materialPropertyID_Position = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Position));
            materialPropertyID_Radius = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Radius));
            materialPropertyID_Intensity = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Intensity));
            materialPropertyID_NoiseStrength = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.NoiseStrength));
            materialPropertyID_EdgeSize = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.EdgeSize));
            materialPropertyID_RingCount = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.RingCount));
            materialPropertyID_Phase = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Phase));
            materialPropertyID_Frequency = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Frequency));
            materialPropertyID_Smooth = Shader.PropertyToID(DRMUtility.GetMaterialPropertyName(shape, count, type, blendMode, ID, scope, DynamicRadialMasks.Enum.MaskPropertyName.Smooth));
        }
        void UpdateShaderData(DynamicRadialMasks.Enum.MaskScope updateScope)
        {
            //During project building propertyIDs may be not generated correctly and build fails
            if (materialPropertyID_Position == 0 || materialPropertyID_Radius == 0 || materialPropertyID_Intensity == 0)
                GenerateShaderPropertyIDs();
            if (materialPropertyID_Position == 0 || materialPropertyID_Radius == 0 || materialPropertyID_Intensity == 0)
                return;


            if (updateScope == DynamicRadialMasks.Enum.MaskScope.Global)
            {
                Shader.SetGlobalVectorArray(materialPropertyID_Position, shaderData_Position);
                Shader.SetGlobalFloatArray(materialPropertyID_Radius, shaderData_Radius);
                Shader.SetGlobalFloatArray(materialPropertyID_Intensity, shaderData_Intensity);
                Shader.SetGlobalFloatArray(materialPropertyID_NoiseStrength, shaderData_NoiseStrength);
                Shader.SetGlobalFloatArray(materialPropertyID_EdgeSize, shaderData_EdgeSize);
                Shader.SetGlobalFloatArray(materialPropertyID_RingCount, shaderData_RingCount);
                Shader.SetGlobalFloatArray(materialPropertyID_Phase, shaderData_Phase);
                Shader.SetGlobalFloatArray(materialPropertyID_Frequency, shaderData_Frequency);
                Shader.SetGlobalFloatArray(materialPropertyID_Smooth, shaderData_Smooth);
            }
            else if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;

                    materials[i].SetVectorArray(materialPropertyID_Position, shaderData_Position);
                    materials[i].SetFloatArray(materialPropertyID_Radius, shaderData_Radius);
                    materials[i].SetFloatArray(materialPropertyID_Intensity, shaderData_Intensity);
                    materials[i].SetFloatArray(materialPropertyID_NoiseStrength, shaderData_NoiseStrength);
                    materials[i].SetFloatArray(materialPropertyID_EdgeSize, shaderData_EdgeSize);
                    materials[i].SetFloatArray(materialPropertyID_RingCount, shaderData_RingCount);
                    materials[i].SetFloatArray(materialPropertyID_Phase, shaderData_Phase);
                    materials[i].SetFloatArray(materialPropertyID_Frequency, shaderData_Frequency);
                    materials[i].SetFloatArray(materialPropertyID_Smooth, shaderData_Smooth);
                }
            }
        }



        void OnDrawGizmos()
        {
            if (drawInEditor)
            {
                for (int i = 0; i < shaderData_Position.Length; i++)
                {
                    Gizmos.color = Color.white * shaderData_Intensity[i];
                    Gizmos.DrawWireSphere(shaderData_Position[i], shaderData_Radius[i]);
                }
            }
        }


#if UNITY_EDITOR     
        [ContextMenu("Add DRM Game Objects Pool", false, 1000)]
        void AddDRMGameObjectsPool()
        {
            DRMGameObjectsPool pool = UnityEditor.Undo.AddComponent<DRMGameObjectsPool>(gameObject);
            pool.DRMController = this;
        }

        [ContextMenu("Add DRM Live Objects Pool", false, 1001)]
        void AddDRMLiveObjectsPool()
        {
            DRMLiveObjectsPool pool = UnityEditor.Undo.AddComponent<DRMLiveObjectsPool>(gameObject);
            pool.DRMController = this;
        }

        [ContextMenu("Add Selected Objects Materials To The Array", false, 1002)]
        void AddSelectedObjectsMaterialsToArray()
        {
            List<Material> listMaterials = new List<Material>();
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != null)
                    listMaterials.Add(materials[i]);
            }

            foreach (GameObject g in UnityEditor.Selection.gameObjects)
            {
                foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
                {
                    if (r != null && r.sharedMaterials != null && r.sharedMaterials.Length > 0)
                    {
                        foreach (Material m in r.sharedMaterials)
                        {
                            if (m != null && listMaterials.Contains(m) == false)
                                listMaterials.Add(m);
                        }
                    }
                }
            }

            UnityEditor.Undo.RecordObject(this, "Add Materials");
            materials = listMaterials.ToArray();
        }

        [ContextMenu("Find Pool Updating This DRMController", false, 2001)]
        void FindUsedObjectsPool()
        {
            //Find pool updating this controller
            DRMGameObjectsPool[] gameObjectsPool = UnityEngine.Object.FindObjectsOfType<DRMGameObjectsPool>();
            if (gameObjectsPool != null && gameObjectsPool.Length > 0)
            {
                foreach (var pool in gameObjectsPool)
                {
                    if (pool != null && pool.DRMController == this)
                    {
                        UnityEditor.EditorGUIUtility.PingObject(pool.gameObject);
                        return;
                    }
                }
            }

            DRMLiveObjectsPool[] liveObjectsPool = UnityEngine.Object.FindObjectsOfType<DRMLiveObjectsPool>();
            if (liveObjectsPool != null && liveObjectsPool.Length > 0)
            {
                foreach (var pool in liveObjectsPool)
                {
                    if (pool != null && pool.DRMController == this)
                    {
                        UnityEditor.EditorGUIUtility.PingObject(pool.gameObject);
                        return;
                    }
                }
            }
        }

        [ContextMenu("Find DRMController Script With The Same Settings", false, 2001)]
        void FindDuplicate()
        {
            foreach (var script in UnityEngine.Object.FindObjectsOfType<DRMController>())
            {
                if (script != this &&
                   script.shape == this.shape &&
                   script.count == this.count &&
                   script.type == this.type &&
                   script.blendMode == this.blendMode &&
                   script.ID == this.ID &&
                   script.scope == this.scope)
                {
                    UnityEditor.EditorGUIUtility.PingObject(script.gameObject);
                }
            }
        }       

        [ContextMenu("Open DRM Editor Window", false, 3001)]
        void OpenDRMEditorWindow()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("Window/Amazing Assets/Dynamic Radial Masks");
        }

        [ContextMenu("Open DRM Documentation", false, 3002)]
        void OpenDocumentation()
        {
            Application.OpenURL("https://docs.google.com/document/d/1nbvgZzM7ZF2LN3ndOlq9Ud8TwGpfr9KO_fO0T9rPZLA");
        }
#endif
    }
}