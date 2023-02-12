using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Range Skill", menuName = "Create Skill/Range Skill")]
public class RangeSkill : SkillSO
{
    public override void ActivateSkill(ActivationMethod activation, GameObject gameObject, Transform transform, LayerMask hitMask, bool reset)
    {
        resetSkill = reset;

        if (resetSkill)
        {
            InvokeMethod(transform.position, gameObject);
            TakeDamage(gameObject, transform, hitMask);
            resetSkill = false;
            Debug.Log("h3");
        }

        if (activation == ActivationMethod.InstantRefreshing)
        {
            InvokeMethod(transform.position, gameObject);
            TakeDamage(gameObject, transform, hitMask);
            Debug.Log("h2");
        }
    }

    public override void InvokeMethod(Vector3 pos, GameObject obj)
    {
        Instantiate(obj, pos, Quaternion.identity);
    }

    public override void TakeDamage(GameObject gameObject, Transform transform, LayerMask hitMask)
    {
        if(hitMethod == HitMethod.Raycast) //Balas ejemplo
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

        else if(hitMethod == HitMethod.OverlapSphere)
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
    }
}