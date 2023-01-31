using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected EnemyController baseController;
    protected EnemyMovement movement;
    protected Animator anim;

    protected bool active;

    public abstract bool Validate();

    protected virtual void Start()
    {
        baseController = GetComponentInParent<EnemyController>();
        movement = GetComponentInParent<EnemyMovement>();
        anim = baseController.GetComponent<Animator>();
        active = true;
    }

    public virtual void Init()
    {
    }

    public abstract void UpdateBehaviour();

    public virtual void Release()
    {
        active = false;
    }
}
