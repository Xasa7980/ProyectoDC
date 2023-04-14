using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region LogicParameters
    [SerializeField] LayerMask impactMask;
    [SerializeField] Transform hitLocation;
    [SerializeField] float radius;
    [SerializeField] float strenght;
    [SerializeField] float refreshCounter;
    [SerializeField, Range(1.35f,3)] float refreshTime = 2;
    [SerializeField] FMODUnity.EventReference kickFX;
    [SerializeField] bool castMeleeAtk = true;

    [SerializeField] GameObject impactEffect;
    #endregion
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        ResetMeleeAttack();
    }
    public void DoMeleeAttack()
    {
        if (castMeleeAtk)
        {
            anim.SetTrigger("Attack");
            FMODUnity.RuntimeManager.PlayOneShot(kickFX);
            Collider[] colls = Physics.OverlapSphere(hitLocation.position, radius, impactMask);
            if(colls.Length > 0)
            {
                foreach(Collider coll in colls)
                {
                    if (coll.TryGetComponent<iDamageable>(out iDamageable damageable))
                    {
                        damageable.ApplyDamage(Damage(strenght));
                        castMeleeAtk = false;
                    }
                }
            }
            else castMeleeAtk = false;
        }
    }
    float Damage(float str)
    {
        return str;
    }
    void ResetMeleeAttack()
    {
        if (!castMeleeAtk)
        {
            refreshCounter += Time.deltaTime;
            if (refreshCounter > refreshTime)
            {
                refreshCounter = 0;
                castMeleeAtk = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitLocation.position, radius);
    }
}
