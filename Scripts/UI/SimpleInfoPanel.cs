//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Base.UI
{
    public class SimpleInfoPanel : Base.ObjectsControl.BaseObject
    {
        public TextMeshProUGUI[] texts;
        public virtual void TogglePanel(bool show)
        {
            gameObject.SetActive(show);
        }

        public void deactivateAll()
        {
            foreach (TextMeshProUGUI t in texts)
            {
                t.gameObject.SetActive(false);
            }
        }


        public void setText(int index, string text)
        {
            texts[index].gameObject.SetActive(true);
            texts[index].text = text;
        }

        public void setScreenPosition
            (Vector2 pos)
        {
            transform.position = pos;
        }

    }
}
