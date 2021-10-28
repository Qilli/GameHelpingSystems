using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Audio
{
    public interface IPlaySound 
    {
        public void playSound();
        public void stopSound();
        public void pauseSound();
        public void unPauseSound();
        public bool isPlaying();
    }
}
