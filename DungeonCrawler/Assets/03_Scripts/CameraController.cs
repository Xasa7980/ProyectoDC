using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController current { get; private set; }

    public Transform target;
    [SerializeField] float followSpeed = 5;

    public static Vector3 offset { get; set; }

    [SerializeField] Camera _gameCamera;
    public Camera gameCamera => _gameCamera;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed * Time.deltaTime);
    }
}
