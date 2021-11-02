
using UnityEngine;
using UnityEditor;
namespace Base.Editor
{
    public class AudioEditorHelper : MonoBehaviour
    {
        [MenuItem("Assets/AudioHelper/CreateAudioFile")]
        public static void Init(MenuCommand command)
        {
            string savePath = getCurrentPath();
            //create
            AudioHelperParamsWindow w = AudioHelperParamsWindow.CreateAudioHelperParamsWindow();
            w.CurrentEditorPath = savePath;
            w.Show();
        }

        private static string getCurrentPath()
        {
            string path = null;
            if (Selection.assetGUIDs.Length == 0)
                path = "Assets/Gameplay/Data/Audio/Sounds";
            else
                path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            return path;
        }
    }
}
