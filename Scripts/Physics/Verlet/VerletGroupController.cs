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
            public Vector3 snap;
            [Header("Points")]
            public Vector3 defaultNewPointPos = -Vector3.up;
            public Vector3 defaultNewPointPrevPos = Vector3.zero;
            public Color spheresColor=Color.blue;
            public float sphereSize=0.2f;
            public Color selectedSphereColor = Color.red;        
            [Header("Sticks")]
            public Color stickColor = Color.green;
            public Color activeStickColor = Color.yellow;
            public float stickSize = 0.2f;
        }
        #endregion

        #region PUBLIC PARAMS
        [Header("Editor Prefs")]
        public VerletEditorPrefs editorPrefs;
        [Header("Simulation settings")]
        public bool simulationActive = true;
        public float localGravityForce = 0.5f;
        public float localFrictionForce = 0.99f;
        #endregion
        #region PRIVATE PARAMS
        [SerializeReference]
        private List<VerletPoint> points = new List<VerletPoint>();
        [SerializeReference]
        private List<VerletStick> sticks = new List<VerletStick>();
        #endregion

        #region PUBLIC FUNC
        public int PointsCount { get => points.Count; }
        public int SticksCount { get => sticks.Count; }
        public VerletPoint getPointAtIndex(int index)
        {
            Debug.Assert(index >= 0 && index < PointsCount, "Wrong point index");
            return points[index];
        }
        public bool isPointIndexValid(int index)
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

        public bool isStickWithPoint(int stickIndex,int selectedPoint)
        {
            VerletStick tempStick = getStickAt(stickIndex);
            return tempStick.hasPoint(getPointAtIndex(selectedPoint));
        }

        public void removePointAt(int selectedPoint)
        {
           if(isPointIndexValid(selectedPoint))
            {
                points.RemoveAt(selectedPoint);
                removeSticksWith(selectedPoint);
            }
        }

        public bool isStickIndexValid(int index)
        {
            return index >= 0 && index < sticks.Count;
        }
        public void addNewStick(VerletPoint p0, VerletPoint p1)
        {
            VerletStick newStick = new VerletStick();
            newStick.setStick(p0, p1);
            sticks.Add(newStick);
        }
        public void removeStickAt(int index)
        {
            if(isStickIndexValid(index))
            {
                sticks.RemoveAt(index);
            }
        }
        public VerletStick getStickAt(int index)
        {
            Debug.Assert(index >= 0 && index < SticksCount, "Wrong stick index");
            return sticks[index];
        }

        public override void onFixedUpdate(float fixedDelta)
        {
            base.onFixedUpdate(fixedDelta);

            if(simulationActive)
            {
                updatePoints();
                for (int a = 0; a < 5; ++a) updateSticks();
            }
        }
        public void addNewStick(int stickPoint0, int stickPoint1)
        {
            VerletPoint point0 = getPointAtIndex(stickPoint0);
            VerletPoint point1 = getPointAtIndex(stickPoint1);
            if(point0!=null&&point1!=null)
            {
                addNewStick(point0, point1);
            }
        }

        public bool stickForPointsExist(int point0, int point1)
        {
            int index=tryFindStickForPointIndexes(point0, point1);
            return index!=-1 ? true : false;
        }

        public void removeStick(int point0, int point1)
        {
            int index = tryFindStickForPointIndexes(point0, point1);
            if (index != -1) sticks.RemoveAt(index);
        }
        public void recalculateStickLengths()
        {
            sticks.ForEach(stick => stick.calculateLength());
        }

        #endregion
        #region PRIVATE FUNC
        private int tryFindStickForPointIndexes(int point0, int point1)
        {
            VerletPoint p0 = getPointAtIndex(point0);
            VerletPoint p1 = getPointAtIndex(point1);
            int index = sticks.FindIndex((stick) => stick.hasPoint(p0) && stick.hasPoint(p1));
            return index;
        }
        private void updateSticks()
        {
            VerletStick tempStick = null;
            for(int a=0;a<SticksCount;++a)
            {
                tempStick = sticks[a];
                tempStick.constrainStick();
            }
        }

        private void removeSticksWith(int selectedPoint)
        {
            VerletPoint p = getPointAtIndex(selectedPoint);
            sticks.RemoveAll((stick) => stick.hasPoint(p));
        }
        private void updatePoints()
        {
            float vx = 0;
            float vy = 0;
            float vz = 0;
            VerletPoint tempPoint = null;
            for(int a=0;a<points.Count;++a)
            {
                tempPoint = points[a];
                if (!tempPoint.IsLocked)
                {
                    vx = tempPoint.Position.x - tempPoint.PrevPosition.x;
                    vy = tempPoint.Position.y - tempPoint.PrevPosition.y;
                    vz = tempPoint.Position.z - tempPoint.PrevPosition.z;
                    tempPoint.PrevPosition = tempPoint.Position;
                    float mul = localFrictionForce;
                    tempPoint.movePointBy(vx * mul, (vy - localGravityForce) * mul, vz * mul);
                }
            }
        }
        #endregion
    }
}
