using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] UI_PlayerStatsUpgradeLayout upgradeLayout;
    [SerializeField] Transform container;
    List<Upgrade> upgrades = new List<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        upgradeLayout.CreateInstance(upgrade, container);
    }

    public void AddUpgradeDelayed(Upgrade upgrade, float delay)
    {
        StartCoroutine(AddDelayed(upgrade, delay));
    }

    IEnumerator AddDelayed(Upgrade upgrade, float delay)
    {
        yield return new WaitForSeconds(delay);

        upgrades.Add(upgrade);
        upgradeLayout.CreateInstance(upgrade, container);
    }
}
