using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehaviour : EnemyBehaviour
{
    [SerializeField] RadialSensor sensor;
    [SerializeField] float damage;

    [SerializeField] float minAttackInterval;
    [SerializeField] float maxAttackInterval;
    float attackCounter;

    [SerializeField] AttackType attack;

    private void Update()
    {
        if (attackCounter > 0)
            attackCounter -= Time.deltaTime;
    }

    public override void Init()
    {
        base.Init();
        attack.ConfigureAnimator((AnimatorOverrideController)anim.runtimeAnimatorController);
    }

    public override void Release()
    {
        base.Release();
    }

    public override void UpdateBehaviour()
    {
        if (attackCounter <= 0)
        {
            anim.SetTrigger("Attack");
            attackCounter = Random.Range(minAttackInterval, maxAttackInterval);
        }
        else
        {
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
            anim.SetFloat("Speed_X", 0, 0.2f, Time.deltaTime);
            anim.SetFloat("Speed_Y", 0, 0.2f, Time.deltaTime);
        }
    }

    public override bool Validate()
    {
        return sensor.ThreatsDetected();
    }

    public void PerformAttack()
    {

    }
}
