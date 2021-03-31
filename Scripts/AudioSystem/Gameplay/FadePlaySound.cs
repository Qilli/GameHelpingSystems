//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.CommonCode;

namespace Base.Audio
{
    public class FadePlaySound : BasePlaySound
    {
        private Coroutine c;
        public override void playSound()
        {
            StopAllCoroutines();
            base.playSound();
            StartCoroutine(Base.CommonCode.Common.FadeAudioSource.StartFade(Source, 0.1f, 1.0f));
        }

        public override void stopSound()
        {
            c = StartCoroutine(Base.CommonCode.Common.FadeAudioSource.StartFade(Source, 0.3f, 0.0f));
        }
    }
}
