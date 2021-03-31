//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
Ustawienia dźwięku dla konkretnego źródła dźwięku
 */

namespace Base.Audio
{
    [CreateAssetMenu(menuName = "EngineAssets/System/Audio/SoundObjectSettings")]
    public class SoundObjectSettings : ScriptableObject
    {
        public AudioMixerGroup mixer;

        public enum AudioType
        {
            Audio_2D,
            Audio_3D
        }

        public AudioType type;

        [Range(0.0f, 1.0f)]
        public float volume = 1f;
        [Range(0.0f, 1.0f)]
        public float pitch = 1f;
        [Range(-1.0f, 1.0f)]
        public float stereoPan = 0f;
        [Range(0.0f, 1.0f)]
        public float spartialBlend = 1f;
        [Range(0.0f, 5.0f)]
        public float dopplerEffect = 1f;
        [Range(0.0f, 360f)]
        public float spread = 1f;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
        public float minDistance = 10;
        public float maxDistance = 15;
        public bool loop = false;
        /// <summary>
        /// jeśli mamy grupowe odgrywanie to w momencie kiedy dany sample przestanie grać ile czekamy aż zacznie grać następny
        /// </summary>
        public float offsetBetweenNextGroupPlay = 0.2f;

        public bool randomPitch = false;
        [Range(0.0f, 1.0f)]
        public float pitchOffset = 0.1f;

        [Header("MusicONLYOption")]
        public bool fadeOut = false;

        public void setSettings(AudioSource source)
        {
            if (type == AudioType.Audio_2D)
            {
                source.spatialBlend = 0f; //wylaczamy dzwiek 3d
            }
            else if (type == AudioType.Audio_3D)
            { //opcje 3d
                source.spatialBlend = spartialBlend;
                source.dopplerLevel = dopplerEffect;
                source.spread = spread;
                source.rolloffMode = rolloffMode;
                source.minDistance = minDistance;
                source.maxDistance = maxDistance;
            }

            source.outputAudioMixerGroup = mixer;
            source.loop = loop;
            source.volume = volume;
            source.panStereo = stereoPan;
            source.pitch = calculatePitch();
        }

        float calculatePitch()
        {
            float pitchCalc = pitch;
            if (randomPitch)
            {
                pitchCalc = Random.Range(pitch - pitchOffset, pitch + pitchOffset);
            }
            return pitchCalc;

        }
    }
}
