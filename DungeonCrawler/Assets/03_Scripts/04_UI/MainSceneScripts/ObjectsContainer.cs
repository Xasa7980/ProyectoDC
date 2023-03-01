using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsContainer : MonoBehaviour
{
    [SerializeField] Base_UI_Window[] gameObjectsToActive;
    [SerializeField] Base_UI_Window[] gameObjectsToDeactive;

    private void Start()
    {
        foreach(Base_UI_Window window in gameObjectsToActive)
        {
            window.Init();
        }

        foreach (Base_UI_Window window in gameObjectsToDeactive)
        {
            window.Init();
        }
    }

    public void ActivatingObjects()
    {
        for (int i = 0; i < gameObjectsToActive.Length; i++)
        {
            gameObjectsToActive[i].Show();
        }
    }
    public void DeactivatingObjects()
    {
        for (int i = 0; i < gameObjectsToDeactive.Length; i++)
        {
            gameObjectsToDeactive[i].Hide();
        }
    }

}
