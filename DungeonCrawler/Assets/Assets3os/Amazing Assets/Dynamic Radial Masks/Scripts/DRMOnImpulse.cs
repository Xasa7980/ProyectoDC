using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    public class DRMOnImpulse : MonoBehaviour
    {
        public DRMLiveObjectsPool DRMLiveObjectsPool;

        public float impulseFrequency = 1;
        float deltaTime;


        public DRMLiveObject DRMLiveObject;


        private void Start()
        {
            deltaTime = impulseFrequency;
        }

        void Update()
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > impulseFrequency)
            {
                deltaTime = 0;

                DRMLiveObjectsPool.AddItem(transform.position, DRMLiveObject);
            }
        }
    }
}