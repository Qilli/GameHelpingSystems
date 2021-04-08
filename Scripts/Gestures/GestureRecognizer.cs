using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
namespace Base.Gestures
{
    public abstract class GestureRecognizer : Base.ObjectsControl.BaseObject
    {

        public class GestureResult
        {
            public string gestureName;
            public float probability;
        }

        public delegate void OnNewGesture(GestureResult recognizedGesture);

        protected GesturePointsInput inputSource = null;
        protected GestureResult lastGesture = null;
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

        protected virtual void onNewPoint(Point point)
        {

        }

        public void setInputSource(GesturePointsInput inputSource_)
        {
            inputSource = inputSource_;
        }

        public void registerListener_OnNewGesture(OnNewGesture onNew)
        {
            newGestureDetected += onNew;
        }

        public void unregisterListener_OnNewGesture(OnNewGesture onNew)
        {
            newGestureDetected -= onNew;
        }

        public override void onUpdate(float delta)
        {
            if (isCollecting())
            {
                if(inputSource.onUpdate(delta))
                {
                    onNewPoint(inputSource.LastAddedPoint);
                }

                if (endRecording())
                {
                    endCollectingInput();
                    lastGesture = recognizeGesture();
                    if(newGestureDetected!=null)newGestureDetected(lastGesture);
                    inputSource.resetRecording();
                }
            }
            else
            {
                if (startRecording())
                {
                    startCollectingInput();
                }
            }
        }

        protected abstract bool startRecording();
        protected abstract bool endRecording();
        protected abstract GestureResult recognizeGesture();
    }
}
