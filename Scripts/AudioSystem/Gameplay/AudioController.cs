//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Base.ObjectsControl;

namespace Base.Audio
{
    public class AudioController : BaseObject
    {
        [Header("Base")]
        public AudioSettings settings;
        public float timePauseStarted;

        //public List<ObjectAudioManager>

        public override void init()
        {
            base.init();
            settings.setSettings();
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}