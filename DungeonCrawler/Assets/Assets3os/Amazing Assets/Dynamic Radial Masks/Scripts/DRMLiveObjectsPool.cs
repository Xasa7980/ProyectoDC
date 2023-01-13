using System;
using System.Collections.Generic;

using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    [ExecuteAlways]
    public class DRMLiveObjectsPool : DRMObjectsPoolBase
    {       
        [HideInInspector]
        public List<DRMLiveObject> DRMLiveObjects;



        override public void UpdateController()
        {
            if (DRMController == null)
                return;


            //Make sure all variables are set to zero
            DRMController.ResetShaderData();



            if (DRMLiveObjects != null)
            {
                Initialize();

                //Animate
                if (DRMLiveObjects.Count > 0)
                {
                    for (int i = DRMLiveObjects.Count - 1; i >= 0; i -= 1)
                    {
                        if (DRMLiveObjects[i].Animate() == false)
                        {
                            DRMLiveObjects.RemoveAt(i);


                            //Pool became empty
                            if (DRMLiveObjects.Count == 0 && CallbackOnPoolIsEmpty != null)
                                CallbackOnPoolIsEmpty(this);
                        }
                    }
                }

                //Update Controller
                if (DRMLiveObjects.Count > 0)
                {
                    for (int i = 0; i < DRMLiveObjects.Count; i++)
                    {
                        //Does DRMController support such amount of indexes?
                        if (i >= DRMController.count)
                            break;

                        DRMController.shaderData_Position[i] = DRMLiveObjects[i].position;
                        DRMController.shaderData_Radius[i] = DRMLiveObjects[i].radius.currentValue;
                        DRMController.shaderData_Intensity[i] = DRMLiveObjects[i].intensity.currentValue;
                        DRMController.shaderData_NoiseStrength[i] = DRMLiveObjects[i].noise.currentValue;
                        DRMController.shaderData_EdgeSize[i] = DRMLiveObjects[i].edgeSize.currentValue;
                        DRMController.shaderData_RingCount[i] = DRMLiveObjects[i].ringCount.currentValue;
                        DRMController.shaderData_Phase[i] = DRMLiveObjects[i].phaseSpeed.currentValue;
                        DRMController.shaderData_Frequency[i] = DRMLiveObjects[i].frequency.currentValue;
                        DRMController.shaderData_Smooth[i] = DRMLiveObjects[i].smooth.currentValue;
                    }
                }
            }
        }

        public void AddItem(Vector3 position, DRMLiveObject obj)
        {
            Initialize();

            //Pool cannot hold more objects than DRMController supports
            if (DRMLiveObjects.Count < DRMController.count)
            {
                DRMLiveObjects.Add(new DRMLiveObject(obj, position));

                if (CallbackOnFirstElementAdded != null && DRMLiveObjects.Count == 1)
                    CallbackOnFirstElementAdded(this);
            }
        }

        void Initialize()
        {
            if (DRMLiveObjects == null)
                DRMLiveObjects = new List<DRMLiveObject>();
        }

        [ContextMenu("Clear Pool")]
        public void ClearPool()
        {            
            if(DRMController != null)
                DRMController.ResetShaderData();

            if (DRMLiveObjects != null && DRMLiveObjects.Count > 0)
            {
                DRMLiveObjects.Clear();

                if (CallbackOnPoolIsEmpty != null)
                    CallbackOnPoolIsEmpty(this);
            }
        }
    }
}