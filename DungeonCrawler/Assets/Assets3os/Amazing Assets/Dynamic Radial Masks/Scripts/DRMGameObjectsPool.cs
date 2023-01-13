using System.Collections.Generic;

using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    [ExecuteAlways]
    public class DRMGameObjectsPool : DRMObjectsPoolBase
    {

#if UNITY_EDITOR
        [DRMGameObjectsPoolCreateButtonAttribute]
        public int createButton;
#endif

        public List<DRMGameObject> DRMGameObjects = new List<DRMGameObject>();
        int previousObjectsCount = 0;


        override public void UpdateController()
        {
            if (DRMController == null)
                return;


            //Make sure all variables are set to zero
            DRMController.ResetShaderData();


            if (DRMGameObjects != null)
            {
                //Check callback events
                if(previousObjectsCount != DRMGameObjects.Count)
                {
                    if (DRMGameObjects.Count == 0 && CallbackOnPoolIsEmpty != null)
                        CallbackOnPoolIsEmpty(this);

                    if (previousObjectsCount == 0 && DRMGameObjects.Count > 0 && CallbackOnFirstElementAdded != null)
                        CallbackOnFirstElementAdded(this);


                    previousObjectsCount = DRMGameObjects.Count;
                }


                //Update DRMController
                for (int i = 0; i < DRMGameObjects.Count; i++)
                {
                    //Does DRMController support such amount of indexes?
                    if (i >= DRMController.count)
                        break;


                    if (DRMGameObjects[i] != null && DRMGameObjects[i].isActiveAndEnabled)
                    {
                        DRMController.shaderData_Position[i] = DRMGameObjects[i].transform.position;
                        DRMController.shaderData_Radius[i] = DRMGameObjects[i].radius;
                        DRMController.shaderData_Intensity[i] = DRMGameObjects[i].intensity;
                        DRMController.shaderData_NoiseStrength[i] = DRMGameObjects[i].noiseStrength;
                        DRMController.shaderData_EdgeSize[i] = DRMGameObjects[i].edgeSize;
                        DRMController.shaderData_RingCount[i] = DRMGameObjects[i].ringCount;
                        DRMController.shaderData_Phase[i] = DRMGameObjects[i].currentPhase;
                        DRMController.shaderData_Frequency[i] = DRMGameObjects[i].frequency;
                        DRMController.shaderData_Smooth[i] = DRMGameObjects[i].smooth;
                    }
                }
            }
            else
            {
                if(previousObjectsCount > 0 && CallbackOnPoolIsEmpty != null)
                    CallbackOnPoolIsEmpty(this);

                previousObjectsCount = 0;
            }
        }
    }
}