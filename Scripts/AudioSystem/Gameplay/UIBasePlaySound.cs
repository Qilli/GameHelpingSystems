using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Audio
{
    public class UIBasePlaySound : BasePlaySound
    {
        public override void init()
        {
            base.init();
            //if object has button set audio to play on click
            UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(this.playSound);
            }
        }
    }
}
