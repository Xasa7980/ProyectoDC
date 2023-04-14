using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public const string mainMusicBus = "bus:/AmbientMusic";
    public const string fxSoundsBus = "bus:/GameplayFX";
    public const string fxUISoundsBus = "bus:/UIFX";


    FMOD.Studio.EventInstance music;
    [SerializeField] FMODUnity.EventReference generalMusic;
    [SerializeField] FMODUnity.EventReference ambienceMusic;
    private void Start()
    {
        SetMusic(ambienceMusic);
        SetMusic(generalMusic);
    }
    void SetMusic(FMODUnity.EventReference _generalMusic)
    {
        music = FMODUnity.RuntimeManager.CreateInstance(_generalMusic);

        FMOD.Studio.PLAYBACK_STATE playback;
        music.getPlaybackState(out playback);
        if (playback.Equals(FMOD.Studio.PLAYBACK_STATE.STOPPED))
        {
            music.start();
        }
    }
    void StopMusic(FMODUnity.EventReference _generalMusic)
    {
        music = FMODUnity.RuntimeManager.CreateInstance(_generalMusic);

        FMOD.Studio.PLAYBACK_STATE playback;
        music.getPlaybackState(out playback);
        if (playback.Equals(FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public static FMOD.Studio.Bus GetBusInstance(string busPath)
    {
        return FMODUnity.RuntimeManager.GetBus(busPath);
    }
    private void OnDisable()
    {
        StopMusic(ambienceMusic);
        StopMusic(generalMusic);
    }
}
