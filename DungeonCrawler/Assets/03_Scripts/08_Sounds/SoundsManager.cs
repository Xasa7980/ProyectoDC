using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public const string mainMusicBus = "bus:/AmbientMusic";
    public const string fxSoundsBus = "bus:/GameplayFX";

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
    public static FMOD.Studio.Bus GetMusicProperties(string busPath)
    {
        return FMODUnity.RuntimeManager.GetBus(busPath);
    }

    public static FMOD.Studio.Bus SetMusicProperties(string busPath)
    {
        return FMODUnity.RuntimeManager.GetBus(busPath);
    }
}
