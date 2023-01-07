using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerHUB
{
    float hp, maxHp;
    public PlayerHUB() { }
    public void SettingValuesToStaticHUD(float _hp, float _maxHp, Image hpIm, TextMeshProUGUI hpText)
    {
        hp = _hp;
        maxHp = _maxHp;
        HPBar(hpIm);
        SettingHp(hpText);
    }
    public void SettingValuesToScreenHUD(float _hp, float _maxHp, Image hpIm, TextMeshProUGUI hpText)
    {
        hp = _hp;
        maxHp = _maxHp;
        HPBar(hpIm);
        SettingHp(hpText);
    }
    public void ScreenHUDState(GameObject screenHUD, bool state)
    {
        screenHUD.SetActive(state);
    }
    void SettingHp(TextMeshProUGUI hpText/*, TextMeshProUGUI mpText, TextMeshProUGUI xpText*/)
    {
        hpText.text = maxHp.ToString() + "/" + hp.ToString();
        //mpText.text = maxMp.ToString() + "/" + mp.ToString(); Si PONEMOS ENERGIA PARA USAR HABILIDADES
        //xpText.text = ((xp / maxXp) * 100).ToString() + "% / " + ((maxXp / maxXp) * 100).ToString() + "% Exp"; SI PONEMOS EXPERIENCIA PARA OBTENER MAS NIVEL
    }
    void HPBar(Image hpIm)
    {
        hpIm.fillAmount = hp / maxHp;

    }
    //void MPBar(Image mpIm)
    //{
    //    mpIm.fillAmount = (float)mp / maxMp;
    //}
    //void XPBar(Image xpIm)
    //{
    //    xpIm.fillAmount = (float)xp / maxXp;
    //}
    public void CreatingDamageTexts(ObjectPool textPool)
    {
        textPool.RequestObject();
    }
}
