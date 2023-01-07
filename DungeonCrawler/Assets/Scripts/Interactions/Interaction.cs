using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, iInteractable
{
    public bool interactable { get; set; }

    public bool GetInteraction()
    {
        return interactable;
    }
}
