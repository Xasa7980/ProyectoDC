using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] UI_PlayerStatsUpgradeLayout upgradeLayout;
    [SerializeField] Transform container;
    List<Upgrade> allUpgrades = new List<Upgrade>();

    List<Upgrade> automaticUpgrades = new List<Upgrade>();

    void AddUpgrade(Upgrade upgrade)
    {
        allUpgrades.Add(upgrade);
        upgradeLayout.CreateInstance(upgrade, container);
        ProcessUpgrade(upgrade);
    }

    public void AddUpgradeDelayed(Upgrade upgrade, float delay)
    {
        StartCoroutine(AddDelayed(upgrade, delay));
    }

    IEnumerator AddDelayed(Upgrade upgrade, float delay)
    {
        yield return new WaitForSeconds(delay);

        AddUpgrade(upgrade);
    }

    void ProcessUpgrade(Upgrade upgrade)
    {
        Health health = GetComponent<Health>();

        switch (upgrade.target)
        {
            case Upgrade.Target.PlayerHealth:
                {
                    switch (upgrade.mode)
                    {
                        case Upgrade.Mode.Percent:
                            health.SetMaxHealth(health.maxHealth + (health.maxHealth * upgrade.value));
                            health.SetHealth(health.currentHealth + (health.currentHealth * upgrade.value));
                            return;

                        case Upgrade.Mode.Absolute:
                            health.SetMaxHealth(health.maxHealth + upgrade.value);
                            health.SetHealth(health.currentHealth + upgrade.value);
                            return;
                    }
                    break;
                }

            case Upgrade.Target.PlayerEnergyRecover:
                {
                    switch (upgrade.mode)
                    {
                        case Upgrade.Mode.Percent:
                            health.SetEnergyRefillSpeed(health.maxEnergy * upgrade.value);
                            return;

                        case Upgrade.Mode.Absolute:
                            health.SetMaxEnergy(upgrade.value);
                            return;
                    }
                    break;
                }

            case Upgrade.Target.PlayerEnergy:
                {
                    switch (upgrade.mode)
                    {
                        case Upgrade.Mode.Percent:
                            health.SetMaxEnergy(health.maxEnergy + (health.maxEnergy * upgrade.value));
                            return;

                        case Upgrade.Mode.Absolute:
                            health.SetMaxEnergy(health.maxEnergy + upgrade.value);
                            return;
                    }
                    break;
                }

            case Upgrade.Target.Skill:
                {
                    switch (upgrade.mode)
                    {
                        case Upgrade.Mode.Percent:

                            break;

                        case Upgrade.Mode.Absolute:

                            break;
                    }
                    break;
                }

            case Upgrade.Target.Weapon:
                {
                    GetComponentInChildren<RifleController>().AddUpgrade(upgrade);
                    break;
                }

            case Upgrade.Target.Automatic:
                {
                    automaticUpgrades.Add(upgrade);
                    break;
                }
        }
    }
}
