using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] LayerMask hitMask;
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] List<SkillSO> activedSkills;
    [SerializeField] List<GameObject> skillObject;
    void Update()
    {
        ReleaseActualSkillFunctions();
    }
    void ReleaseActualSkillFunctions()
    {
        if (activedSkills.Count > -1)
        {
            foreach (SkillSO skill in activedSkills)
            {
                skill.SkillDuration(skill.gaterableObj);
                skill.SkillReset(skill);
            }
        }
    }
    void SetActiveSkill()
    {
        activedSkills.Add(GetActiveSkill());
    }
    SkillSO GetActiveSkill()
    {
        for (int i = 0; i < skillSet.skills.Length; i++)
        {
            if(skillSet.skills[i].activationMethod == ActivationMethod.InstantRefreshing)
            {
                return skillSet.skills[i];
            }
            else if (skillSet.skills[i].skillIsActive)
            {
                return skillSet.skills[i];
            }
        }
        return null;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 9)
        {
            skillSet.skills[0].TakeDamage(other, transform, hitMask);
            skillSet.skills[1].TakeDamage(other, transform, hitMask);
            skillSet.skills[2].TakeDamage(other, transform, hitMask);
        }
    }
    public void CastSkill1()
    {
        if (skillSet.skills[0].canCast) 
        {
            ActivateSkill(skillSet.skills[0], skillSet.skills[0].instancePosition, skillSet.skills[0].instanceRotation);
            SetActiveSkill();
        }
    }
    public void CastSkill2()
    {
        if (skillSet.skills[1].canCast)
        {
            ActivateSkill(skillSet.skills[1], skillSet.skills[1].instancePosition, skillSet.skills[1].instanceRotation);
            SetActiveSkill();
        }
    }
    public void CastSkill3()
    {
        if (skillSet.skills[2].canCast)
        {
            ActivateSkill(skillSet.skills[2], skillSet.skills[2].instancePosition, skillSet.skills[2].instanceRotation);
            SetActiveSkill();
        }
    }
    public void ActivateSkill(SkillSO skill, Vector3 pos, Quaternion rot)
    {
        if (skill.canCast & skill.activationMethod != ActivationMethod.InstantRefreshing)
        {
            skill.skillIsActive = true;
            skill.gaterableObj = skill.InvokeMethod(skill.prefab, pos, rot, transform);
            skill.canCast = false;
        }

        if (skill.activationMethod == ActivationMethod.InstantRefreshing)
        {
            skill.skillIsActive = true;
            skill.gaterableObj = skill.InvokeMethod(skill.prefab, pos, rot, transform);
            skill.canCast = false;
        }
    }
}
