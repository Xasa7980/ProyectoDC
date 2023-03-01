using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootBehaviour : EnemyBehaviour
{
    [SerializeField] Sensor sensor;
    [SerializeField] float aimSpeed = 9;
    [SerializeField] float attackInterval = 2;
    float attackCounter;

    [Header("Aim IK")]
    [SerializeField] Rig aimRig;
    [SerializeField] Transform aimTarget;
    float aimWeight = 0;

    float targetHeightOffset;
    Transform _target;
    Transform target
    {
        get => _target;
        set
        {
            _target = value;
            if (value)
            {
                Collider targetCollider = value.GetComponent<Collider>();

                switch (targetCollider)
                {
                    case CapsuleCollider capsuleCollider:
                        targetHeightOffset = capsuleCollider.height * 0.75f;
                        break;

                    case BoxCollider boxCollider:
                        targetHeightOffset = boxCollider.size.y * 0.75f;
                        break;

                    case CharacterController characterController:
                        targetHeightOffset = characterController.height * 0.75f;
                        break;

                    default:
                        targetHeightOffset = 0;
                        break;
                }
            }

        }
    }

    [Space(20)]
    [SerializeField] AttackType attack;
    [SerializeField] RifleController rifle;

    [Space(20)]
    [SerializeField] bool moveWhileShooting = false;
    [SerializeField] float moveInterval = 9;
    float moveCounter;

    private void Update()
    {
        if (attackCounter > 0)
            attackCounter -= Time.deltaTime;

        if (!target)
        {
            aimWeight = Mathf.Lerp(aimWeight, 0, Time.deltaTime * 20);
            aimRig.weight = aimWeight;
        }
    }

    public override void Init()
    {
        base.Init();

        attack.ConfigureAnimator((AnimatorOverrideController)anim.runtimeAnimatorController);
        movement.overrideController = true;
        anim.SetBool("Aiming", true);
        moveCounter = moveInterval;
        movement.SetDestination(baseController.transform.position);
    }

    public override void Release()
    {
        base.Release();
        target = null;
        movement.overrideController = false;
        anim.SetBool("Aiming", false);
        anim.SetTrigger("InterruptAttack");
    }

    public override void UpdateBehaviour()
    {
        if (!target)
        {
            target = sensor.GetNearestThreat();

            
        }
        else
        {
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);

            aimWeight = Mathf.Lerp(aimWeight, 1, Time.deltaTime * 20);
            aimRig.weight = aimWeight;

            aimTarget.position = target.position + Vector3.up * targetHeightOffset;

            Vector3 targetDirection = (target.position - transform.position).normalized;
            targetDirection.y = 0;
            Quaternion aimDirection = Quaternion.LookRotation(targetDirection);
            baseController.transform.rotation = Quaternion.Slerp(baseController.transform.rotation, aimDirection, aimSpeed * Time.deltaTime);
            
            if (Vector3.Angle(transform.forward, targetDirection) < 5 && attackCounter <= 0)
            {
                anim.SetTrigger("Shoot");
                attackCounter = attackInterval;
            }

            if (moveWhileShooting)
            {
                movement.nextPosition = baseController.transform.position;

                if (moveCounter > 0) moveCounter -= Time.deltaTime;

                if (moveCounter <= 0)
                {
                    moveCounter = moveInterval;

                    Vector3 newDir = MathOps.PlaneDirFromAngle(Random.Range(0, 360));
                    Vector3 newPoint = transform.position + newDir * Random.Range(3, 6);

                    movement.SetDestination(newPoint);
                }

                if (movement.remainingDistance > 0.5f)
                {
                    float speedX = Vector3.Dot(baseController.transform.right, movement.desiredVelocity.normalized) * 2;
                    float speedY = Vector3.Dot(baseController.transform.forward, movement.desiredVelocity.normalized) * 2;

                    anim.SetFloat("Speed_X", speedX, 0.2f, Time.deltaTime);
                    anim.SetFloat("Speed_Y", speedY, 0.2f, Time.deltaTime);
                }
                else
                {
                    anim.SetFloat("Speed_X", 0, 0.2f, Time.deltaTime);
                    anim.SetFloat("Speed_Y", 0, 0.2f, Time.deltaTime);
                }
            }
        }
    }

    public void Shoot()
    {
        attack.Perform(rifle);
    }

    public override bool Validate()
    {
        return sensor.ThreatsDetected();
    }
}
