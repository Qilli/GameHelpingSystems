//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Base.Audio
{

    [CreateAssetMenu(menuName = "EngineAssets/System/Audio/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        public AudioMixer mixer;
        public float globalSoundVolume = 1.0f;
        public float musicSoundVolume = 1.0f;
        public float sfxSoundVolume = 1.0f;


        [Header("Params names")]
        public string mainVolume;
        public string musicVolume;
        public string sfxVolume;


        [Header("Default Fade options")]
        public float fadeVolumeDecrease = 1f;
        public float fadeMusicDecrease = 1f;
        [Header("Pitch options")]
        public bool randomizedPitch;
        public float pitchLowerOffset = 0.05f;
        public float pitchUpperOffset = 0.05f;

        public virtual void setMainVolume(float v_)
        {
            globalSoundVolume = v_;
            setSettings();
        }

        public virtual void setMusicVolume(float v_)
        {
            musicSoundVolume = v_;
            setSettings();
        }

        public virtual void setSFXVolume(float v_)
        {
            sfxSoundVolume = v_;
            setSettings();
        }

        public virtual void setSettings()
        {
            mixer.SetFloat(mainVolume, globalSoundVolume);
            mixer.SetFloat(musicVolume, musicSoundVolume);
            mixer.SetFloat(sfxVolume, sfxSoundVolume);
        }

        public float getValueFromSlider(float value)
        {
            float calculatedValue = Mathf.Log10(value) * 20; ;
            return calculatedValue;
        }

    }
}
