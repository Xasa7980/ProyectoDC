using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimtationEvents : MonoBehaviour
{
    [SerializeField] EventData[] eventDatas;

    public void SendEvent(string eventName)
    {
        foreach(EventData data in eventDatas)
        {
            if (data.TryCatch(eventName))
                break;
        }
    }

    [System.Serializable]
    public struct EventData
    {
        [SerializeField] string eventName;
        [SerializeField] UnityEvent events;

        public bool TryCatch(string call)
        {
            if (call == eventName)
            {
                events.Invoke();
                return true;
            }

            return false;
        }
    }
}