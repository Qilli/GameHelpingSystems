//K.Homa 02.03.2021
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Base.TextControl
{
    public class TextElementsManager : Base.ObjectsControl.BaseObject
    {
        private List<TextElement> _textElements = new List<TextElement>();

        public TextElement CreateTextElement(TextType textType, string text, Vector3 position, Color color,
                                            float fontSize = 12, float timeToDestroy = -1, GameObject childElement = null)
        {
            GameObject go = new GameObject("TextElement");
            if (childElement != null)
            {
                childElement.transform.SetParent(go.transform);
            }

            GameObject goParent = new GameObject("TextElementObject");
            go.transform.parent = goParent.transform;
            

            switch (textType)
            {
                case TextType.Text2D:
                    go.AddComponent<TextMeshProUGUI>();
                    break;
                case TextType.Text3D:
                    TextMeshPro t=go.AddComponent<TextMeshPro>();
                    t.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    t.verticalAlignment = VerticalAlignmentOptions.Middle;
                    t.enableAutoSizing = false;
                    t.rectTransform.sizeDelta = new Vector2() { x = 5.0f, y = 2.0f };
                    break;
            }

            TextElement textElement = go.AddComponent<TextElement>();
            textElement.SetText(text);
            textElement.SetPosition(Vector3.zero);
            textElement.SetColor(color);
            textElement.SetFontSize(fontSize);
            go.transform.localPosition = Vector3.zero;

            if (timeToDestroy > 0)
            {
                textElement.SetAutoDestroyTime(timeToDestroy);
                textElement.onAutoDestroy += RemoveTextElement;
            }
            _textElements.Add(textElement);
            return textElement;
        }

        public override void onUpdate(float delta)
        {
            base.onUpdate(delta);
            for(int a=0;a<_textElements.Count;++a)
            {
                _textElements[a].onUpdate(delta);
            }
        }

        public bool RemoveTextElement(TextElement textElement)
        {
            if (textElement != null)
            {
                _textElements.Remove(textElement);
                Destroy(textElement.transform.parent.gameObject);
                return true;
            }

            return false;

        }

        public void RemoveAllTextElements()
        {
            foreach (TextElement textElement in _textElements)
            {
                if (textElement != null)
                    Destroy(textElement.transform.parent.gameObject);
            }

            _textElements.Clear();
        }

        public void PauseAll()
        {
            foreach (TextElement textElement in _textElements)
            {
                if (textElement != null)
                    iTween.Pause(textElement.gameObject);
            }
        }

        public void UnpauseAll()
        {
            foreach (TextElement textElement in _textElements)
            {
                if (textElement != null)
                    iTween.Resume(textElement.gameObject);
            }
        }
    }
}
