//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.ObjectsControl;

namespace Base.Cameras
{
    public class StaticCamera2D : BaseObject
    {
        public Camera cam;
        public float targetWidth;
        public float targetHeight;

        private void calculateOrthoSize()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float gameArenaAspect = targetWidth / targetHeight;
            if (screenAspect >= gameArenaAspect)
            {
                cam.orthographicSize = targetHeight / 2.0f;
            }
            else
            {
                float difference = gameArenaAspect / screenAspect;
                cam.orthographicSize = targetHeight * 0.5f * difference;
            }
        }

        public override void init()
        {
            base.init();
            calculateOrthoSize();
        }

    }
}
