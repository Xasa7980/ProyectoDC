using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float strenght;
    float timeToResetMelee, timeToResetRanged;
    float timeToMeleeReady, timeToRangedReady;
    bool castMeleeAtk = true;
    bool castRangedAtk = true;


    private void Update()
    {
        ResetMeleeAttack();
        ResetRangedAttack();
    }
    public void DoAttack()
    {
        // if(castMeleeAtk) // ENEMY.HEALTH -= Attack(strenght,//enemy def, //enemy dmg reduction)
        Debug.Log("hi");
    }
    public void DoRangeAttack()
    {
        //if(castRangedAtk) // ENEMY.HEALTH -= RangeAttack(strenght,//enemy def, //enemy dmg reduction)
        Debug.Log("hi2");

    }
    float Attack(float str, float enemyDefense, float redBonus)
    {
        float dmgReducted = strenght - (strenght * redBonus);
        castMeleeAtk = false;
        return dmgReducted - enemyDefense;
    }
    float RangeAttack(float str, float enemyRangeDefense, float redBonus)
    {
        float dmgReducted = strenght - (strenght * redBonus);
        castRangedAtk = false;
        return dmgReducted - enemyRangeDefense;
    }
    void ResetMeleeAttack()
    {
        if (!castMeleeAtk)
        {
            timeToResetMelee += Time.deltaTime;
            if (timeToResetMelee > timeToMeleeReady)
            {
                castMeleeAtk = true;
            }
        }
    }
    void ResetRangedAttack()
    {
        if (!castRangedAtk)
        {
            timeToResetMelee += Time.deltaTime;
            if (timeToResetMelee > timeToMeleeReady)
            {
                castRangedAtk = true;
            }
        }
    }

}
