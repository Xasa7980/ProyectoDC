using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] Image iconSkill1, iconSkill2, iconSkill3;

    [SerializeField] LayerMask hitMask;
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] SkillSO[] skills;
    [SerializeField] GameObject[] skillObject = new GameObject[3];
    bool[] skillStart = new bool[3];
    private void Start()
    {
        //skills[0] = skillSet.skills[0];
        //skills[1] = skillSet.skills[1];
        //skills[2] = skillSet.skills[2];
        iconSkill1.sprite = skills[0].image;
        iconSkill2.sprite = skills[1].image;
        iconSkill3.sprite = skills[2].image;

    }
    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            SkillDuration(i);
            SkillReset(i);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ReleaseActualSkillFunctions(other.gameObject);
    }
    void ReleaseActualSkillFunctions(GameObject target)
    {
        if(target.layer == 9)
        {
            if (skillStart[0])
            {
                skills[0].DamageCounter();
                if (skills[0].doDamage) skills[0].TakeDamage(gameObject, transform, hitMask);
                skillStart[0] = false;
            }
            else if (skillStart[1])
            {
                skills[1].DamageCounter();
                if (skills[1].doDamage) skills[1].TakeDamage(gameObject, transform, hitMask);
                skillStart[1] = false;
            }
            else if (skillStart[2])
            {
                skills[2].DamageCounter();
                if (skills[2].doDamage) skills[2].TakeDamage(gameObject, transform, hitMask);
                skillStart[2] = false;
            }
        }
    }

    public void CastSkill1()
    {
        if (skills[0].canCast) 
        {
            skills[0].skillIsActive = true;
            skillObject[0] = skills[0].InvokeMethod(skills[0].prefab, skills[0].instancePosition, skills[0].instanceRotation, transform);
            skillStart[0] = true;
            skills[0].canCast = false;
        }
    }
    public void CastSkill2()
    {
        if (skills[1].canCast)
        {
            skills[1].skillIsActive = true;
            skillObject[1] = skills[1].InvokeMethod(skills[1].prefab, skills[1].instancePosition, skills[1].instanceRotation, transform);
            skillStart[1] = true;
            skills[1].canCast = false;
        }
    }
    public void CastSkill3()
    {
        if (skills[2].canCast)
        {
            skills[2].skillIsActive = true;
            skillObject[2] = skills[2].InvokeMethod(skills[2].prefab, skills[2].instancePosition, skills[2].instanceRotation, transform);
            skillStart[2] = true;
            skills[2].canCast = false;
        }
    }
    public void SkillDuration(int index)
    {
        if (skills[index].skillIsActive & skillObject[index] != null)
        {
            skills[index].skillCounter += Time.deltaTime;
            if (skills[index].skillCounter > skills[index].skillTime)
            {
                if (skillObject[index] != null)
                {
                    Destroy(skillObject[index]);
                    skillObject[index] = null;
                }
                skills[index].skillCounter = 0;
                skills[index].skillIsActive = false;
            }
        }
    }
    public void SkillReset(int index)
    {
        if (!skills[index].skillIsActive)
        {
            if (skills[index].activationMethod == ActivationMethod.RefreshingTime)
            {
                if (!skills[index].skillIsActive & !skills[index].canCast)
                {
                    skills[index].resetCounter += Time.deltaTime;
                    if (skills[index].resetCounter >= skills[index].resetTime)
                    {
                        skills[index].resetCounter = 0;
                        skills[index].canCast = true;
                    }
                }
            }
            else if (skills[index].activationMethod == ActivationMethod.RefreshingHits)
            {
                if (!skills[index].skillIsActive & !skills[index].canCast)
                {
                    if (skills[index].hitsImpacted >= skills[index].hitsNeededToRefresh)
                    {
                        skills[index].hitsImpacted = 0;
                        skills[index].canCast = true;
                    }
                }
            }
            else if (skills[index].activationMethod == ActivationMethod.InstantRefreshing)
            {
                if (!skills[index].skillIsActive & !skills[index].canCast)
                {
                    skills[index].canCast = true;
                }
            }
        }
    }
    void ResetSkillParams()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].canCast = true;
            skills[i].doDamage = true;
            skills[i].skillIsActive = true;
            skills[i].resetCounter = 0;
            skills[i].hitsImpacted = 0;
        }
    }
    private void OnDisable()
    {
        ResetSkillParams();
    }
}
