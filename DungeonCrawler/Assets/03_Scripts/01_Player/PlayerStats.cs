using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    #region Logical
    [SerializeField] float currentHealth, maxHealth, healPower;
    public float CurrentHealth { get { return currentHealth; } }
    public float MaxHealth { get { return currentHealth; } set { currentHealth = value; } }
    public float SetHealPower { set { healPower = value; } }
    bool Die() => currentHealth <= 0;

    string playerName;
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    float DmgReductionBonus;
    float castTime;
    #endregion
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        OnDie();
        RecoverHealthOverTime(healPower);
    }
    public void ApplyDamage(float damage2apply)
    {
        currentHealth -= damage2apply;
    }
    public void RecoverHealth(float healthToRecover)
    {
        currentHealth += healthToRecover;
    }
    void RecoverHealthOverTime(float healing)
    {
        castTime += Time.deltaTime;
        if (castTime >= 2)
        {
            currentHealth += healing;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            castTime = 0;
        }
    }
    void OnDie()
    {
        if (Die() & deathPanel)
        {
            gameObject.SetActive(false);
            deathPanel.SetActive(true);
        }
    }
}
