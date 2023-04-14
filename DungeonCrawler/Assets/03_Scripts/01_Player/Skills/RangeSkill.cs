using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Range Skill", menuName = "Create Skill/Range Skill")]
public class RangeSkill : SkillSO
{
    public override GameObject InvokeMethod(GameObject obj, Vector3 pos, Quaternion rot, Transform transform)
    {
        return Instantiate(obj, transform.position + Vector3.up * 2, rot);
    }

    public override void TakeDamage(GameObject target, Transform transform, LayerMask hitMask)
    {

        if (hitMethod == HitMethod.TriggerCollision & damageMethod == DamageMethod.InstantDamage)
        {

            if (target.TryGetComponent<iDamageable>(out iDamageable damageable))
            {
                damageable.ApplyDamage(damage);
                hitsImpacted++;
            }
        }
        else if (hitMethod == HitMethod.TriggerCollision & damageMethod == DamageMethod.DamageOvertime)
        {

            if (doDamage)
            {
                if (target.TryGetComponent<iDamageable>(out iDamageable damageable))
                {
                    damageable.ApplyDamage(damage);
                    hitsImpacted++;
                    doDamage = false;
                }
            }
            else DamageCounter();
        }
    }
    /* if(hitMethod == HitMethod.Raycast & damageMethod == DamageMethod.InstantDamage) //Balas ejemplo
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime, hitMask))
            {
                if (hit.collider.TryGetComponent<RaycastEventReciever>(out RaycastEventReciever reciever))
                {
                    reciever.TryInvoke(RaycastEventReciever.RaycastEventType.Shoot, ray);
                }

                if (hit.collider.TryGetComponent<iDamageable>(out iDamageable damageable))
                {
                    damageable.ApplyDamage(damage);
                    hitsImpacted++;
                }
                Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                Destroy(gameObject);
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        if (hitMethod == HitMethod.Raycast & damageMethod == DamageMethod.DamageOvertime) //Balas ejemplo
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime, hitMask))
            {
                if (hit.collider.TryGetComponent<RaycastEventReciever>(out RaycastEventReciever reciever))
                {
                    reciever.TryInvoke(RaycastEventReciever.RaycastEventType.Shoot, ray);
                }

                if (hit.collider.TryGetComponent<iDamageable>(out iDamageable damageable))
                {
                    damageable.ApplyDamage(damage);
                    hitsImpacted++;
                }
                Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                Destroy(gameObject);
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else if (hitMethod == HitMethod.OverlapSphere)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, hitRadius, hitMask);
            if (colls.Length > 0)
            {
                foreach (Collider coll in colls)
                {

                    if (coll.TryGetComponent<iDamageable>(out iDamageable damageable))
                    {
                        damageable.ApplyDamage(damage);
                        hitsImpacted++;
                    }
                }
            }
        }
        */ //OTROS METODOS DE GOLPEO POR SI ACASO, ACTUALIZAR
    public override void DamageCounter()
    {
        continueCounter += Time.deltaTime;
        if (continueCounter > continueTime)
        {
            continueCounter = 0;
            doDamage = true;
        }

    }
}