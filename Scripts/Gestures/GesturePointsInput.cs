using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GESTURES
using PDollarGestureRecognizer;
namespace Base.Gestures
{
    public abstract class GesturePointsInput
    {
        public enum InputRecordingType
        {
            EVERY_FRAME,
            TIME_SPAN
        }

        protected GesturePoints gesturePoints = new GesturePoints();

        [Header("Input base parameters")]
        public InputRecordingType recordType = InputRecordingType.EVERY_FRAME;
        public float timeSpan = 0.0f;
        private float timer = 0;

        public Point LastAddedPoint
        {
            get { return gesturePoints.LastAddedPoint; }
        }


        public List<Point> getPoints() { return gesturePoints.getPoints(); }

        public virtual void resetRecording()
        {
            gesturePoints.resetState();
        }

        public virtual bool isRecording()
        {
            return gesturePoints.getState() == GesturePoints.State.COLLECTING;
        }

        public virtual bool onUpdate(float delta)
        {
            GesturePoints.State currentState = gesturePoints.getState();
            if (currentState == GesturePoints.State.EMPTY)
            {
                //no gesture recognizing on, return
                return false;
            }
            else if (currentState == GesturePoints.State.COLLECTING)
            {
                if (recordType == InputRecordingType.EVERY_FRAME)
                {
                    gesturePoints.addNewPoint(getNewPoint());
                    return true;
                }
                else
                {
                    timer += delta;
                    if (delta > timeSpan)
                    {
                        delta = 0;
                        gesturePoints.addNewPoint(getNewPoint());
                        return true;
                    }return false;
                }
            }
            else if (currentState == GesturePoints.State.PAUSED)
            {
                return false;
            }
            else if (currentState == GesturePoints.State.END)
            {
                return false;
            }
            return false;
        }

        public virtual void startRecording()
        {
            gesturePoints.resetState();
            gesturePoints.setState(GesturePoints.State.COLLECTING);
        }
        public virtual void endRecording()
        {
            gesturePoints.setStateAsReady();
        }
        public abstract Point getNewPoint();
    }
}
#endif