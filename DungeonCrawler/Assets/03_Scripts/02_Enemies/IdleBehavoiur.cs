using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavoiur : EnemyBehaviour
{
    public override void Init()
    {
        base.Init();

        anim = baseController.GetComponent<Animator>();
    }

    public override void Release()
    {
        base.Release();
    }

    public override void UpdateBehaviour()
    {
        anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        anim.SetFloat("Speed_X", 0, 0.2f, Time.deltaTime);
        anim.SetFloat("Speed_Y", 0, 0.2f, Time.deltaTime);
    }

    public override bool Validate()
    {
        return true;
    }
}
