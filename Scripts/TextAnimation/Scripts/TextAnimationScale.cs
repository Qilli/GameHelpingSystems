//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.TextControl
{
    public class TextAnimationScale : TextAnimation
    {
        Vector3 _startScale = Vector3.one;
        Vector3 _endScale;
        public TextAnimationScale(float speed,Vector3 startScaleValue,
            Vector3 endScaleValue)
        {
            SetAnimationSpeed(speed);
            _startScale = startScaleValue;
            _endScale = endScaleValue;
        }

        public override void StartAnimation(TextElement textElement)
        {
            textElement.transform.localScale = _startScale;
            iTween.ScaleTo(textElement.gameObject, iTween.Hash(
                "scale", _endScale,
                "speed", _animationSpeed,
                "easetype", iTween.EaseType.easeInOutQuad,
                "looptype", loopType,
                "onstart", "SetAnimationIsPlayingToTrue",
                "onstartparams", this,
                "oncomplete", "SetAnimationIsPlayingToFalse",
                "oncompleteparams", this
                ));
        }

        public void SetStartScale(Vector3 startScale)
        {
            _startScale = startScale;
        }

        public void SetEndScale(Vector3 endScale)
        {
            _endScale = endScale;
        }
    }
}


