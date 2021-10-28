//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 Podstawowy player dla dźwięku, po prostu odgrywa daną ścieżkę.
*/

namespace Base.Audio
{
    [CreateAssetMenu(menuName = "EngineAssets/System/Audio/SingleAudioPlayer")]
    public class SingleAudioPlayer : BaseAudioPlayer
    {
        public AudioClip clip;

        public override void play(AudioSource source)
        {
            source.clip = clip;
            source.Play();
        }

        public override void stop(AudioSource source)
        {
            source.Stop();
        }

        public override void pause(AudioSource source)
        {
            if (source.isPlaying)
                source.Pause();
        }

        public override void unPause(AudioSource source)
        {
            source.UnPause();
        }

        public override AudioClip getDefaultClip()
        {
            return clip;
        }
    }
}
