using System;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Curves
{
    [System.Serializable]
    public class CurvedPath
    {
        #region PUBLIC PARAMS

        #endregion
        #region PRIVATE PARAMS
        [SerializeField]
        private List<Vector3> pathPoints = new List<Vector3>();
        [SerializeField]
        public bool ClosedPath { get; private set; }
        #endregion
        #region PUBLIC FUNC
        public CurvedPath()
        {
            initCurve(new Vector3[]
            {
                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(-1,0,1),
                new Vector3(0,0,1)
            });
        }

        internal void matchTangents()
        {
            for (int i = 3; i < pathPoints.Count - 1; i += 3)
            {
                Vector3 pointAnchor = pathPoints[i];
                Vector3 pointTangent1 = pathPoints[i - 1];
                pathPoints[i + 1] = pointAnchor * 2.0f - pointTangent1;
            }
        }

        public CurvedPath(Vector3 center)
        {
            initCurve(new Vector3[]
           {
                center-Vector3.forward*5,
                center - Vector3.forward*5 - Vector3.right *5,
                center + Vector3.forward*5 ,
                center +Vector3.forward + Vector3.right*5
           });
        }

        public void addSegment(Vector3 anchorPoint)
        {
            Vector3 tangent = pathPoints[pathPoints.Count - 1] * 2.0f - pathPoints[pathPoints.Count - 2];
            pathPoints.Add(tangent);
            pathPoints.Add((anchorPoint + tangent) * 0.5f);
            pathPoints.Add(anchorPoint);
        }
        public Vector3 getPointAt(int index)
        {
            return pathPoints[index];
        }
        public Vector3 getLastPoint() => pathPoints[pathPoints.Count - 1];
        public void setPointAt(int index, in Vector3 value)
        {
            pathPoints[index] = value;
        }
        public void setTangetAndMatch(int i, Vector3 newPos)
        {
            //compute tangent index
            int index = i % 3;
            if (index == 0) return;//anchor point index
            if ((i - 1) % 3 == 0)
            {
                //right tangent
                setPointAt(i, newPos);
                //left tangent
                Vector3 newTangent = pathPoints[i - 1] * 2 - newPos;
                if (i > 1 && !ClosedPath)//start anchor has only one 
                {
                    setPointAt(i - 2, newTangent);
                }
                else if (i == 1 && ClosedPath)//start element but path is closed
                {
                    setPointAt(pathPoints.Count - 1, newTangent);
                }
            }
            else
            {
                //right tangent
                setPointAt(i, newPos);
                Vector3 newTangent;
                if (i == pathPoints.Count - 2 && ClosedPath)//last element in the list
                {
                    newTangent = pathPoints[0] * 2 - newPos;
                    setPointAt(1, newTangent);
                }
                else
                {
                    newTangent = pathPoints[i + 1] * 2 - newPos;
                    setPointAt(i + 2, newTangent);
                }
            }
        }
        public void setClosePath(bool isClosed)
        {
            if(isClosed)
            {
                ClosedPath = true;
                setPointAt(pathPoints.Count - 1, pathPoints[0]);
                //set tangent
                setTangetAndMatch(pathPoints.Count - 2, pathPoints[pathPoints.Count - 2]);
            }
            else
            {
                ClosedPath = false;
            }
        }
        public bool isIndexValid(int id)
        {
            return id >= 0 && id < pathPoints.Count;
        }
        public void moveTangentFor(int i, Vector3 offset)
        {
            if (i > 0 && i!= pathPoints.Count-1)
            {
                pathPoints[i - 1] += offset;
                pathPoints[i + 1] += offset;
            }
            else if(i==0)
            {
                pathPoints[1] += offset;
                if (ClosedPath)
                {
                    pathPoints[pathPoints.Count - 1] += offset;
                }
            }
            else
            {
                //for one side end
                pathPoints[i - 1] += offset;
                if (ClosedPath)
                {
                    pathPoints[1] += offset;
                }
            }
        }
        public int getPointsCount() => pathPoints.Count;
        #endregion
        #region PRIVATE FUNC
        private void initCurve(Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                pathPoints.Add(points[i]);
            }
        }
        #endregion

    }
}
