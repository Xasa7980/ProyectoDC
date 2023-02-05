using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] ScriptableObject skillObjectSlot1;
    [SerializeField] ScriptableObject skillObjectSlot2;

    [SerializeField] Transform hitLocation;
    bool activation;
    float timeGetReady;
    [SerializeField] float timeToReady;

    void Update()
    {
        if (skillObjectSlot1 != null) ObjectSO1().ResetSkill(timeGetReady,timeToReady);
        if (skillObjectSlot2 != null) ObjectSO2().ResetSkill(timeGetReady, timeToReady);
    }

    public void ActivateSkill1()
    {
        if(skillObjectSlot1 != null) ObjectSO1().ActivateSkill(activation, ObjectSO1().prefab, hitLocation);
    }
    public void ActivateSkill2()
    {
        if (skillObjectSlot2 != null) ObjectSO2().ActivateSkill(activation, ObjectSO2().prefab, hitLocation);
    }

    SkillSO ObjectSO1()
    {
        return (SkillSO)skillObjectSlot1;
    }
    SkillSO ObjectSO2()
    {
        return (SkillSO)skillObjectSlot2;
    }
}
