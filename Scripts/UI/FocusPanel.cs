//K.Homa 27.02.2021
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Base.UI
{
    public class FocusPanel : MonoBehaviour, IPointerDownHandler
    {
        private RectTransform panel;

        void Awake()
        {
            panel = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            panel.SetAsLastSibling();
        }

    }
}