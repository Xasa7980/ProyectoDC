using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lookSpeed = 7;
    CharacterController controller;
    [SerializeField] FixedJoystick moveJoystick;
    #region DodgeParameters
    [SerializeField, Range(0,1.5f)] float evadeRange = 0.5f;
    float timeToResetDodge;
    [SerializeField] float timeDodgeGetReady = 2;
    bool canDodge;
    bool dodgeIsRefreshed;
    #endregion
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Moving();
        ResetDodge();
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
            if (canDodge) DodgeMove(direction,velocity);
        }

        //Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal, 0, Input.GetAxisRaw("Vertical") + moveJoystick.Vertical) * speed * Time.deltaTime;
        controller.Move((velocity + Physics.gravity) * Time.deltaTime);
    }
    public void DoDodge()
    {
        if(dodgeIsRefreshed) canDodge = true;
    }
    void DodgeMove(Vector3 direction, Vector3 velocity)
    {
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
        }
        controller.Move(velocity * 0.75f);
        canDodge = false;
        dodgeIsRefreshed = false;
    }
    void ResetDodge()
    {
        if (!dodgeIsRefreshed)
        {
            timeToResetDodge += Time.deltaTime;
            if(timeToResetDodge > timeDodgeGetReady)
            {
                dodgeIsRefreshed = true;
                timeToResetDodge = 0;
            }
        }
    }
}
