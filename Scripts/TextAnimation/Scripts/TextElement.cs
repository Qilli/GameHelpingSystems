//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Base.QTE;

namespace Base.TextControl
{
    public class TextElement : MonoBehaviour
    {
        private TMP_Text _textMeshPro;
        private TextType _typeOfText;
        private float _timeToDestroy;

        List<TextAnimation> _onShowAnimations = new List<TextAnimation>();
        List<TextAnimation> _onIdleAnimations = new List<TextAnimation>();
        List<TextAnimation> _onHideAnimations = new List<TextAnimation>();

        public Func<TextElement, bool> onAutoDestroy;
        public TMP_Text Text => _textMeshPro;

        public TextType TypeOfText { get => _typeOfText; }

        public TMP_Text getNativeText()
        {
            return Text;
        }

        private void OnEnable()
        {
            _textMeshPro = gameObject.GetComponent<TextMeshPro>();
            if (_textMeshPro != null)
            {
                _typeOfText = TextType.Text3D;
                return;
            }

            _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            _typeOfText = TextType.Text2D;
        }

        public void SetText(string text)
        {
            _textMeshPro.SetText(text);
        }

        public void SetPosition(Vector3 position)
        {
            _textMeshPro.transform.position = position;
        }

        public void SetPosition(Vector2 position)
        {
            _textMeshPro.transform.position = position;
        }

        public void SetFont(TMP_FontAsset font)
        {
            _textMeshPro.font = font;
        }

        public void SetFontSize(float fontSize)
        {
            _textMeshPro.fontSize = fontSize;
        }

        public void SetColor(Color color)
        {
            _textMeshPro.color = color;
        }

        public void SetAutoDestroyTime(float animationDuration)
        {
            _timeToDestroy = animationDuration;
            if (animationDuration > 0)
            {
                QuickTimeEvent e = new QuickTimeEvent(1, animationDuration, onEvent);
                //GlobalDataContainer.It.qteController.addAndStartQTEvent(e); TODO musi dodac ten event zeby to w ogole dzialalo
            }

        }

        void onEvent()
        {
            onAutoDestroy?.Invoke(this);
        }

        public void OnShowAnimation()
        {
            foreach (TextAnimation textAnimation in _onShowAnimations)
            {
                textAnimation.StartAnimation(this);
            }
        }

        public void OnIdleAnimation()
        {
            foreach (TextAnimation textAnimation in _onIdleAnimations)
            {
                textAnimation.StartAnimation(this);
            }
        }

        public void OnHideAnimation()
        {
            foreach (TextAnimation textAnimation in _onHideAnimations)
            {
                textAnimation.StartAnimation(this);
            }
        }

        public void AddOnShowAnimation(TextAnimation textAnimation)
        {
            _onShowAnimations.Add(textAnimation);
        }

        public void AddOnIdleAnimation(TextAnimation textAnimation)
        {
            _onIdleAnimations.Add(textAnimation);
        }

        public void AddOnHideAnimation(TextAnimation textAnimation)
        {
            _onHideAnimations.Add(textAnimation);
        }

        public void RemoveOnShowAnimation(TextAnimation textAnimation)
        {
            _onShowAnimations.Remove(textAnimation);
        }

        public void RemoveOnIdleAnimation(TextAnimation textAnimation)
        {
            _onIdleAnimations.Remove(textAnimation);
        }

        public void RemoveOnHideAnimation(TextAnimation textAnimation)
        {
            _onHideAnimations.Remove(textAnimation);
        }

        public void SetAnimationIsPlayingToTrue(TextAnimation textAnimation)
        {
            textAnimation.SetIsPlayingToTrue();
        }

        public void SetAnimationIsPlayingToFalse(TextAnimation textAnimation)
        {
            textAnimation.SetIsPlayingToFalse();
        }

        public void StopAnimation()
        {
            iTween.Stop(gameObject);
        }
    }
}

