using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GESTURES
using PDollarGestureRecognizer;
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

        private List<Point> gesturePoints= new List<Point>();
        private State gestureState = State.EMPTY;
        private Point lastAddedPoint = new Point(0,0,0);

        public Point LastAddedPoint
        {
            get { return lastAddedPoint; }
        }

        public List<Point> getPoints(bool resetState_=false) { if (resetState_) resetState(); return gesturePoints; }
        public void resetState() { setState(State.EMPTY); gesturePoints.Clear(); }
        public void setState(GesturePoints.State newState) { gestureState = newState; }
        public void setStateAsReady() { setState(State.END);}
        public bool startCollecting() { if (gestureState != State.EMPTY) return false; gesturePoints.Clear(); setState(State.COLLECTING);return true; }
        public State getState() { return gestureState; }

        //points manipulation
        public void addNewPoint(Point point) {
            lastAddedPoint = point;
            gesturePoints.Add(point);
        }
        public void addNewPoints(Point[] points) { gesturePoints.AddRange(points); }
    }

}
#endif