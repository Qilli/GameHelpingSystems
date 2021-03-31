//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Audio
{
    public class RandomFromGroupPlaySound : BasePlaySound
    {
        private bool isPlayingGroup = false;
        private Coroutine groupPlay;
        public override void playSound()
        {
            base.playSound();
            if (isPlayingGroup == false)
            {
                isPlayingGroup = true;
                //start coroutine
                groupPlay = StartCoroutine("updateGroupPlay");
            }

        }

        public override void stopSound()
        {
            //base.stopSound();
            isPlayingGroup = false;
            if (groupPlay != null) StopCoroutine(groupPlay);
            groupPlay = null;
        }

        private IEnumerator updateGroupPlay()
        {
            while (isPlayingGroup)
            {
                yield return null;
                if (IsPaused) yield return null;
                if (!isPlaying() && isPlayingGroup)
                {
                    yield return new WaitForSeconds(audioPlayer.settings.offsetBetweenNextGroupPlay);
                    playSound();
                }
            }

        }

    }
}
