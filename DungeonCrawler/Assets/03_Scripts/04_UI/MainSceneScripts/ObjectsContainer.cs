using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsContainer : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectsToActive;
    [SerializeField] GameObject[] gameObjectsToDeactive;
    public void ActivatingObjects()
    {
        for (int i = 0; i < gameObjectsToActive.Length; i++)
        {
            gameObjectsToActive[i].SetActive(true);
        }
    }
    public void DeactivatingObjects()
    {
        for (int i = 0; i < gameObjectsToDeactive.Length; i++)
        {
            gameObjectsToDeactive[i].SetActive(false);
        }
    }

}
