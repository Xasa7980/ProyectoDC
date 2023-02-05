using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Create Skill")]
public class SkillSO : ScriptableObject
{
    public LayerMask impactMask;
    public GameObject prefab;
    public GameObject hitEffect;
    public float damage;
    public float speed;
    public bool hasResetTime;

    public bool resetSkill;
    public float skillDuration;
    public float skillResetTime;
    void TakeDamage(GameObject gameObject, Transform transform)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime, impactMask))
        {
            if (hit.collider.TryGetComponent<RaycastEventReciever>(out RaycastEventReciever reciever))
            {
                reciever.TryInvoke(RaycastEventReciever.RaycastEventType.Shoot, ray);
            }

            if (hit.collider.TryGetComponent<iDamageable>(out iDamageable damageable))
            {
                damageable.ApplyDamage(damage);
            }
            Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(gameObject);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
    public void ActivateSkill(bool activation, GameObject gameObject, Transform transform)
    {
        if (!hasResetTime)
        {
            if (!activation) return;

            hitEffect.SetActive(true);
            prefab.SetActive(true);
            TakeDamage(gameObject, transform);
        }
        else
        {
            if (resetSkill)
            {
                if (!activation) return;

                hitEffect.SetActive(true);
                prefab.SetActive(true);
                TakeDamage(gameObject, transform);
                resetSkill = false;
            }
        }
    }
    public void ResetSkill(float timeGetReady,float timeToReady)
    {
        if (!hasResetTime) return;
        timeGetReady += Time.deltaTime;
        if(timeGetReady > timeToReady)
        {
            resetSkill = true;
            timeGetReady = 0;
        }
    }
}
