using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        public List<Vector3> getPoints() { return gesturePoints.getPoints(); }

        public virtual void resetRecording()
        {
            gesturePoints.resetState();
        }

        public virtual bool isRecording()
        {
            return gesturePoints.getState() == GesturePoints.State.COLLECTING;
        }

        public virtual void onUpdate(float delta)
        {
            GesturePoints.State currentState = gesturePoints.getState();
            if (currentState == GesturePoints.State.EMPTY)
            {
                //no gesture recognizing on, return
                return;
            }
            else if (currentState == GesturePoints.State.COLLECTING)
            {
                if (recordType == InputRecordingType.EVERY_FRAME)
                {
                    gesturePoints.addNewPoint(getNewPoint());
                }
                else
                {
                    timer += delta;
                    if (delta > timeSpan)
                    {
                        delta = 0;
                        gesturePoints.addNewPoint(getNewPoint());
                    }
                }
            }
            else if (currentState == GesturePoints.State.PAUSED)
            {
                return;
            }
            else if (currentState == GesturePoints.State.END)
            {
                return;
            }

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
        public abstract Vector3 getNewPoint();
    }
}
