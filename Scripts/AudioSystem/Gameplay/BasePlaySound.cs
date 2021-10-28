//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.ObjectsControl;
/*
Podstawowy player dźwięku dla gameplayu 
*/

namespace Base.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BasePlaySound : BaseObject,IPlaySound
    {
        #region PUBLIC PARAMS
        [Header("Parameters")]
        public BaseAudioPlayer audioPlayer;
        public bool playOnStart = false;
        #endregion
        #region PRIVATE 
        private AudioSource source;
        #endregion
        #region PUBLIC FUNC
        public bool IsPaused { get; private set; }
        public AudioSource Source { get { return source; } }

        public override void init()
        {
            if (inited == false)
            {
                base.init();
                source = GetComponent<AudioSource>();
                audioPlayer.settings.setSettings(source);
                if (playOnStart) playSound();
                inited = true;
            }
        }

        public virtual void playSound()
        {
            audioPlayer.play(source);
        }

        public virtual void stopSound()
        {
            audioPlayer.stop(source);
        }

        public virtual void pauseSound()
        {
            audioPlayer.pause(source);
            IsPaused = true;
        }

        public virtual void unPauseSound()
        {
            audioPlayer.unPause(source);
            IsPaused = false;
        }

        public virtual bool isPlaying()
        {
            return source.isPlaying;
        }
        #endregion
        #region HELPER
        void Start()
        {
            init();
        }
        void Reset()
        {
          //  source=gameObject.AddComponent<AudioSource>();
        //    source.outputAudioMixerGroup = Resources.FindObjectsOfTypeAll<UnityEngine.Audio.AudioMixerGroup>()[0];
        }
        void OnValidate()
        {
            if(audioPlayer!=null)
            {
                if (source == null)
                {
                    if ((source = GetComponent<AudioSource>()) == null)
                    {
                        source = gameObject.AddComponent<AudioSource>();
                    }
                }
                source.clip = audioPlayer.getDefaultClip();
                audioPlayer.settings.setSettings(source);
            }
        }
        #endregion
    }
}
