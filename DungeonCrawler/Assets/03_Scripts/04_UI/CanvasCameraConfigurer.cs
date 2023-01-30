using System.Collections;
using UnityEngine;

public class CanvasCameraConfigurer : MonoBehaviour
{
    static Camera uiCamera;

    // Use this for initialization
    public static void ConfigureAll()
    {
        if (uiCamera == null)
            uiCamera = GameObject.FindGameObjectWithTag("UI_Cam").GetComponent<Camera>();

        CanvasCameraConfigurer[] canvases = FindObjectsOfType<CanvasCameraConfigurer>();

        foreach (CanvasCameraConfigurer c in canvases)
        {
            Canvas canvas = c.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
        }
    }
}