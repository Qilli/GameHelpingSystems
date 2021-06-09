//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.TextControl
{
    public enum LoopType
    {
        Once,
        Loop,
        PingPong
    }
    public abstract class TextAnimation
    {
        protected float _animationSpeed = 0;
        protected bool _isPlaying = false;
        protected iTween.LoopType loopType = iTween.LoopType.none;
        public abstract void StartAnimation(TextElement textElement);

        public void StopAnimation(TextElement textElement)
        {
            _isPlaying = false;
            iTween.Stop(textElement.gameObject);
        }

        public void SetIsPlayingToTrue()
        {
            _isPlaying = true;
        }

        public void SetIsPlayingToFalse()
        {
            _isPlaying = false;
        }

        public float GetAnimationSpeed()
        {
            return _animationSpeed;
        }

        public iTween.LoopType GetLoopType()
        {
            return loopType;
        }

        public virtual void SetAnimationSpeed(float animationSpeed)
        {
            if (animationSpeed >= 0)
                _animationSpeed = animationSpeed;
        }

        public void SetLoopType(LoopType newLoopType)
        {
            switch (newLoopType)
            {
                case LoopType.Once:
                    loopType = iTween.LoopType.none;
                    break;
                case LoopType.Loop:
                    loopType = iTween.LoopType.loop;
                    break;
                case LoopType.PingPong:
                    loopType = iTween.LoopType.pingPong;
                    break;
            }
        }

        public bool IsOver()
        {
            return !_isPlaying;
        }
    }
}

    

