using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lookSpeed = 7;

    CharacterController controller;
    Animator anim;

    bool aiming = false;
    bool rolling = false;

    [SerializeField] FixedJoystick moveJoystick;
    [SerializeField] float dodgeSpeed = 5;
    //#region DodgeParameters
    //[SerializeField, Range(0,1.5f)] float evadeRange = 0.5f;
    //float timeToResetDodge;
    //[SerializeField] float timeDodgeGetReady = 2;
    //bool canDodge;
    //bool dodgeIsRefreshed;
    //#endregion
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Moving();
        //ResetDodge();
    }
    void Moving()
    {
        aiming = Input.GetMouseButton(0);
        anim.SetBool("Aiming", aiming);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Roll");
            rolling = true;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            anim.SetFloat("Speed_X", 0, 0.25f, Time.deltaTime);
            anim.SetFloat("Speed_Y", 0, 0.25f, Time.deltaTime);

            Vector3 rollFrontMove = CameraController.current.transform.forward * Input.GetAxis("Vertical") * 2;
            Vector3 rollSideMove = CameraController.current.transform.right * Input.GetAxis("Horizontal") * 2;
            Vector3 rollVelocity = rollFrontMove + rollSideMove;

            Vector3 rollDirection = rollVelocity.normalized;

            if (rollDirection != Vector3.zero)
            {
                Quaternion rollLookRotation = Quaternion.LookRotation(rollDirection);
                transform.rotation = rollLookRotation;
            }

            controller.Move(rollDirection * dodgeSpeed * Time.deltaTime);

            return;
        }

        Vector3 frontMove = CameraController.current.transform.forward * (Input.GetAxis("Vertical") + moveJoystick.Vertical) * speed;
        Vector3 sideMove = CameraController.current.transform.right * (Input.GetAxis("Horizontal") + moveJoystick.Horizontal) * speed;
        Vector3 velocity = frontMove + sideMove;
        Vector3 direction = velocity;
        direction.y = 0;
        direction.Normalize();

        if (!aiming)
        {
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
                //if (canDodge) DodgeMove(direction, velocity);
            }

            anim.SetFloat("Speed", velocity.sqrMagnitude, 0.1f, Time.deltaTime);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDst;

            if (groundPlane.Raycast(ray, out rayDst))
            {
                Vector3 aimPoint = ray.GetPoint(rayDst);
                Vector3 lookPoint = aimPoint;
                lookPoint.y = 0;
                Vector3 lookDirection = (lookPoint - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);

                float speedX = Vector3.Dot(transform.right, direction) * 2;
                float speedY = Vector3.Dot(transform.forward, direction) * 2;

                anim.SetFloat("Speed_X", speedX, 0.2f, Time.deltaTime);
                anim.SetFloat("Speed_Y", speedY, 0.2f, Time.deltaTime);
            }
        }

        //Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal, 0, Input.GetAxisRaw("Vertical") + moveJoystick.Vertical) * speed * Time.deltaTime;
        //controller.Move((velocity + Physics.gravity) * Time.deltaTime);

        controller.Move(Physics.gravity * Time.deltaTime);
    }

    public void GrabWeapon()
    {
        //Evento de animacion llamados desde el animator
        //Aqui poner la logica para que el player agarre el arma de la espalda
    }

    public void LooseWeapon()
    {
        //Evento de animacion llamados desde el animator
        //Aqui poner la logica para que el player suelte el arma en la espalda
    }

    //public void DoDodge()
    //{
    //    if(dodgeIsRefreshed) canDodge = true;
    //}

    //void DodgeMove(Vector3 direction, Vector3 velocity)
    //{
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
    //    }
    //    controller.Move(velocity * 0.75f);
    //    canDodge = false;
    //    dodgeIsRefreshed = false;
    //}

    //void ResetDodge()
    //{
    //    if (!dodgeIsRefreshed)
    //    {
    //        timeToResetDodge += Time.deltaTime;
    //        if(timeToResetDodge > timeDodgeGetReady)
    //        {
    //            dodgeIsRefreshed = true;
    //            timeToResetDodge = 0;
    //        }
    //    }
    //}
}
