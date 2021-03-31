//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;

namespace Base.CommonCode
{

    public class Recorder : MonoBehaviour
    {
        public AnimationClip clip;

        private GameObjectRecorder m_Recorder;

        void Start()
        {
            // Create recorder and record the script GameObject.
            m_Recorder = new GameObjectRecorder(gameObject);
            // Bind all the Transforms on the GameObject and all its children.
            m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        }

        void LateUpdate()
        {
            if (clip == null)
                return;

            // Take a snapshot and record all the bindings values for this frame.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }

        void OnDisable()
        {
            if (clip == null)
                return;

            if (m_Recorder.isRecording)
            {
                // Save the recorded session to the clip.
                m_Recorder.SaveToClip(clip);
            }
        }
    }
#else
public class Recorder : MonoBehaviour
{
}
#endif


}
