//K.Homa 02.03.2021
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.TextControl
{
    public class TextAnimationColor : TextAnimation
    {
        private Color _endColor;

        public override void StartAnimation(TextElement textElement)
        {
            iTween.ColorTo(textElement.gameObject, iTween.Hash(
                "color", _endColor,
                "speed", _animationSpeed,
                "looptype", loopType,
                "onstart", "SetAnimationIsPlayingToTrue",
                "onstartparams", this,
                "oncomplete", "SetAnimationIsPlayingToFalse",
                "oncompleteparams", this
                ));
        }

        public void SetEndColor(Color endColor)
        {
            _endColor = endColor;
        }
    }
}


