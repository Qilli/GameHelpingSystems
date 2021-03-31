using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Gestures
{
    public abstract class GestureRecognizer : Base.ObjectsControl.BaseObject
    {
        public class Gesture
        {
            int gestureID;
        }

        public delegate void OnNewGesture(Gesture recognizedGesture);

        protected GesturePointsInput inputSource;
        protected Gesture lastGesture = null;
        protected OnNewGesture newGestureDetected;

        protected virtual void startCollectingInput()
        {
            inputSource.startRecording();
        }
        protected virtual void endCollectingInput()
        {
            inputSource.endRecording();
        }
        protected virtual bool isCollecting()
        {
            return inputSource.isRecording();
        }

        public void registerListener(OnNewGesture onNew)
        {
            newGestureDetected += onNew;
        }

        public void unregisterListener(OnNewGesture onNew)
        {
            newGestureDetected -= onNew;
        }

        public override void onUpdate(float delta)
        {
            if(isCollecting())
            {
                inputSource.onUpdate(delta);
                if(endRecording())
                {
                    endCollectingInput();
                    lastGesture=recognizeGesture();
                    newGestureDetected(lastGesture);
                    inputSource.resetRecording();
                }
            }
            else
            {
                if(startRecording())
                {
                    startCollectingInput();
                }
            }
        }

        protected abstract bool startRecording();
        protected abstract bool endRecording();
        protected abstract Gesture recognizeGesture();
    }
}
