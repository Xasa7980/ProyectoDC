using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController current { get; private set; }

    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 5;

    Camera gameCamera;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        gameCamera = Camera.main;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}
