using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] float sightRange = 12;
    [SerializeField] float reactionSpeed = 7;
    [SerializeField, Range(0, 1)] float gunReactionOffset = 1;
    [SerializeField] LayerMask targetMask;

    [SerializeField] Transform pivot;
    [SerializeField] Transform gun;

    Transform currentTarget;

    void Update()
    {
        if (!currentTarget)
        {
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, sightRange, targetMask);
            if (possibleTargets.Length > 0)
            {
                float minDst = float.MaxValue;
                Transform nearestTarget = null;

                foreach (Collider t in possibleTargets)
                {
                    float sqrDst = (t.transform.position - transform.position).sqrMagnitude;
                    if (sqrDst < minDst)
                    {
                        minDst = sqrDst;
                        nearestTarget = t.transform;
                    }
                }

                currentTarget = nearestTarget;
            }
        }
        else
        {
            if ((currentTarget.position - transform.position).sqrMagnitude > sightRange * sightRange)
            {
                currentTarget = null;
                return;
            }

            //Calcular la rotacion en horizontal de la torreta
            Vector3 targetHorDirection = (currentTarget.position - transform.position).normalized;
            targetHorDirection.y = 0;
            Quaternion pivotLookRotation = Quaternion.LookRotation(targetHorDirection);
            pivot.rotation = Quaternion.Slerp(pivot.rotation, pivotLookRotation, reactionSpeed * Time.deltaTime);

            //Calcular la rotacion vertical del cañon
            Vector3 targetVerDirection = (currentTarget.position - gun.position).normalized;
            Quaternion gunLookRotation = Quaternion.LookRotation(targetVerDirection);
            gun.rotation = Quaternion.Slerp(gun.rotation, gunLookRotation, reactionSpeed * gunReactionOffset * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
