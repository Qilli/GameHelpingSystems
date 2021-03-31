//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Cameras
{
    public class CameraMoveTo : MonoBehaviour
    {
        protected bool isMoving_;
        protected Transform toMove;
        public virtual void startMoveTo(Transform t, Vector3 start, Vector3 end, Camera targetCam, float targetOrthoSize)
        {

        }

        public void updateMoveTo(float delta)
        {

        }

        public virtual bool isMoving()
        {
            return isMoving_;
        }

    }
}
