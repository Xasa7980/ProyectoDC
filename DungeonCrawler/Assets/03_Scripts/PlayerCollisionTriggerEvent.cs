using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCollisionTriggerEvent : MonoBehaviour
{
    [SerializeField] UnityEvent onPlayerEnter = new UnityEvent();
    [SerializeField] UnityEvent onPlayerStay = new UnityEvent();
    [SerializeField] UnityEvent onPlayerExit = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            onPlayerEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            onPlayerStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            onPlayerExit.Invoke();
        }
    }
}
