using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : EnemyBehaviour
{
    [SerializeField] RadialSensor sensor;
    [SerializeField] float aimSpeed = 9;
    [SerializeField] float attackInterval = 2;
    float attackCounter;

    Transform target;

    [Space(20)]
    [SerializeField] AttackType attack;
    [SerializeField] RifleController rifle;

    [Space(20)]
    [SerializeField] bool moveWhileShooting = false;
    [SerializeField] float nearestRadius = 7;
    [SerializeField] float moveInterval = 9;
    float moveCounter;

    private void Update()
    {
        if (attackCounter > 0)
            attackCounter -= Time.deltaTime;
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
                movement.agent.nextPosition = baseController.transform.position;

                if (moveCounter > 0) moveCounter -= Time.deltaTime;

                if (moveCounter <= 0)
                {
                    moveCounter = moveInterval;

                    Vector3 newDir = MathOps.DirFromAngle(target.eulerAngles.y + Random.Range(-90, 90));
                    Vector3 newPoint = newDir * Random.Range(nearestRadius, sensor.DetectionRadius);

                    movement.SetDestination(newPoint);
                }

                if (movement.agent.remainingDistance > 0.5f)
                {
                    float speedX = Vector3.Dot(baseController.transform.right, movement.agent.desiredVelocity.normalized) * 2;
                    float speedY = Vector3.Dot(baseController.transform.forward, movement.agent.desiredVelocity.normalized) * 2;

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

    private void LateUpdate()
    {
        if (!active) return;

        //Aplicar Inverse Kinematics para apuntar bien
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
