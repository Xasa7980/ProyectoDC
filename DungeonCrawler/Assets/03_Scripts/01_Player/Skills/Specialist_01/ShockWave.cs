using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] SkillSO skill;

    [SerializeField] float speed = 2;
    [SerializeField] float pushForce;
    [SerializeField] LayerMask hitMask;
    [SerializeField] float radius;
    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<iDamageable>(out iDamageable damageable))
        {
            GetThreatsInside();
        }
    }

    void GetThreatsInside()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, radius, hitMask);
        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.localPosition - transform.localPosition).normalized;
            float speedX = Vector3.Dot(threat.transform.right.normalized, threatDir) * pushForce;
            float speedZ = Vector3.Dot(threat.transform.forward.normalized, threatDir) * pushForce;
            Vector3 pushedPos = new Vector3(speedX, 0, speedZ);
            Vector3 threatFinalPos = threatDir + pushedPos;
            threatFinalPos.y = 0;
            //threat.transform.position = Vector3.MoveTowards(threat.transform.position, threatFinalPos, speed);
            threat.transform.Translate(threatFinalPos.normalized * pushForce, (Space)ForceMode.Impulse);
            skill.DamageCounter();
            if (skill.doDamage) skill.TakeDamage(threat.gameObject, threat.transform, hitMask);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
