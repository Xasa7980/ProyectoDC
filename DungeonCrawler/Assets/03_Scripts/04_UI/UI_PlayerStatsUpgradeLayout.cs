using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatsUpgradeLayout : MonoBehaviour
{
    public Upgrade upgrade { get; private set; }

    [SerializeField] Image image;

    public UI_PlayerStatsUpgradeLayout CreateInstance(Upgrade upgrade, Transform parent = null)
    {
        UI_PlayerStatsUpgradeLayout instance = Instantiate(this, parent);

        instance.upgrade = upgrade;
        instance.image.sprite = upgrade.mainImage;

        return instance;
    }
}
