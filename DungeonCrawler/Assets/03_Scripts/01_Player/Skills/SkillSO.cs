using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationMethod
{
    RefreshingTime,
    RefreshingHits,
    InstantRefreshing
}
public enum HitMethod
{
    Raycast,
    OverlapSphere,
    ParticleCollision
}
public abstract class SkillSO : ScriptableObject
{
    public ActivationMethod activationMethod;
    public HitMethod hitMethod;
    public GameObject prefab;
    public GameObject hitEffect;

    public float damage;
    public float speed;
    public bool resetSkill = true; //Si tiene tiempo de refreso y se ha refrescado
    public float hitRadius;
    public float skillDuration;
    public float timeNeededRefresh;
    public float hitsImpacted;
    public float hitsNeededToRefresh;

    public abstract void InvokeMethod(Vector3 pos, GameObject obj);
    public abstract void TakeDamage(GameObject gameObject, Transform transform, LayerMask hitMask);
    public abstract void ActivateSkill(ActivationMethod activation, GameObject gameObject, Transform transform, LayerMask hitMask, bool reset);
}
