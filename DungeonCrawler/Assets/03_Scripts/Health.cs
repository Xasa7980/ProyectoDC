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
    [SerializeField] Image healthBar, energyBar;
    [SerializeField] float smoothness = 1;
    [SerializeField] FillMethod fillMethod;
    
    enum FillMethod
    {
        Filled,
        Slider
    }

    #region Defense&Shield
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject[] playerWithShield;
    [SerializeField] GameObject[] playerWithoutShield;
    bool shieldIsActived;
    [SerializeField] float _defense = 5;
    [SerializeField] float _reductionBonus = 0.05f;
    #endregion
    #region HealthParameters
    [SerializeField] float _maxHealth = 100;

    public float currentHealth { get; private set; }

    public float maxHealth => _maxHealth;

    public float healthPercent => currentHealth / maxHealth;
    float currentHealthPercent;

    #endregion

    #region EnergyParameters
    [SerializeField] float _maxEnergy = 6;
    bool canStartEnergyRecovering;
    [SerializeField] float timeToEnergyRecovering;
    [SerializeField] float energyRecoveringStarts = 6;

    public float currentEnergy { get; private set; }

    public float maxEnergy => _maxEnergy;

    public float energyPercent => currentHealth / maxHealth;
    float currentEnergyPercent;

    #endregion

    [SerializeField] protected UnityEvent OnDieEvent = new UnityEvent();
    public System.Action OnDie = delegate { };

    [SerializeField] protected UnityEvent OnDamageReceivedEvent = new UnityEvent();
    public System.Action OnDamageReceived = delegate { };

    public virtual void ApplyDamage(float damage2apply)
    {
        if (currentEnergy > 0) BlockDamage();
        else
        {
            currentHealth -= damage2apply;
            timeToEnergyRecovering = 0;
        }

        if (currentHealth <= 0)
            Die();
        else
        {
            OnDamageReceived();
            OnDamageReceivedEvent.Invoke();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
        currentEnergy = Mathf.Clamp(currentEnergy, 0, _maxEnergy);
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
        currentHealth += healthToRecover;
        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
    }

    protected virtual void UpdateUI()
    {
        //En el caso de que se decida crear barras de salud para los enemigos
        switch (fillMethod)
        {
            case FillMethod.Slider:
                Vector3 targetPos = Vector3.right * -2400;
                healthBar.transform.localPosition = Vector3.Lerp(targetPos, Vector3.zero, currentHealthPercent);
                energyBar.transform.localPosition = Vector3.Lerp(targetPos, Vector3.zero, currentEnergyPercent);
                break;
        }
    }
    void ActiveShield()
    {
        if (currentEnergy >= maxEnergy)
        {
            shieldIsActived = true;
            canStartEnergyRecovering = false;
            timeToEnergyRecovering = 0;

            if (playerWithShield.Length > 0)
            {
                for (int i = 0; i < playerWithShield.Length; i++)
                {
                    playerWithShield[i].SetActive(true);
                    playerWithoutShield[i].SetActive(false);
                }
            }
        }
        else if (currentEnergy <= 0)
        {
            shieldIsActived = false;
            canStartEnergyRecovering = true;

            if (playerWithShield.Length > 0)
                for (int i = 0; i < playerWithShield.Length; i++)
                {
                    playerWithShield[i].SetActive(false);
                    playerWithoutShield[i].SetActive(true);
                }
        }
    }

    void EnergyWaste()
    {
        if (shieldIsActived & !canStartEnergyRecovering)
        {
            currentEnergy -= Time.deltaTime /** energyWasteTicks*/;
        }
    }
    void RecoverEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0, _maxEnergy);
        if (!shieldIsActived & canStartEnergyRecovering)
        {
            timeToEnergyRecovering += Time.deltaTime;
            if (timeToEnergyRecovering > energyRecoveringStarts)
            {
                currentEnergy += Time.deltaTime /** energyRecoveringTicks*/;
            }
        }
    }
    void BlockDamage()
    {
        currentEnergy--;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentHealthPercent = healthPercent;

        currentEnergy = maxEnergy;
        currentEnergyPercent = energyPercent;

    }

    protected virtual void Update()
    {
        RecoverEnergy();
        EnergyWaste();
        ActiveShield();
        currentHealthPercent = Mathf.Lerp(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        currentEnergyPercent = Mathf.Lerp(currentEnergyPercent, energyPercent, smoothness * Time.deltaTime);
        UpdateUI();
    }
}
