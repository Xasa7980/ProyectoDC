using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene los stats y logica de funcionamiento
/// </summary>
public class ChaseBehaviour : EnemyBehaviour
{
    [SerializeField] Sensor sensor;

    Transform target;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();

        anim = baseController.GetComponent<Animator>();
    }

    public override bool Validate()
    {
        return sensor.ThreatsDetected();
    }

    public override void UpdateBehaviour()
    {
        if (!target)
        {
            target = sensor.GetNearestThreat();
        }
        else
        {
            movement.SetDestination(target.position);
            anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        }
    }

    public override void Release()
    {
        base.Release();
        target = null;
    }
}
