using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UpgradeLayout : MonoBehaviour
{
    public Upgrade upgrade { get; private set; }

    [SerializeField] Button _cardButton;
    [SerializeField] Image _mainImage;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;

    public Button cardButton => _cardButton;
    public Image mainImage => _mainImage;

    public UI_UpgradeLayout CreateInstance(Upgrade upgrade, Transform parent = null)
    {
        UI_UpgradeLayout instance = Instantiate(this, parent);

        instance.upgrade = upgrade;
        instance.title.text = upgrade.displayName;
        instance.description.text = upgrade.description;

        instance._mainImage.sprite = upgrade.mainImage;

        return instance;
    }
}
