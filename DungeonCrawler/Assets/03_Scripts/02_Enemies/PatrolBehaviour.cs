using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolBehaviour : EnemyBehaviour
{
    public enum LoopingMethod { Cycle, Reverse, OneTime }

    public int patrollLength = 7;
    public Vector3[] wayPoints { get; private set; }
    [SerializeField] LoopingMethod loopingMethod = LoopingMethod.Cycle;
    public int currentWayPoint = 0;
    int direction = 1;
    [SerializeField] float guardTime = 3f;
    float guardCounter;

    protected override void Start()
    {
        base.Start();
    }

    public void InitWayPoints()
    {
        baseController = GetComponentInParent<EnemyController>();
        List<Vector3> aviablePoints = new List<Vector3>(baseController.room.freeSpots);

        if (aviablePoints.Count < patrollLength) return;

        wayPoints = new Vector3[patrollLength];

        for(int i = 0; i < patrollLength; i++)
        {
            int index = Random.Range(0, aviablePoints.Count);
            Vector3 wayPoint = aviablePoints[index];
            aviablePoints.RemoveAt(index);
            wayPoints[i] = wayPoint;
        }
    }

    public void SetWayPoints(Vector3[] waypoints)
    {
        baseController = GetComponentInParent<EnemyController>();
        this.wayPoints = waypoints;
    }

    public override void Init()
    {
        if (wayPoints == null) return;

        base.Init();
        movement.SetDestination(wayPoints[currentWayPoint]);
    }

    public override void UpdateBehaviour()
    {
        if (wayPoints == null) return;

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

    //private void OnDrawGizmosSelected()
    //{
        //for (int i = 0; i < wayPoints.Length; i++)
        //{
        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawSphere(wayPoints[i], 0.3f);
        //    if (i > 0)
        //    {
        //        Gizmos.DrawLine(wayPoints[i - 1], wayPoints[i]);
        //    }
        //}

        //if (loopingMethod == LoopingMethod.Cycle)
        //    Gizmos.DrawLine(wayPoints[wayPoints.Length - 1], wayPoints[0]);
    //}

    void SetNextWayPoint()
    {
        movement.SetDestination(wayPoints[currentWayPoint]);
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
