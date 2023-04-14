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
    TriggerCollision
}

public enum DamageMethod
{
    InstantDamage,
    DamageOvertime
}
public abstract class SkillSO : ScriptableObject
{
    [SerializeField] string _displayName;
    [Multiline]
    [SerializeField] string _description;
    [SerializeField] Sprite _image;

    public string displayName => _displayName;
    public string description => _description;
    public Sprite image => _image;

    public ActivationMethod activationMethod;
    public HitMethod hitMethod;
    public DamageMethod damageMethod;

    public GameObject prefab;
    public GameObject hitEffect;

    public Vector3 instancePosition;
    public Quaternion instanceRotation;
    public float speed;

    public bool canCast = true; //Si tiene tiempo de refreso y se ha refrescado

    public abstract GameObject InvokeMethod(GameObject obj, Vector3 pos, Quaternion rot, Transform transform);

    public float damage;
    public float hitRadius;
    public abstract void TakeDamage(GameObject target, Transform transform, LayerMask hitMask);

    public float continueCounter;
    public float continueTime;
    public bool doDamage;
    public abstract void DamageCounter();

    public bool skillIsActive;

    public float skillCounter;
    public float skillTime;
    public float resetCounter;
    public float resetTime;
    public float hitsImpacted;
    public float hitsNeededToRefresh;
}
