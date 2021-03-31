//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Cameras
{
    [CreateAssetMenu(menuName = "EngineAssets/Cameras/Data/CameraSettings2D")]
    public class Camera2DSettings : ScriptableObject
    {
        [Header("General")]
        public bool staticCam = false;
        public float orthoSize = 4.2f;
        public Vector3 cameraInitPosition;
        public Rect gameMapRect;
        [Header("Zoom")]
        public bool zoomOn = true;
        public float zoomInSpeed = 1.0f;
        public float zoomOutSpeed = 1.0f;
        public float maxZoomOffset = 2.0f;
        public float minZoomOffset = 2.0f;
        public float targetCamSizeOnZoom = 4;
        [Header("Pan")]
        public bool panOn = true;
        public float horizontalPanSpeed = 1.0f;
        public float verticalPanSpeed = 1.0f;
        public float mouseDragSpeed = 0.3f;
        public float panSpeed = 0.3f;

    }
}
