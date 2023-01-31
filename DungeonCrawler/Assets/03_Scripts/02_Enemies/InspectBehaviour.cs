using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectBehaviour : EnemyBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    public void AssignInspectPoint(Vector3 point)
    {
        this.point = point;
        hasPoint = true;
        movement.SetDestination(point);
    }

    Vector3 point;
    public bool hasPoint { get; private set; }

    public override void UpdateBehaviour()
    {
        if ((transform.position - point).sqrMagnitude <= 1.5f) hasPoint = false;
    }

    public override bool Validate()
    {
        return hasPoint;
    }

    public override void Release()
    {
        base.Release();
        hasPoint = false;
    }

    public void InspectRayOrigin(Ray ray)
    {
        AssignInspectPoint(ray.origin);
    }
}