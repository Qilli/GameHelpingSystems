using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Physics.Verlet
{
    [System.Serializable]
    public class VerletPoint
    {
        #region PUBLIC PARAMS
        public Vector3 Position { get => position; set => position = value; } 
        public Vector3 PrevPosition { get => prevPosition; set => prevPosition = value; }
        public bool IsLocked { get => isLocked; set => isLocked = value; }
        #endregion
        #region PRIVATE PARAMS
        [SerializeField]
        private Vector3 position=Vector3.zero;
        [SerializeField]
        private Vector3 prevPosition=Vector3.zero;
        [SerializeField]
        private bool isLocked=false;

        internal void movePointBy(float vx, float vy, float vz)
        {
            position.x += vx;
            position.y += vy;
            position.z += vz;
        }
        #endregion

        #region PUBLIC FUNC
        #endregion
        #region PRIVATE FUNC
        #endregion

    }
}
