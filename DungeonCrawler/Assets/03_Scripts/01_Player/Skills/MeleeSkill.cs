using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Skill", menuName = "Create Skill/Melee Skill")]
public class MeleeSkill : SkillSO
{
    public override GameObject InvokeMethod(GameObject obj, Vector3 pos, Quaternion rot, Transform transform)
    {
       return Instantiate(obj, transform.position + Vector3.up * 2, rot);
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
                hitsImpacted++;
            }
        }
    }
    public override void SkillDuration(GameObject gameObject)
    {
        if (skillIsActive)
        {
            skillDuration += Time.deltaTime;
            if (skillDuration > durationTimeOut)
            {
                skillIsActive = false;
                gaterableObj.SetActive(false);
                skillDuration = 0;
            }
        }
    }
    public override void SkillReset(SkillSO activedSkills)
    {
        if (activationMethod == ActivationMethod.RefreshingTime)
        {
            if (!skillIsActive & !canCast)
            {
                resetCounter += Time.deltaTime;
                if (resetCounter >= timeNeededRefresh)
                {
                    resetCounter = 0;
                    canCast = true;
                }
            }
        }
        else if (activationMethod == ActivationMethod.RefreshingHits)
        {
            if (!skillIsActive & !canCast)
            {
                if (hitsImpacted >= hitsNeededToRefresh)
                {
                    hitsImpacted = 0;
                    canCast = true;
                }
            }
        }
        else if (activationMethod == ActivationMethod.InstantRefreshing)
        {
            if (!skillIsActive & !canCast)
            {
                canCast = true;
            }
        }
    }
}