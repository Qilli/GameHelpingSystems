using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Data
{
    public interface IObjectSaveLoadCreateControl
    {
        public bool createObject();
        public bool saveObject();
        public bool loadObject();
        public void setPath(string path);
        public bool isPathSet();
        public System.Object getObject();
        public bool isObjectCreated();
    }
}
