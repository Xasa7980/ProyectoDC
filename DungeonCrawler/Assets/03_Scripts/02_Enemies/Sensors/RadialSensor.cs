﻿using System.Collections;
using UnityEngine;

public class RadialSensor : Sensor
{
    [SerializeField, Range(0, 360)] float detectionAngle = 60;

    public float DetectionRadius => detectionRadius;

    public override bool ThreatsDetected()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle <= detectionAngle / 2)
            {
                if (DirectLineToTarget(threat.transform.position))
                    return true;
            }
        }

        return false;
    }

    public override Transform GetNearestThreat()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        float minDst = float.MaxValue;
        Transform nearest = null;

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle <= detectionAngle / 2)
            {
                float sqrDst = (threat.transform.position - transform.position).sqrMagnitude;
                if(sqrDst < minDst && DirectLineToTarget(threat.transform.position))
                {
                    minDst = sqrDst;
                    nearest = threat.transform;
                }
            }
        }

        return nearest;
    }

    public override bool InRange(Vector3 position)
    {
        float sqrDst = (position - transform.position).sqrMagnitude;
        return sqrDst <= Mathf.Pow(detectionRadius, 2);
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, detectionRadius);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + MathOps.DirFromAngle((-detectionAngle / 2 + transform.rotation.eulerAngles.y)) * detectionRadius);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + MathOps.DirFromAngle((detectionAngle / 2 + transform.rotation.eulerAngles.y)) * detectionRadius);
#endif
    }
}