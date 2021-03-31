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
    public class BasePlaySound : BaseObject
    {
        #region PUBLIC PARAMS
        [Header("Parameters")]
        public BaseAudioPlayer audioPlayer;
        public bool playOnStart = false;
        #endregion
        private AudioSource source;
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

    }
}
