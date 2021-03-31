//K.Homa 02.03.2021
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.TextControl
{
    public class TextAnimationMovement : TextAnimation
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;


        public override void StartAnimation(TextElement textElement)
        {
            textElement.transform.position = _startPosition;
            iTween.MoveTo(textElement.gameObject, iTween.Hash(
                "position", _endPosition,
                "speed", _animationSpeed,
                "easetype", iTween.EaseType.linear,
                "looptype", loopType,
                "onstart", "SetAnimationIsPlayingToTrue",
                "onstartparams", this,
                "oncomplete", "SetAnimationIsPlayingToFalse",
                "oncompleteparams", this
                ));
        }

        public void SetStartPosition(Vector3 startPosition)
        {
            _startPosition = startPosition;
        }

        public void SetEndPosition(Vector3 endPosition)
        {
            _endPosition = endPosition;
        }
    }
}


