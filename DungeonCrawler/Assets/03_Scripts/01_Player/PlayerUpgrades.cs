using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] UI_PlayerStatsUpgradeLayout upgradeLayout;
    [SerializeField] Transform container;
    List<Upgrade> allUpgrades = new List<Upgrade>();

    #region Upgrade Tracker
    [SerializeField] List<UpgradeTracker> automaticUpgrades = new List<UpgradeTracker>();

    bool ContainsUpgrade(Upgrade upgrade)
    {
        foreach(UpgradeTracker u in automaticUpgrades)
        {
            if(u.upgrade == upgrade)
                return true;
        }

        return false;
    }

    bool TryReplaceUpgrade(Upgrade upgrade)
    {
        if (automaticUpgrades.Count == 0) return false;

        for(int i = 0; i < automaticUpgrades.Count; i++)
        {
            if(automaticUpgrades[i].upgrade == upgrade)
            {
                automaticUpgrades[i].IncreaseProbability(upgrade.value);
                return true;
            }
        }

        return false;
    }
    #endregion

    private void Update()
    {
        foreach(UpgradeTracker u in automaticUpgrades)
        {
            u.Update();
        }
    }

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
                    if (!TryReplaceUpgrade(upgrade))
                    {
                        automaticUpgrades.Add(new UpgradeTracker(upgrade, this.transform));
                    }
                    break;
                }
        }
    }

    class UpgradeTracker
    {
        public readonly Upgrade upgrade;
        readonly Transform target;
        float timer;
        public float prob { get; private set; }

        public UpgradeTracker (Upgrade upgrade, Transform target)
        {
            this.upgrade = upgrade;
            this.target = target;
            prob = upgrade.value;
            timer = upgrade.timeInterval;
        }

        public UpgradeTracker(Upgrade upgrade, Transform target, float prob)
        {
            this.upgrade = upgrade;
            this.target = target;
            this.prob = prob;
            timer = upgrade.timeInterval;
        }

        public void IncreaseProbability(float prob)=>this.prob += prob;

        public void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = upgrade.timeInterval;
                if (Random.value < prob)
                    upgrade.Use(target);
            }
        }
    }
}
