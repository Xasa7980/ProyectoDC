using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, iDamageable
{
    [SerializeField] float _maxHealth = 100;

    public float currentHeath { get; private set; }

    public float maxHealth => _maxHealth;

    public float percent => currentHeath / maxHealth;

    [SerializeField] protected UnityEvent OnDieEvent = new UnityEvent();
    public System.Action OnDie = delegate { };

    public virtual void ApplyDamage(float damage2apply)
    {
        currentHeath -= damage2apply;

        if (currentHeath <= 0)
            Die();

        currentHeath = Mathf.Clamp(currentHeath, 0, _maxHealth);
        UpdateUI();
    }

    public virtual void Die()
    {
        //Die
        OnDieEvent.Invoke();
        OnDie();
    }

    public virtual void Recover(float healthToRecover)
    {
        currentHeath += healthToRecover;
        currentHeath = Mathf.Clamp(currentHeath, 0, _maxHealth);
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        //En el caso de que se decida crear barras de salud para los enemigos
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHeath = maxHealth;
    }
}
