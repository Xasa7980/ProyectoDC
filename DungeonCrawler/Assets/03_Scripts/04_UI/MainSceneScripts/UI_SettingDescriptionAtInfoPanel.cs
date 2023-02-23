using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_SettingDescriptionAtInfoPanel : MonoBehaviour
{
    [SerializeField] string titleText;
    [SerializeField, TextArea] string descriptionText;
    [SerializeField] Sprite imageForBackground;
    [SerializeField] Image backgroundImage;
    [SerializeField]TextMeshProUGUI title,changableText;
    void InitializingTexts()
    {
        title.text = titleText;
        changableText.text = descriptionText;
    }
    void SettingBackgroundImage()
    {
        if (imageForBackground == null)
        {
            backgroundImage.color = new Color(1, 1, 1, 0);
            return;
        }
        backgroundImage.color = new Color(1, 1, 1, 0.35f);
        backgroundImage.sprite = imageForBackground;
    }
    public void SettingText()
    {
        InitializingTexts();
        SettingBackgroundImage();
    }
}
