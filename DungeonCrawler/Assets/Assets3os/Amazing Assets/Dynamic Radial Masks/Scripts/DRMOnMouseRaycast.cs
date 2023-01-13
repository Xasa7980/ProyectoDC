using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    public class DRMOnMouseRaycast : MonoBehaviour
    {
        public LayerMask raycastLayer = 1;
        public bool selfRaycastOnly;
        public float clicksPerSecond = 1;
        float clicksTimer;

        public DRMLiveObjectsPool[] DRMLiveObjectsPools;

        [Space(5)]
        public DRMLiveObject DRMLiveObject = new DRMLiveObject();



        void Update()
        {
            clicksTimer += Time.deltaTime;


            if (DRMInput.GetLeftMouseButtonDown())     //Do raycast on every mouse click
            {
                clicksTimer = 0;

                DoRaycast();
            }
            else if (DRMInput.GetLeftMouseButton())    //Do raycast when hodling mouse button down
            {
                if (clicksTimer > 1.0f / clicksPerSecond)
                {
                    clicksTimer = 0;

                    DoRaycast();
                }
            }

        }

        void DoRaycast()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(DRMInput.GetMousePosition());

            if (Physics.Raycast(ray, out hitInfo, 1000, raycastLayer))
            {
                int poolIndex = DRMLiveObjectsPools.Length == 1 ? 0 : Random.Range(0, DRMLiveObjectsPools.Length);
                if (DRMLiveObjectsPools[poolIndex] != null)
                {
                    if (selfRaycastOnly)
                    {
                        if (hitInfo.collider.gameObject == this.gameObject)
                            DRMLiveObjectsPools[poolIndex].AddItem(hitInfo.point, DRMLiveObject);    //Add item to the pool only if collider is the same script holder                    
                    }
                    else
                    {
                        DRMLiveObjectsPools[poolIndex].AddItem(hitInfo.point, DRMLiveObject);    //Add item to the pool only if collider is the same script holder
                    }
                }
            }
        }

        private void Reset()
        {
            raycastLayer = 1;
            clicksPerSecond = 10;

            DRMLiveObject.Reset();
        }
    }
}