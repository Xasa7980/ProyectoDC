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
        FMOD.Studio.Bus bus = SoundsManager.GetBusInstance(SoundsManager.mainMusicBus);
        bus.setVolume(musicVolumeSlider.value / musicVolumeSlider.maxValue);
        sliderText.text = musicVolumeSlider.value.ToString();
    }
    public void SetFXVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetBusInstance(SoundsManager.fxSoundsBus);
        bus.setVolume(fxVolumeSlider.value / fxVolumeSlider.maxValue);
        sliderText.text = fxVolumeSlider.value.ToString();
    }
    public void SetUIFXVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetBusInstance(SoundsManager.fxUISoundsBus);
        bus.setVolume(fxUIVolumeSlider.value / fxUIVolumeSlider.maxValue);
        sliderText.text = fxUIVolumeSlider.value.ToString();
    }
}
