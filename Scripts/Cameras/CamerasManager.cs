﻿using UnityEngine;
using Base.ObjectsControl;
namespace Base.Cameras
{
    public class CamerasManager : BaseObject
    {
        public virtual Camera getMainNativeCamera()
        {
            return Camera.main;
        }

    }
}
