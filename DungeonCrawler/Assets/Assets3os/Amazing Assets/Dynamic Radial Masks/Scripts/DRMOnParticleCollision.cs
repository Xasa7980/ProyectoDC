using System.Collections.Generic;

using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DRMOnParticleCollision : MonoBehaviour
    {
        public DRMLiveObjectsPool[] DRMLiveObjectsPools;

        public DRMLiveObject DRMLiveObject;

        ParticleSystem pSystem;
        List<ParticleCollisionEvent> collisionEvents;



        private void OnParticleCollision(GameObject other)
        {
            if (other != null && DRMLiveObjectsPools != null && DRMLiveObjectsPools.Length > 0)
            {
                if (pSystem == null)
                    pSystem = GetComponent<ParticleSystem>();

                if (collisionEvents == null)
                    collisionEvents = new List<ParticleCollisionEvent>();



                int numCollisionEvents = pSystem.GetCollisionEvents(other, collisionEvents);

                int i = 0;
                while (i < numCollisionEvents)
                {
                    Vector3 pos = collisionEvents[i].intersection;


                    int poolIndex = DRMLiveObjectsPools.Length == 1 ? 0 : Random.Range(0, DRMLiveObjectsPools.Length);

                    if (DRMLiveObjectsPools[poolIndex] != null)
                        DRMLiveObjectsPools[poolIndex].AddItem(pos, DRMLiveObject);

                    i++;
                }
            }
        }
    }
}