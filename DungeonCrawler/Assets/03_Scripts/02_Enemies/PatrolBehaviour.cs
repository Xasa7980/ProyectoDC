using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : EnemyBehaviour
{
    public enum LoopingMethod { Cycle, Reverse, OneTime }

    [SerializeField] Transform[] wayPoints;
    [SerializeField] LoopingMethod loopingMethod = LoopingMethod.Cycle;
    int currentWayPoint = 0;
    int direction = 1;
    [SerializeField] float guardTime = 3f;
    float guardCounter;

    private void Start()
    {
        foreach(Transform t in wayPoints)
        {
            t.parent = null;
        }
    }

    public override void Init()
    {
        base.Init();
        movement.SetDestination(wayPoints[currentWayPoint].position);
    }

    public override void UpdateBehaviour()
    {

        if (movement.remainingDistance <= 0.5f)
        {
            guardCounter -= Time.deltaTime;
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);

            if (guardCounter <= 0)
            {
                SetNextWayPoint();
                guardCounter = guardTime;
            }
        }
        else
        {
            anim.SetFloat("Speed", 1, 0.2f, Time.deltaTime);
        }
    }

    public override bool Validate()
    {
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wayPoints[i].position, 0.3f);
            if (i > 0)
            {
                Gizmos.DrawLine(wayPoints[i - 1].position, wayPoints[i].position);
            }
        }

        if (loopingMethod == LoopingMethod.Cycle)
            Gizmos.DrawLine(wayPoints[wayPoints.Length - 1].position, wayPoints[0].position);
    }

    void SetNextWayPoint()
    {
        movement.SetDestination(wayPoints[currentWayPoint].position);
        currentWayPoint += direction;

        switch (loopingMethod)
        {
            case LoopingMethod.Reverse:
                if (currentWayPoint == -1 || currentWayPoint == wayPoints.Length)
                {
                    direction *= -1;
                    currentWayPoint += direction;
                }
                break;

            case LoopingMethod.Cycle:
                direction = 1;
                if (currentWayPoint == wayPoints.Length)
                {
                    currentWayPoint = 0;
                }
                break;

            case LoopingMethod.OneTime:
                direction = 1;
                if (currentWayPoint == wayPoints.Length)
                {
                    currentWayPoint = wayPoints.Length - 1;
                }
                break;
        }
    }
}
