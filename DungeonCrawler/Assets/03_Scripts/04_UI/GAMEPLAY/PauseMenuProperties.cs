using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PauseMenuProperties : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider, fxVolumeSlider, fxUIVolumeSlider;
    [SerializeField] TextMeshProUGUI masterVolumeValue, fxVolumeValue, fxUIVolumeValue;


    private void Start()
    {
        SetMasterVolume(masterVolumeValue);
        SetFXVolume(fxVolumeValue);
        SetUIFXVolume(fxUIVolumeValue);
    }
    public void SetMasterVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.mainMusicBus);
        PlayerPrefs.SetFloat("MasterVolume", musicVolumeSlider.value);
        bus.setVolume(PlayerPrefs.GetFloat("MasterVolume"));
        sliderText.text = PlayerPrefs.GetFloat("MasterVolume").ToString();
    }
    public void SetFXVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.fxSoundsBus);
        PlayerPrefs.SetFloat("fxVolume", fxVolumeSlider.value);
        bus.setVolume(PlayerPrefs.GetFloat("fxVolume"));
        sliderText.text = PlayerPrefs.GetFloat("fxUIVolume").ToString();
    }
    public void SetUIFXVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.fxUISoundsBus);
        PlayerPrefs.SetFloat("fxVolume", fxUIVolumeSlider.value);
        bus.setVolume(PlayerPrefs.GetFloat("fxUIVolume"));
        sliderText.text = PlayerPrefs.GetFloat("fxUIVolume").ToString();
    }
    public float GetMasterVolume()
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.mainMusicBus);
        bus.getVolume(out float volume);
        return volume;
    }
    public float GetFXVolume()
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.fxSoundsBus);
        bus.getVolume(out float volume);
        return volume;
    }
    public float GetUIFXVolume()
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.fxUISoundsBus);
        bus.getVolume(out float volume);
        return volume;
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVolume", GetMasterVolume());
        PlayerPrefs.SetFloat("fxVolume", GetFXVolume());
        PlayerPrefs.SetFloat("fxUIVolume", GetUIFXVolume());

    }
}
