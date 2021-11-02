using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Animation
{
    public class OnAnimationEventPlayAudio : MonoBehaviour, IOnAnimationEvent
    {
        [Header("target name type")]
        public string targetEventName;
        [Header("Used Sound")]
        public Audio.BasePlaySound soundToPlay;
        public void onAnimationEvent(string eventAnimName)
        {
            if(targetEventName==eventAnimName)
            {
                soundToPlay?.playSound();
            }
        }
    }
}
