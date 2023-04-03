using UnityEngine;

namespace Michsky.UI.Shift
{
    public class UIManagerBGMusic : MonoBehaviour
    {
        [Header("Resources")]
        public UIManager UIManagerAsset;
        [SerializeField] FMODUnity.EventReference bgMusic;
        void Awake()
        {
            UpdateSource();
        }

        void LateUpdate()
        {
            //if (UIManagerAsset != null)
            //{
            //    if (UIManagerAsset.enableDynamicUpdate == true)
            //        dynamicUpdateEnabled = true;
            //    else
            //        dynamicUpdateEnabled = false;

            //    if (dynamicUpdateEnabled == true)
            //        UpdateSource();
            //}
        }

        void UpdateSource()
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(bgMusic);
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            if (state.Equals(FMOD.Studio.PLAYBACK_STATE.STOPPED))
            {
                instance.start();
            }
        }
        private void OnDisable()
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(bgMusic);
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            if (state.Equals(FMOD.Studio.PLAYBACK_STATE.PLAYING))
            {
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }
}