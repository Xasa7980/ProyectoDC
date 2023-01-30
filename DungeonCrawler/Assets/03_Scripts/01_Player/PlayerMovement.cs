using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    public void SetPlayerLockedState(bool value)
    {
        locked = value;

    }

    public bool locked { get; private set; }

    [SerializeField] float speed = 5;
    [SerializeField] float lookSpeed = 7;
    [SerializeField] Transform aimTarget;
    [SerializeField] Rig aimRig;
    RifleController currentWeapon;
    float shootDelayCounter = 0.15f;

    CharacterController controller;
    Animator anim;

    public static bool aiming { get; private set; }

    [SerializeField] FixedJoystick moveJoystick;
    [SerializeField] FixedJoystick attackJoystick;

    [SerializeField] float dodgeSpeed = 5;
    //#region DodgeParameters
    //[SerializeField, Range(0,1.5f)] float evadeRange = 0.5f;
    //float timeToResetDodge;
    //[SerializeField] float timeDodgeGetReady = 2;
    //bool canDodge;
    //bool dodgeIsRefreshed;
    //#endregion
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentWeapon = GetComponentInChildren<RifleController>();
        FindObjectOfType<DungeonController>().onRoomClearedUnityEvent.AddListener(() => SetPlayerLockedState(true));
    }

    private void Update()
    {
        if (locked)
        {
            anim.SetFloat("Speed_X", 0, 0.25f, Time.deltaTime);
            anim.SetFloat("Speed_Y", 0, 0.25f, Time.deltaTime);
            anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

            return;
        }

        Moving();
        //ResetDodge();
    }
    void Moving()
    {
        aiming = attackJoystick.Direction.sqrMagnitude >= 0.1;

        if (Input.GetMouseButtonDown(0))
            shootDelayCounter = 0.15f;

        anim.SetBool("Aiming", aiming);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Roll");
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            anim.SetFloat("Speed_X", 0, 0.25f, Time.deltaTime);
            anim.SetFloat("Speed_Y", 0, 0.25f, Time.deltaTime);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 20);

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

            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 20);

            CameraController.offset = Vector3.zero;
        }
        else
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Vector3 origin = new Vector3(transform.position.x, currentWeapon.transform.position.y, transform.position.z);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 20);

            if (shootDelayCounter <= 0)
                currentWeapon.TryShoot();
            else
                shootDelayCounter -= Time.deltaTime;

            //Vector3 frontAim = Camera.current.transform.forward * moveJoystick.Direction.y;
            //Vector3 sideAim = Camera.current.transform.right * moveJoystick.Direction.x;
            Vector3 stickFront = attackJoystick.Vertical * CameraController.current.transform.forward;
            Vector3 stickSide = attackJoystick.Horizontal * CameraController.current.transform.right;
            Vector3 aimStickDirection = stickFront + stickSide;

            CameraController.offset = aimStickDirection * 2.5f;

            Vector3 aimPoint = transform.position + aimStickDirection * 30 + Vector3.up * (currentWeapon.transform.position.y - transform.position.y);
            aimTarget.position = aimPoint;
            Vector3 lookPoint = aimPoint;
            Vector3 lookDirection = (lookPoint - transform.position).normalized;
            lookDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);

            float speedX = Vector3.Dot(transform.right, direction) * 2;
            float speedY = Vector3.Dot(transform.forward, direction) * 2;

            anim.SetFloat("Speed_X", speedX, 0.2f, Time.deltaTime);
            anim.SetFloat("Speed_Y", speedY, 0.2f, Time.deltaTime);
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
