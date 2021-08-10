//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.UI
{
    public class InfoPanelsControler : MonoBehaviour
    {
        public SimpleInfoPanel[] panels;
        public Vector2 panelsStartOffset = new Vector2(5, -5);

        private void Awake()
        {
            foreach (SimpleInfoPanel p in panels)
            {
                p.TogglePanel(false);
                p.deactivateAll();
            }
        }

        public void hideAll()
        {
            foreach (SimpleInfoPanel p in panels)
            {
                p.TogglePanel(false);
                p.deactivateAll();
            }
        }

        public void setScreenPosition(Vector2 pos)
        {
            Vector2 offset = Camera.main.ViewportToScreenPoint(panelsStartOffset);
            transform.position = pos + offset;
        }

        public SimpleInfoPanel getInfoPanel(int index)
        {
            return panels[index];
        }
    }
}
