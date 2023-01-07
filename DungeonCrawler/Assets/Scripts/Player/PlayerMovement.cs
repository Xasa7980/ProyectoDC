using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
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
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal, 0, Input.GetAxisRaw("Vertical") + moveJoystick.Vertical) * speed * Time.deltaTime;
        controller.Move(move);
    }
}
