using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PauseMenuProperties : MonoBehaviour
{
    [SerializeField] Slider masterMusicSlider, fxSoundSlider;
    [SerializeField] TextMeshProUGUI masterVolumeValue, fxVolumeValue;

    private void Start()
    {
        SetMasterVolume(masterVolumeValue);
        SetFXVolume(fxVolumeValue);
    }
    public void SetMasterVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.mainMusicBus);
        bus.setVolume(masterMusicSlider.value / masterMusicSlider.maxValue);
        sliderText.text = masterMusicSlider.value.ToString();
    }
    public void SetFXVolume(TextMeshProUGUI sliderText)
    {
        FMOD.Studio.Bus bus = SoundsManager.GetMusicProperties(SoundsManager.fxSoundsBus);
        bus.setVolume(fxSoundSlider.value / fxSoundSlider.maxValue);
        sliderText.text = fxSoundSlider.value.ToString();
    }

}
