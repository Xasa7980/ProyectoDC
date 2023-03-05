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
    [SerializeField] string _displayName;
    [Multiline]
    [SerializeField] string _description;
    [SerializeField] Sprite _image;

    public string displayName => _displayName;
    public string description => _description;
    public Sprite image => _image;

    public ActivationMethod activationMethod;
    public HitMethod hitMethod;
    public GameObject prefab;
    public GameObject hitEffect;
    public GameObject gaterableObj;

    public Vector3 instancePosition;
    public Quaternion instanceRotation;
    public float speed;

    public bool canCast = true; //Si tiene tiempo de refreso y se ha refrescado

    public abstract GameObject InvokeMethod(GameObject obj, Vector3 pos, Quaternion rot, Transform transform);

    public float damage;
    public float hitRadius;
    public abstract void TakeDamage(GameObject gameObject, Transform transform, LayerMask hitMask);

    public bool skillIsActive;

    public float skillDuration;
    public float durationTimeOut;
    public abstract void SkillDuration(GameObject obj);

    public float resetCounter;
    public float timeNeededRefresh;
    public float hitsImpacted;
    public float hitsNeededToRefresh;
    public abstract void SkillReset(/*List<SkillSO> */SkillSO activedSkills);

}
