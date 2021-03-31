//K.Homa 02.03.2021
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Base.TextControl
{
    public class TextElementsManager : MonoBehaviour
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

            switch (textType)
            {
                case TextType.Text2D:
                    go.AddComponent<TextMeshProUGUI>();
                    break;
                case TextType.Text3D:
                    go.AddComponent<TextMeshPro>();
                    break;
            }

            TextElement textElement = go.AddComponent<TextElement>();
            textElement.SetText(text);
            textElement.SetPosition(position);
            textElement.SetColor(color);
            textElement.SetFontSize(fontSize);
            if (timeToDestroy > 0)
            {
                textElement.SetAutoDestroyTime(timeToDestroy);
                textElement.onAutoDestroy += RemoveTextElement;
            }
            _textElements.Add(textElement);
            return textElement;
        }

        public bool RemoveTextElement(TextElement textElement)
        {
            if (textElement != null)
            {
                _textElements.Remove(textElement);
                Destroy(textElement.gameObject);
                return true;
            }

            return false;

        }

        public void RemoveAllTextElements()
        {
            foreach (TextElement textElement in _textElements)
            {
                if (textElement != null)
                    Destroy(textElement.gameObject);
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
