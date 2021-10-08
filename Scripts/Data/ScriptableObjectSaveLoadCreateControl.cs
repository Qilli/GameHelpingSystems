using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Base.Data
{
    public class ScriptableObjectSaveLoadCreateControl<T> : IObjectSaveLoadCreateControl where T: ScriptableObject
    {
        private ScriptableObject so;
        private string path = "";
        public bool createObject()
        {
            so = ScriptableObject.CreateInstance<T>();
            return so != null;
        }

        public object getObject()
        {
            return so;
        }

        public bool isObjectCreated()
        {
            return so != null;
        }

        public bool isPathSet()
        {
            return !string.IsNullOrEmpty(path);
        }

        public bool loadObject()
        {
            if(isPathSet())
            {
                so = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                return so != null;
            }
            return false;
        }

        public bool saveObject()
        {
            if (so!=null)
            {
                if (!isPathSet()) return false;
                if(!isAsssetInDatabase())AssetDatabase.CreateAsset(so, path);
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return true;
            }return false;
        }

        public void setPath(string path)
        {
            this.path = path;
        }

        private bool isAsssetInDatabase()
        {
            var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            return asset != null;
        }
    }
}
