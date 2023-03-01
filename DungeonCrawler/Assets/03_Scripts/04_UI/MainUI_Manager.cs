using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI_Manager : MonoBehaviour
{
    public void StartNewOperation()
    {
        LevelManager.initializeInstruction = LevelManager.InitializeInstruction.NewLevel;
        SceneManager.LoadScene(1);
    }

    public void ContinueOperation()
    {
        LevelManager.initializeInstruction = LevelManager.InitializeInstruction.LoadLevel;
        SceneManager.LoadScene(1);
    }
}
