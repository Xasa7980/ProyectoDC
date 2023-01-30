using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUBManager : MonoBehaviour
{
    // Las variables screen... será por si el jugador podra ver su barra de vida en el juego o tendra que mover la cabeza hasta donde se encuentra la interfaz de usuario
    [SerializeField] Image staticHpIm, screenHpIm;
    [SerializeField] TextMeshProUGUI screenPlayerName, staticPlayerName, staticHpText, screenHpText;
    [SerializeField] GameObject normalDmgText, critDmgText; //En caso de usar textos para mostrar el daño causado 

    PlayerHUB pHUB;
    Health pHealth;
    void Start()
    {
        pHUB = new PlayerHUB();
        pHealth = GetComponent<Health>();
    }

    void Update()
    {
        UpdatingPlayerStaticHUD();
    }
    void UpdatingPlayerStaticHUD()
    {
        pHUB.SettingValuesToStaticHUD(pHealth.currentHealth,pHealth.maxHealth,staticHpIm,staticHpText);
        staticPlayerName.text = pHealth.playerName;
    }
    //void UpdatingPlayerScreenHUD() Funciona igual que UpdatingPlayerStaticHUD(); pero para el screen
    //{
    //    pHUB.SettingValuesToScreenHUD(pStats.CurrentHealth, pStats.MaxHealth, screenHpIm, staticHpText);
    //    screenPlayerName.text = pStats.PlayerName;
    //}
}
