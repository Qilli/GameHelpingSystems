using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Audio;
namespace Base.Editor
{
    public class AudioHelperParamsWindow : EditorWindow
    {
        public string AudioNameEditor
        { get; set; } = "NewAudioObject";
        public string CurrentEditorPath { set; get; }
        [SerializeField]
        private AudioMixerGroup selectedGroup;
        [SerializeField]
        private AudioClip selectedClip;
        public static AudioHelperParamsWindow CreateAudioHelperParamsWindow()
        {
            AudioHelperParamsWindow window = ScriptableObject.CreateInstance(typeof(AudioHelperParamsWindow)) as AudioHelperParamsWindow;
            return window;
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Choose audio name: ");
            AudioNameEditor = EditorGUILayout.TextField(AudioNameEditor);
            selectedGroup= (AudioMixerGroup)EditorGUILayout.ObjectField(selectedGroup, typeof(AudioMixerGroup),false);
            selectedClip = (AudioClip)EditorGUILayout.ObjectField(selectedClip, typeof(AudioClip), false);
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Create"))
            {
                createAudioObject();
            }
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void createAudioObject()
        {    
            AssetDatabase.CreateFolder(CurrentEditorPath, AudioNameEditor);
            //create objects
            string fullPath = CurrentEditorPath + @"\" + AudioNameEditor+@"\";
            Audio.SingleAudioPlayer audio = ScriptableObject.CreateInstance<Audio.SingleAudioPlayer>();
            audio.clip = selectedClip;
            AssetDatabase.CreateAsset(audio, fullPath+AudioNameEditor+".asset");
            Undo.RegisterCreatedObjectUndo(audio, "Created audio object");
            //create settings
            Audio.SoundObjectSettings audioSettings = ScriptableObject.CreateInstance<Audio.SoundObjectSettings>();
            AssetDatabase.CreateAsset(audioSettings, fullPath+AudioNameEditor+"_setttings.asset");
            audio.settings = audioSettings;
            audioSettings.mixer = selectedGroup;
            Undo.RegisterCreatedObjectUndo(audioSettings, "Created audio settings object");
            EditorUtility.SetDirty(audio);
            EditorUtility.SetDirty(audioSettings);
            this.Close();
        }
    }
}
