using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Gestures
{

    public class GesturePoints
    {
        public enum State
        {
            COLLECTING,
            END,
            EMPTY,
            PAUSED
        }

        private List<Vector3> gesturePoints= new List<Vector3>();
        private State gestureState = State.EMPTY;

        public List<Vector3> getPoints(bool resetState_=false) { if (resetState_) resetState(); return gesturePoints; }
        public void resetState() { setState(State.EMPTY); gesturePoints.Clear(); }
        public void setState(GesturePoints.State newState) {}
        public void setStateAsReady() { setState(State.END);}
        public bool startCollecting() { if (gestureState != State.EMPTY) return false; gesturePoints.Clear(); setState(State.COLLECTING);return true; }
        public State getState() { return gestureState; }

        //points manipulation
        public void addNewPoint(Vector3 point) { gesturePoints.Add(point); }
        public void addNewPoints(Vector3[] points) { gesturePoints.AddRange(points); }
    }

}
