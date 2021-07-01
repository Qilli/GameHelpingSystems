//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Base.UI
{

    public class PanelWindow : MonoBehaviour
    {
        [Header("UI Data")]
        public TextMeshProUGUI windowTitle;
        [Header("UIControllers Resize Panel")]
        public ResizePanel resizePanel;
        public Vector2 minScreenSize;
        public Vector2 maxScreenSize;

        protected virtual void Awake()
        {
            setMinMaxWindowSize(minScreenSize, maxScreenSize);
        }

        public virtual void TogglePanel()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            if (gameObject.activeSelf == true)
            {
                onOpen();
            }
            else
            {
                onClose();
            }
        }

        public virtual void TogglePanel(bool show)
        {
            gameObject.SetActive(show);
            if (gameObject.activeSelf == true)
            {
                onOpen();
            }
            else
            {
                onClose();
            }
        }

        public virtual void onOpen()
        {

        }

        public virtual void onClose()
        {

        }

        public virtual void setWindowTitle(string title)
        {
            windowTitle.SetText(title);
        }

        public virtual void setMinMaxWindowSize(Vector2 min, Vector2 max)
        {
            resizePanel.minSize = min;
            resizePanel.maxSize = max;
        }

    }

}
