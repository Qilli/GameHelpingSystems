using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [System.Serializable]
    public abstract class SharedVariable
    {
        [System.Serializable]
        public enum SharedType
        {
            GAMEOBJECT,
            OBJECT,
            TRANSFORM,
            FLOAT,
            INT,
            STRING,
            BOOL
        }
        public string name;
        public string typeName;
        public SharedType type;
    }
    [System.Serializable]
    public class SharedVariable<T> : SharedVariable
    {
        public T value;
    }
    [System.Serializable]
    public class SharedTransform: SharedVariable<Transform>
    {
        public SharedTransform()
        {
            type = SharedVariable<Transform>.SharedType.TRANSFORM;
            typeName = "Transform";
        }
    }
    [System.Serializable]
    public class SharedInt : SharedVariable<int>
    {
        public SharedInt()
        {
            type = SharedVariable<int>.SharedType.INT;
            typeName = "Integer";
        }
    }
    [System.Serializable]
    public class SharedString : SharedVariable<string>
    {
        public SharedString()
        {
            type = SharedVariable<int>.SharedType.STRING;
            typeName = "String";
        }
    }
    [System.Serializable]
    public class SharedGameObject : SharedVariable<GameObject>
    {
        public SharedGameObject()
        {
            type = SharedVariable<int>.SharedType.GAMEOBJECT;
            typeName = "GameObject";
        }
    }
    [System.Serializable]
    public class SharedObject : SharedVariable<Object>
    {
        public SharedObject()
        {
            type = SharedVariable<int>.SharedType.OBJECT;
            typeName = "Object";
        }
    }
    [System.Serializable]
    public class SharedFloat : SharedVariable<float>
    {
        public SharedFloat()
        {
            type = SharedVariable<int>.SharedType.FLOAT; 
            typeName = "Float";
        }
    }
    [System.Serializable]
    public class SharedBool : SharedVariable<bool>
    {
        public SharedBool()
        {
            type = SharedVariable<int>.SharedType.BOOL;
            typeName = "Bool";
        }
    }

    [System.Serializable]
    public class EditorSharedVariable
    {
        public string name;
        public SharedVariable.SharedType type;
        public EditorSharedVariable()
        {
            name = "";
            type = SharedVariable.SharedType.GAMEOBJECT;
        }

        public EditorSharedVariable(string name_,SharedVariable.SharedType type_)
        {
            name = name_;
            type = type_;
        }
    }
}
