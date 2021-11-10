using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Physics.Verlet
{
    [System.Serializable]
    public class VerletStick
    {
        #region PUBLIC PARAMS
        [SerializeReference]
        public VerletPoint point0;
        [SerializeReference]
        public VerletPoint point1;
        public float stickLength;
        #endregion
        #region PRIVATE PARAMS

        #endregion

        #region PUBLIC FUNC
         public void setStick(VerletPoint p0,VerletPoint p1)
        {
            point0 = p0; point1 = p1; stickLength = Vector3.Distance(point0.Position, point1.Position);
        }

        internal void constrainStick()
        {
            float currentDistance = Vector3.Distance(point0.Position, point1.Position);
            Vector3 pos0 = point0.Position;
            Vector3 pos1 = point1.Position;
            Vector3 diff = pos1 - pos0;
            float distanceDifference = stickLength - currentDistance;
            float toAddPerc = (distanceDifference/currentDistance)*0.5f;
            if(!point0.IsLocked)point0.movePointBy(-diff.x*toAddPerc    ,- diff.y * toAddPerc, -diff.z * toAddPerc);
            if(!point1.IsLocked)point1.movePointBy(diff.x * toAddPerc, diff.y * toAddPerc, diff.z * toAddPerc);
        }

        internal bool hasPoint(VerletPoint verletPoint)
        {
            return verletPoint == point0 || verletPoint == point1;
        }

        internal void calculateLength()
        {
            stickLength = Vector3.Distance(point0.Position, point1.Position);
        }
        #endregion
        #region PRIVATE FUNC
        #endregion
    }
}
