using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerDeath : MonoBehaviour
{
    Image loadMainBar;
    void PlayerDie(float health)
    {
        
    }
    IEnumerator ChargeMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");// ejemplo de prueba;
        while (!asyncLoad.isDone)
        {
            FillingMainMenuBar(asyncLoad.progress);
            yield return null;
        }

    }
    void FillingMainMenuBar(float velocity)
    {
        loadMainBar.gameObject.SetActive(true);
        loadMainBar.transform.localPosition = Vector3.Lerp(Vector3.right * 300, Vector3.zero, velocity);
    }
}
