using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lookSpeed = 7;
    CharacterController controller;
    [SerializeField] FixedJoystick moveJoystick;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Moving();
    }
    void Moving()
    {
        Vector3 frontMove = CameraController.current.transform.forward * (Input.GetAxis("Vertical") + moveJoystick.Vertical) * speed;
        Vector3 sideMove = CameraController.current.transform.right * (Input.GetAxis("Horizontal") + moveJoystick.Horizontal) * speed;
        Vector3 velocity = frontMove + sideMove;
        Vector3 direction = velocity;
        direction.y = 0;
        direction.Normalize();

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
        }

        //Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal, 0, Input.GetAxisRaw("Vertical") + moveJoystick.Vertical) * speed * Time.deltaTime;
        controller.Move((velocity + Physics.gravity) * Time.deltaTime);
    }
}
