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
    [CreateAssetMenu(menuName = "EngineAssets/System/Audio/RandomFromGroup")]
    public class RandomFromGroup : BaseAudioPlayer
    {
        public AudioClip[] clips;

        public override void play(AudioSource source)
        {
            source.clip = getRandom();
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
            else source.Play();
        }

        public override void unPause(AudioSource source)
        {
            source.UnPause();
        }

        private AudioClip getRandom()
        {
            int index = Random.Range(0, clips.Length);
            return clips[index];
        }

        public override AudioClip getDefaultClip()
        {
            return clips[0];
        }
    }
}
