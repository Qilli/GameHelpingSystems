//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/*
Bazowy player 
*/

namespace Base.Audio
{
  public abstract class BaseAudioPlayer : ScriptableObject
    {
        public SoundObjectSettings settings;
        public abstract void play(AudioSource source);
        public abstract void stop(AudioSource source);
        public abstract void pause(AudioSource source);
        public abstract void unPause(AudioSource source);
    }
}
