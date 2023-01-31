using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Type", menuName = "Top Down Engine/Attack Type")]
public class AttackType : ScriptableObject
{
    [SerializeField] string _displayName;
    [SerializeField] int _baseDamage;
    [SerializeField] AnimationClip _attackAnimation;
    [SerializeField] Projectile projectile;
    public string displayName => _displayName;
    public int baseDamage => _baseDamage;

    [SerializeField] GameObject attackEffect;

    public void ConfigureAnimator(AnimatorOverrideController controller)
    {
        controller["Dummy_Attack_Melee"] = _attackAnimation;
    }

    public void Perform(Vector3 position)
    {

    }

    public void Perform(iDamageable target)
    {
        target.ApplyDamage(baseDamage);
    }

    public void Perform(RifleController rifle)
    {
        rifle.Shoot(projectile);
    }
}
