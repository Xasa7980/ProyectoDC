using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] LayerMask hitMask;
    [SerializeField] SkillSO skillObjectSlot1;
    [SerializeField] SkillSO skillObjectSlot2;
    [SerializeField] Transform hitLocation;

    [SerializeField] float counterToRefresh1;
    [SerializeField] float counterToRefresh2;
    public bool ResetSkill1 { get { return skillObjectSlot1.resetSkill; } set { skillObjectSlot1.resetSkill = value;} }
    public bool ResetSkill2 { get { return skillObjectSlot2.resetSkill; } set { skillObjectSlot2.resetSkill = value; } }

    public float HitsImpacted { get { return skillObjectSlot1.hitsImpacted; } set { skillObjectSlot1.hitsImpacted = value; } }

    void Update()
    {
        if(skillObjectSlot1 != null & skillObjectSlot1.activationMethod == ActivationMethod.RefreshingTime) counterToRefresh1 += Time.deltaTime;
        if (skillObjectSlot2 != null & skillObjectSlot2.activationMethod == ActivationMethod.RefreshingTime) counterToRefresh2 += Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
            Debug.Log(other.name);

        if (other.gameObject.layer == 9)
        {
            skillObjectSlot1.TakeDamage(other.gameObject, transform, hitMask);
            skillObjectSlot2.TakeDamage(other.gameObject, transform, hitMask);
        }
    }
    public void ActivateSkill1()
    {
        ResetMethod(skillObjectSlot1.activationMethod, counterToRefresh1, skillObjectSlot1);
        skillObjectSlot1.ActivateSkill(skillObjectSlot1.activationMethod, skillObjectSlot1.prefab, hitLocation, hitMask, ResetSkill1);
        ResetSkill1 = false;
    }
    public void ActivateSkill2()
    {
        ResetMethod(skillObjectSlot2.activationMethod, counterToRefresh2, skillObjectSlot2);
        skillObjectSlot2.ActivateSkill(skillObjectSlot2.activationMethod, skillObjectSlot2.prefab, hitLocation, hitMask, ResetSkill2);
        ResetSkill2 = false; 

        skillObjectSlot2.ActivateSkill(skillObjectSlot2.activationMethod, skillObjectSlot2.prefab, hitLocation, hitMask, ResetSkill1);
    }
    public void ResetMethod(ActivationMethod reset, float counter, SkillSO skillObject)
    {
        if (reset == ActivationMethod.RefreshingTime) SkillReset(counter, skillObject.timeNeededRefresh);

        else if (reset == ActivationMethod.RefreshingHits) SkillReset(HitsImpacted, skillObject.hitsNeededToRefresh);

        else if (reset == ActivationMethod.InstantRefreshing) SkillReset(0, 0);
    }
    public void SkillReset(float counterToReset, float timeToGetReady)
    {
        if (counterToReset >= timeToGetReady)
        {
            ResetSkill1 = true;
            counterToRefresh1 = 0;
            HitsImpacted = 0;
        }
    }
}
