using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]

public class Health : MonoBehaviour, iDamageable
{
    [SerializeField] string _playerName;
    public string playerName { get; set; }

    [SerializeField] Component[] removeComponentsOnDead;
    Animator anim;

    [SerializeField] bool hasUI;
    [SerializeField] Image healthBar, energyBar;
    [SerializeField] float smoothness = 1;
    [SerializeField] FillMethod fillMethod;

    public bool isDeath { get; private set; }
    
    enum FillMethod
    {
        Filled,
        Slider,
        None
    }

    #region Defense&Shield
    [SerializeField] GameObject forceFieldShield;
    //[SerializeField] GameObject[] playerWithShield;
    //[SerializeField] GameObject[] playerWithoutShield;
    bool shieldIsActived;
    #endregion
    #region HealthParameters
    [Header("Health Parameters")]
    [SerializeField] float _maxHealth = 100;

    public float currentHealth { get; private set; }

    public float maxHealth => _maxHealth;

    public float healthPercent => currentHealth / maxHealth;
    float currentHealthPercent;

    public void SetMaxHealth(float maxHealth) => this._maxHealth = maxHealth;
    public void SetHealth(float health) => this.currentHealth = health;
    #endregion
    #region EnergyParameters
    [Header("Energy Parameters")]
    [SerializeField] float _maxEnergy = 6;
    public float currentEnergy;

    public float energyPercent => currentEnergy / maxEnergy;
    float currentEnergyPercent;

    public float maxEnergy => _maxEnergy;

    [SerializeField] float countdownToRefillEnergy = 4;
    float energyRefillCount;
    [SerializeField] float energyRefillSpeed = 1;

    public void SetMaxEnergy(float maxEnergy) => this._maxEnergy = maxEnergy;
    public void SetEnergyRefillSpeed(float refillSpeed) => this.energyRefillSpeed = refillSpeed;
    #endregion
    #region FXParameters
    [SerializeField] FMODUnity.EventReference brokenShieldInputSound;
    [SerializeField] FMODUnity.EventReference rechargingShieldInputSound;
    [SerializeField] FMODUnity.EventReference rechargedShieldInputSound;
    bool shieldRecharged;
    float rechargeFxFrecuency;
    [SerializeField] FMODUnity.EventReference takeDamageInputSound, takeDamageToShieldInputSound, takeDamagePlayerInputSound;


    #endregion
    #region DataSaver
    [SerializeField] string saveDataPath = "PlayerData.sav";
    public Transform playerTransform;

    public float healthRemaining;
    public float energyRemaining;
    #endregion

    [SerializeField] protected UnityEvent OnDieEvent = new UnityEvent();
    public System.Action OnDie = delegate { };

    [SerializeField] protected UnityEvent OnDamageReceivedEvent = new UnityEvent();
    public event System.Action OnDamageReceived = delegate { };

    [SerializeField] protected UnityEvent OnDamageBlockedEvent = new UnityEvent();
    public event System.Action OnDamageBlocked = delegate { };

    public virtual void ApplyDamage(float damage2apply)
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= 1;

            OnDamageBlocked();
            OnDamageBlockedEvent.Invoke();
            if (forceFieldShield != null) forceFieldShield.SetActive(true);
        }
        else
        {
            if (forceFieldShield != null) forceFieldShield.SetActive(false);


            currentHealth -= damage2apply;

            if (currentHealth <= 0)
                Die();
            else
            {
                OnDamageReceived();
                OnDamageReceivedEvent.Invoke();
            }
        }

        energyRefillCount = countdownToRefillEnergy;

        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
        currentEnergy = Mathf.Clamp(currentEnergy, 0, _maxEnergy);
    }
    void ActiveShield()
    {
        if (currentEnergy > maxEnergy - 1)
        {
            if(currentEnergy >= maxEnergy)
            {
                if (!shieldRecharged)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(rechargedShieldInputSound);
                    shieldRecharged = true;
                    if(forceFieldShield != null)
                    {
                        forceFieldShield.SetActive(true);
                        forceFieldShield.transform.position = transform.position;
                    }
                }
            }
        }
        else
        {
            if(currentEnergy <= 0)
            {
                if (shieldRecharged)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(brokenShieldInputSound);
                    shieldRecharged = false;
                    if (forceFieldShield != null) forceFieldShield.SetActive(false);
                }
            }
        }
    }
    public virtual void Die()
    {
        if (!isDeath)
        {
            isDeath = true;
            //Die
            OnDieEvent.Invoke();
            OnDie();
            foreach (Component component in removeComponentsOnDead)
            {
                Destroy(component);
            }
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
                healthBar.transform.localPosition = Vector3.Lerp(Vector3.left * 2400, Vector3.zero, currentHealthPercent);
                energyBar.transform.localPosition = Vector3.Lerp(Vector3.left * 1400, Vector3.zero, currentEnergyPercent);
                break;

            case FillMethod.Filled:
                healthBar.fillAmount = currentHealthPercent;
                energyBar.fillAmount = currentEnergyPercent;
                break;
        }
    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();

        //currentHealth = maxHealth;
        //healthRemaining = currentHealth;
        //currentHealthPercent = healthPercent;

        //currentEnergy = maxEnergy;
        //energyRemaining = currentEnergy;
        //currentEnergyPercent = energyPercent;
    }

    protected virtual void Update()
    {
        if (forceFieldShield != null) ActiveShield();
        if (energyRefillCount > 0)
            energyRefillCount -= Time.deltaTime;
        else if (currentEnergy < maxEnergy)
        {
            if(currentEnergy < maxEnergy - 1)
            {
                rechargeFxFrecuency += Time.deltaTime;
                if (rechargeFxFrecuency > 0.75f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(rechargingShieldInputSound);
                    rechargeFxFrecuency = 0;
                }
            }
            currentEnergy = Mathf.MoveTowards(currentEnergy, maxEnergy, energyRefillSpeed * Time.deltaTime);
        }

        currentHealthPercent = Mathf.MoveTowards(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        currentEnergyPercent = Mathf.MoveTowards(currentEnergyPercent, energyPercent, smoothness * Time.deltaTime);
        //currentHealthPercent = Mathf.Lerp(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        //currentEnergyPercent = Mathf.Lerp(currentEnergyPercent, energyPercent, smoothness * Time.deltaTime * 2);
        UpdateUI();
    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
        healthRemaining = currentHealth;
        currentHealthPercent = healthPercent;

        currentEnergy = maxEnergy;
        energyRemaining = currentEnergy;
        currentEnergyPercent = energyPercent;

        //LoadData();
    }
    private void OnDisable()
    {
        //SaveData();
    }
    public void ImpactShotFx(bool hasShield)
    {
        if (hasShield) FMODUnity.RuntimeManager.PlayOneShot(takeDamageToShieldInputSound);
        else if(GetComponent<Player>()) FMODUnity.RuntimeManager.PlayOneShot(takeDamageToShieldInputSound);
        else FMODUnity.RuntimeManager.PlayOneShot(takeDamageInputSound);
    }

    [ContextMenu("S")]
    void SaveData()
    {
        if (GetComponent<Player>())
        {
            healthRemaining = currentHealth;
            energyRemaining = currentEnergy;

            PlayerData playerData = new PlayerData(this);
            BinarySerializer.SerializingPlayerData<PlayerData>(saveDataPath, playerData);
        }
    }
    [ContextMenu("L")]
    void LoadData()
    {
        if (MainUI_Manager.newPlay & !GetComponent<Player>()) return;

        PlayerData p = BinarySerializer.Deserialize<PlayerData>(saveDataPath);
        playerTransform.position = new Vector3(p.playerPos[0], p.playerPos[1], p.playerPos[2]);
        playerTransform.rotation = new Quaternion(p.playerRot[0], p.playerRot[1], p.playerRot[2], 0);
        healthRemaining = p.healthRemaining;
        energyRemaining = p.energyRemaining;

        currentHealth = healthRemaining;
        currentEnergy = energyRemaining;
    }
    [ContextMenu("C")]
    void ClearData()
    {
        BinarySerializer.ClearData(saveDataPath);
    }
}
