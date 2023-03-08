using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI_Manager : MonoBehaviour
{
    public static bool newPlay;
    public void StartNewOperation()
    {
        newPlay = true; ;
        LevelManager.initializeInstruction = LevelManager.InitializeInstruction.NewLevel;
        SceneManager.LoadScene(1);
    }

    public void ContinueOperation()
    {
        newPlay = false;
        LevelManager.initializeInstruction = LevelManager.InitializeInstruction.LoadLevel;
        SceneManager.LoadScene(1);
    }
}
