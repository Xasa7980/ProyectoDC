using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RaycastEventReciever : MonoBehaviour
{
    public enum RaycastEventType
    {
        Shoot,
        Hit
    }

    [SerializeField] RaycastEventStruct[] events;

    public void TryInvoke(RaycastEventType type, Ray ray)
    {
        foreach(RaycastEventStruct et in events)
        {
            et.TryInvoke(type, ray);
        }
    }

    [System.Serializable]
    class RaycastEventStruct
    {
        [SerializeField] RaycastEventType eventType;
        [SerializeField] RaycastEvent raycasEvent = new RaycastEvent();

        public void TryInvoke(RaycastEventType type, Ray ray)
        {
            if(eventType == type)
                raycasEvent.Invoke(ray);
        }
    }

    [System.Serializable]
    class RaycastEvent : UnityEvent<Ray> { }
}