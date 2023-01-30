using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour, iDamageable
{
    [SerializeField] string _playerName;
    public string playerName { get; set; }

    [SerializeField] Component[] removeComponentsOnDead;

    [SerializeField] bool hasUI;
    [SerializeField] Image healthBar;
    [SerializeField] float smoothness = 1;
    [SerializeField] FillMethod fillMethod;

    enum FillMethod
    {
        Filled,
        Slider
    }

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
    float currentPercent;

    #endregion

    [SerializeField] protected UnityEvent OnDieEvent = new UnityEvent();
    public System.Action OnDie = delegate { };

    [SerializeField] protected UnityEvent OnDamageReceivedEvent = new UnityEvent();
    public System.Action OnDamageReceived = delegate { };

    public virtual void ApplyDamage(float damage2apply)
    {
        if (shieldActived) currentHeath -= BlockDamage(damage2apply, reductionBonus);
        else currentHeath -= damage2apply;

        if (currentHeath <= 0)
            Die();
        else
        {
            OnDamageReceived();
            OnDamageReceivedEvent.Invoke();
        }

        currentHeath = Mathf.Clamp(currentHeath, 0, _maxHealth);
    }

    public virtual void Die()
    {
        //Die
        OnDieEvent.Invoke();
        OnDie();

        foreach (Component component in removeComponentsOnDead)
        {
            Destroy(component);
        }
    }

    public virtual void Recover(float healthToRecover)
    {
        currentHeath += healthToRecover;
        currentHeath = Mathf.Clamp(currentHeath, 0, _maxHealth);
    }

    protected virtual void UpdateUI()
    {
        //En el caso de que se decida crear barras de salud para los enemigos
        switch (fillMethod)
        {
            case FillMethod.Slider:
                Vector3 targetPos = Vector3.right * -2400;
                healthBar.transform.localPosition = Vector3.Lerp(targetPos, Vector3.zero, currentPercent);
                break;
        }
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
        currentPercent = percent;
    }

    protected virtual void Update()
    {
        ResetShield();
        currentPercent = Mathf.Lerp(currentPercent, percent, smoothness * Time.deltaTime);
        UpdateUI();
    }
}
