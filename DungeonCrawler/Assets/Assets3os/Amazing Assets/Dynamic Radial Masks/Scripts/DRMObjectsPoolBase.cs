using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    public abstract class DRMObjectsPoolBase : MonoBehaviour
    {
        public DRMController DRMController;

        
        public delegate void Callback(DRMObjectsPoolBase DRMObjectsPool);
        public Callback CallbackOnFirstElementAdded;      //Subscribe to this event to be notified when first element is added to an empty pool 
        public Callback CallbackOnPoolIsEmpty;            //Subscribe to this event to be notified when pool becomes empty


        void OnDestroy()
        {
            if (DRMController != null)
                DRMController.ResetShaderData();
        }

        void OnDisable()
        {
            if (DRMController != null)
                DRMController.ResetShaderData();
        }

        void OnEnable()
        { 
            Update();
        }

        void Update()
        {
            if (DRMController == null)
                return;


            //Force update in editor
            if ((Application.isEditor && Application.isPlaying == false) || DRMController.updateMethod == DRMController.Enum.ScriptUpdateMethod.Update)
            {
                UpdateController();
            }
        }

        void FixedUpdate()
        {
            if (DRMController == null)
                return;


            if (DRMController.updateMethod == DRMController.Enum.ScriptUpdateMethod.FixedUpdate)
            {
                UpdateController();
            }
        }


        abstract public void UpdateController();
    }
}