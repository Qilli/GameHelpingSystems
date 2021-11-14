using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Physics
{
    public class KinematicAgentData
    {
        #region PUBLIC PARAMS
        #endregion
        #region PRIVATE PARAMS
        private Vector3 position;
        private Vector3 velocity;
        private float orientationHorizontal;
        private float rotationHorizontal;
        #endregion

        #region PUBLIC FUNC
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }
        public float OrientationHorizontal
        {
            get => orientationHorizontal;
            set => orientationHorizontal = value;
        }
        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
        public float RotationHorizontal
        {
            get => rotationHorizontal;
            set => rotationHorizontal = value;
        }
        public void set(Vector3 position_, Vector3 velocity_, float orientationHorizontal_, float rotationHorizontal_)
        {
            position = position_;
            velocity = velocity_;
            orientationHorizontal = orientationHorizontal_;
            rotationHorizontal = rotationHorizontal_;
        }

        public static float DirectionToOrientation(Vector3 direction)
        {
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }

        public static Vector3 OrientationToDirection(float orientation)
        {
            return new Vector3(Mathf.Sin(orientation * Mathf.Deg2Rad), 0, Mathf.Cos(orientation * Mathf.Deg2Rad));
        }
        #endregion
        #region PRIVATE FUNC
        #endregion

    }
}
