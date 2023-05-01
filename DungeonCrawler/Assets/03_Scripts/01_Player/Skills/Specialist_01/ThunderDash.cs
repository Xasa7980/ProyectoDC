using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderDash : MonoBehaviour
{
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] SkillSO skill;
    [SerializeField] LayerMask hitMask;
    [SerializeField] float radius;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<iDamageable>(out iDamageable damageable))
        {
            GetThreatsInside();
        }
    }

    void GetThreatsInside()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, radius, hitMask);
        foreach (Collider threat in threats)
        {
            skill.DamageCounter();
            if (skill.doDamage) skill.TakeDamage(threat.gameObject, threat.transform, hitMask);
        }
    }
}
