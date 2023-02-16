using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Skill", menuName = "Create Skill/Melee Skill")]
public class MeleeSkill : SkillSO
{
    public override void ActivateSkill(ActivationMethod activation, GameObject gameObject, Transform transform, LayerMask hitMask, bool reset)
    {
        resetSkill = reset;
        if (resetSkill)
        {
            InvokeMethod(transform.position, gameObject);
            TakeDamage(gameObject, transform, hitMask);
            resetSkill = false;
        }

        if (activation == ActivationMethod.InstantRefreshing)
        {
            InvokeMethod(transform.position, gameObject);
            TakeDamage(gameObject, transform, hitMask);
        }
    }

    public override void InvokeMethod(Vector3 pos, GameObject obj)
    {
        Instantiate(obj, pos + Vector3.up , Quaternion.identity);
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
        else if(hitMethod == HitMethod.ParticleCollision)
        {

            if (gameObject.TryGetComponent<iDamageable>(out iDamageable damageable))
            {
                damageable.ApplyDamage(damage);
            }
        }
    }
}