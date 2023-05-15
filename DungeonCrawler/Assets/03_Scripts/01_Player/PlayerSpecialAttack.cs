using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSpecialAttack : MonoBehaviour
{
    Animator anim;
    int indx;
    [SerializeField] Image iconSkill1, iconSkill2, iconSkill3;

    [SerializeField] LayerMask hitMask;
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] SkillSO[] skills;
    [SerializeField] GameObject[] skillObject = new GameObject[3];
    bool[] skillStart = new bool[3];
    public bool eventCanRelease;
    private void Start()
    {
        //skills[0] = skillSet.skills[0];
        //skills[1] = skillSet.skills[1];
        //skills[2] = skillSet.skills[2];
        TryGetComponent<Animator>(out anim);
        if(anim != null)
        {
            skills[0].anim = anim;
            skills[1].anim = anim;
            skills[1].anim = anim;
        }
        iconSkill1.sprite = skills[0].image;
        iconSkill2.sprite = skills[1].image;
        iconSkill3.sprite = skills[2].image;
    }
    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Land")) SkillEffect(i);
            SkillDuration(i);
            SkillReset(i);
            AnimationEvent(i);

        }
    }
    private void OnTriggerEnter(Collider other)
    {

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
        if (skills[0].canCast & !skillStart[0]) 
        {
            if (skills[0].hasAnimation) if (anim != null) skills[0].SetAnimatorTrigger(anim);
            skillStart[0] = true;
            indx = 0;
            if (skills[0].hasPrefab)
            {
                if (skills[0].hasAnimationEvent)
                {
                    skillObject[0] = skills[2].InvokeMethod(skills[0].prefab, skills[0].instancePosition + transform.position, skills[0].instanceRotation, transform);
                    eventCanRelease = true;
                    skillObject[0].SetActive(false);
                }
                else skillObject[0] = skills[0].InvokeMethod(skills[0].prefab, skills[0].instancePosition + transform.position, skills[0].instanceRotation, transform);
            }
        }
    }
    public void CastSkill2()
    {
        if (skills[1].canCast & !skillStart[1])
        {
            if (skills[1].hasAnimation) if (anim != null) skills[1].SetAnimatorTrigger(anim);
            skillStart[1] = true;
            if (skills[1].hasPrefab)
            {
                if (skills[1].hasAnimationEvent)
                {
                    skillObject[1] = skills[1].InvokeMethod(skills[1].prefab, skills[1].instancePosition + transform.position, skills[1].instanceRotation, transform);
                    eventCanRelease = true;
                    skillObject[1].SetActive(false);
                }
                else skillObject[1] = skills[1].InvokeMethod(skills[1].prefab, skills[1].instancePosition + transform.position, skills[1].instanceRotation, transform);
            }
        }
    }
    public void CastSkill3()
    {
        if (skills[2].canCast & !skillStart[2])
        {
            if (skills[2].hasAnimation) if (anim != null) skills[2].SetAnimatorTrigger(anim);
            skillStart[2] = true;
            if (skills[2].hasPrefab)
            {
                if (skills[2].hasAnimationEvent)
                {
                    skillObject[2] = skills[2].InvokeMethod(skills[2].prefab, skills[2].instancePosition + transform.position, skills[2].instanceRotation, transform);
                    skillObject[2].SetActive(false);
                    eventCanRelease = true; ;
                }
                else skillObject[2] = skills[2].InvokeMethod(skills[2].prefab, skills[2].instancePosition + transform.position, skills[2].instanceRotation, transform);
            }
        }
    }
    public void SkillDuration(int index)
    {
        if (skills[index].skillIsActive)
        {
            if(skills[index].hasDuration)
            {
                if(skillObject[index] != null)
                {
                    skills[index].skillDurationCounter += Time.deltaTime;
                    if (skills[index].skillDurationCounter > skills[index].skillDurationTime)
                    {
                        Destroy(skillObject[index]);
                        skillObject[index] = null;
                        skillStart[index] = false;
                        skills[index].skillDurationCounter = 0;
                        skills[index].skillIsActive = false;
                    }
                }
            }
            else
            {
                Destroy(skillObject[index], 0.5f);
                skillObject[index] = null;
                skillStart[index] = false;
                skills[index].skillDurationCounter = 0;
                skills[index].skillIsActive = false;
            }
        }
    }
    public void SkillReset(int index)
    {
        if (!skills[index].skillIsActive & !skills[index].canCast)
        {
            if (skills[index].activationMethod == ActivationMethod.RefreshingTime)
            {
                skills[index].skillResetCounter += Time.deltaTime;
                if (skills[index].skillResetCounter >= skills[index].skillResetTime)
                {
                    skills[index].skillResetCounter = 0;
                    skills[index].effectDone = false;
                    skills[index].canCast = true;
                }
            }
            else if (skills[index].activationMethod == ActivationMethod.RefreshingHits)
            {
                if (skills[index].skillCurrentImpacts >= skills[index].skillResetImpacts)
                {
                    skills[index].skillCurrentImpacts = 0;
                    skills[index].effectDone = false;
                    skills[index].canCast = true;
                }
            }
            else if (skills[index].activationMethod == ActivationMethod.InstantRefreshing)
            {
                skills[index].effectDone = false;
                skills[index].canCast = true;
            }
        }
    }
    void SkillEffect(int index)
    {
        if(skills[index].effect != null & !skills[index].effectDone)
        {
            skillObject[index] = skills[index].DoEffect(transform);
            Debug.Log(skillObject[index]);

        }
    }
    void AnimationEvent(int index)
    {
        if (skills[index].hasAnimation & skills[index].hasAnimationEvent)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(skills[index].animClipInfo) & eventCanRelease)
            {
                float animationCurTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animationCurTime > 0.25f)
                {
                    skillObject[index].SetActive(true);
                    eventCanRelease = false;
                }
            }
        }
    }
    void ResetSkillParams()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].canCast = true;
            skills[i].effectDone = false;
            skills[i].doDamage = true;
            skills[i].skillIsActive = false;
            skills[i].skillDurationCounter = 0;
            skills[i].skillResetCounter = 0;
            skills[i].skillCurrentImpacts = 0;
        }
    }
    private void OnDisable()
    {
        ResetSkillParams();
    }
}
