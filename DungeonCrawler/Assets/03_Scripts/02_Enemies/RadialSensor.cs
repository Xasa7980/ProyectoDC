using System.Collections;
using UnityEngine;

public class RadialSensor : MonoBehaviour
{
    [SerializeField] float detectionRadius = 15;
    [SerializeField, Range(0, 360)] float detectionAngle = 60;
    [SerializeField] LayerMask detectionMask;

    public float DetectionRadius => detectionRadius;

    public bool ThreatsDetected()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle <= detectionAngle / 2)
                return true;
        }

        return false;
    }

    public Transform GetNearestThreat()
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
                if(sqrDst < minDst)
                {
                    minDst = sqrDst;
                    nearest = threat.transform;
                }
            }
        }

        return nearest;
    }

    public bool InRange(Vector3 position)
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