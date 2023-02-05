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
    float timeToGetReady;
    float timeToBeReady;
    bool castMeleeAtk = true;
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
            Collider[] colls = Physics.OverlapSphere(hitLocation.position, radius, impactMask);
            if(colls.Length > 0)
            {
                foreach(Collider coll in colls)
                {
                    //if (coll.TryGetComponent<RaycastEventReciever>(out RaycastEventReciever reciever))
                    //{
                    //    reciever.TryInvoke(RaycastEventReciever.RaycastEventType.Hit, ray);
                    //}

                    if (coll.TryGetComponent<iDamageable>(out iDamageable damageable))
                    {
                        damageable.ApplyDamage(Damage(strenght));
                        castMeleeAtk = false;
                    }
                }
               
            }
        }
    }
    float Damage(float str/*, float enemyDefense, float redBonus*/)
    {
        float dmgReducted = strenght /*- (strenght * redBonus)*/;
        castMeleeAtk = false;
        return dmgReducted /*- enemyDefense*/;
    }
    void ResetMeleeAttack()
    {
        if (!castMeleeAtk)
        {
            timeToGetReady += Time.deltaTime;
            if (timeToGetReady > timeToBeReady)
            {
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
