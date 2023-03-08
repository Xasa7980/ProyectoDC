using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    public Camera _camera;
    private void LateUpdate()
    {
        transform.forward = _camera.transform.forward;
    }
}
