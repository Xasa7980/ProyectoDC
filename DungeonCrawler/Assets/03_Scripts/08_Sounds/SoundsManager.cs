using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    FMOD.Studio.Bus busPath;

    FMOD.Studio.EventInstance music;
    [SerializeField] FMODUnity.EventReference generalMusic;
    [SerializeField] FMODUnity.EventReference ambienceMusic;
    private void Start()
    {
        SetMusic(ambienceMusic,music);
        SetMusic(generalMusic, music);
    }
    void SetMusic(FMODUnity.EventReference _generalMusic, FMOD.Studio.EventInstance instance)
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(_generalMusic);

        FMOD.Studio.PLAYBACK_STATE playback;
        instance.getPlaybackState(out playback);
        if (playback.Equals(FMOD.Studio.PLAYBACK_STATE.STOPPED))
        {
            instance.start();
        }
    }
    FMOD.Studio.Bus GetMusicProperties(string busPath)
    {
        return FMODUnity.RuntimeManager.GetBus(busPath);
    }

    FMOD.Studio.Bus SetMusicProperties(string busPath)
    {
        return FMODUnity.RuntimeManager.GetBus(busPath);
    }
    private void OnDisable()
    {
        
    }
}
