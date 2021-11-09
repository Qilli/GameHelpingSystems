using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Physics.Verlet
{
    [System.Serializable]
    public class VerletGroupController : Base.ObjectsControl.BaseObject
    {
        #region CLASSES
        [System.Serializable]
        public class VerletEditorPrefs
        {
            [Header("Defaults")]
            public Vector3 defaultNewPointPos = -Vector3.up;
            public Vector3 defaultNewPointPrevPos = Vector3.zero;
            public Color spheresColor=Color.blue;
            public float sphereSize=0.2f;
            public Vector3 snap;
        }
        #endregion

        #region PUBLIC PARAMS
        [Header("Editor Prefs")]
        public VerletEditorPrefs editorPrefs;
        [Header("Simulation settings")]
        public bool simulationActive = true;
        #endregion
        #region PRIVATE PARAMS
        [SerializeField]
        private List<VerletPoint> points = new List<VerletPoint>();
        #endregion

        #region PUBLIC FUNC
        public int PointsCount { get => points.Count; }
        public VerletPoint getAtIndex(int index)
        {
            Debug.Assert(index >= 0 && index < PointsCount, "Wrong point index");
            return points[index];
        }
        public bool isIndexValid(int index)
        {
            return index >= 0 && index < points.Count;
        }
        public int addNewPoint()
        {
            VerletPoint p = new VerletPoint();
            p.Position = editorPrefs.defaultNewPointPos;
            p.PrevPosition = editorPrefs.defaultNewPointPrevPos;
            p.IsLocked = false;
            points.Add(p);
            return points.Count - 1;
        }

        public void removePointAt(int selectedPoint)
        {
           if(isIndexValid(selectedPoint))
            {
                points.RemoveAt(selectedPoint);
            }
        }

        public override void onFixedUpdate(float fixedDelta)
        {
            base.onFixedUpdate(fixedDelta);
            if(simulationActive)
            {
                updatePoints();
            }
        }

        #endregion
        #region PRIVATE FUNC
        private void updatePoints()
        {
            float vx = 0;
            float vy = 0;
            float vz = 0;
            VerletPoint tempPoint = null;
            for(int a=0;a<points.Count;++a)
            {
                tempPoint = points[a];
                vx = tempPoint.Position.x- tempPoint.PrevPosition.x;
                vy = tempPoint.Position.y - tempPoint.PrevPosition.y;
                vz = tempPoint.Position.z - tempPoint.PrevPosition.z;
                tempPoint.movePointBy(vx, vy, vz);
                tempPoint.PrevPosition = tempPoint.Position;
            }
        }
        #endregion
    }
}
