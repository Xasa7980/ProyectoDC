using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, iDamageable
{
    [SerializeField] string _playerName;
    public string playerName { get; set; }

    #region Defense&Shield
    bool shieldActived = false;
    bool shieldNeedRefresh;

    [SerializeField] float _shieldTime;
    float timeToResetShield;
    [SerializeField] float _timeShieldIsReady;
    [SerializeField] float _defense = 5;
    [SerializeField] float _reductionBonus = 0.05f;
    public float defense { get; set; }

    public float reductionBonus { get; set; }

    public float timeShieldIsReady { get; set; }
    public float shieldTime { get; set; }
    #endregion
    #region HealthParameters
    [SerializeField] float _maxHealth = 100;

    public float currentHeath { get; private set; }

    public float maxHealth => _maxHealth;

    public float percent => currentHeath / maxHealth;

    #endregion

    [SerializeField] protected UnityEvent OnDieEvent = new UnityEvent();
    public System.Action OnDie = delegate { };

    public virtual void ApplyDamage(float damage2apply)
    {
        if (shieldActived) currentHeath -= BlockDamage(damage2apply, reductionBonus);
        else currentHeath -= damage2apply;

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

    public virtual void DoShield()
    {
        if(!shieldNeedRefresh)
        {
            shieldActived = true;
            shieldTime += Time.deltaTime;
            if (shieldTime > 3)
            {
                shieldNeedRefresh = true;
                shieldTime = 0;
                shieldActived = false;
            }
        }
    }
    void ResetShield()
    {
        if (shieldNeedRefresh)
        {
            timeToResetShield += Time.deltaTime;
            if (timeToResetShield > timeShieldIsReady)
            {
                timeToResetShield = 0;
                shieldNeedRefresh = false;
            }

        }
    }
    float BlockDamage(float dmg, float redBonus)
    {
        float dmgReducted = dmg - (dmg * redBonus);
        return dmgReducted - defense;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHeath = maxHealth;
    }
    protected virtual void Update()
    {
        ResetShield();
    }
}
