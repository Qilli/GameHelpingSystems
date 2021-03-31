//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.TextControl
{
    public class TextAnimationRotation : TextAnimation
    {
        private float _xRotation = 0;
        private float _yRotation = 0;
        private float _zRotation = 0;



        public override void StartAnimation(TextElement textElement)
        {
            iTween.RotateBy(textElement.gameObject, iTween.Hash(
                "x", _xRotation,
                "y", _yRotation,
                "z", _zRotation,
                "speed", _animationSpeed,
                "easetype", iTween.EaseType.linear,
                "looptype", loopType,
                "onstart", "SetAnimationIsPlayingToTrue",
                "onstartparams", this,
                "oncomplete", "SetAnimationIsPlayingToFalse",
                "oncompleteparams", this
                ));
        }

        public void SetXRotation(float xRotation)
        {
            _xRotation = xRotation;
        }

        public void SetYRotation(float yRotation)
        {
            _yRotation = yRotation;
        }

        public void SetZRotation(float zRotation)
        {
            _zRotation = zRotation;
        }
    }
}


