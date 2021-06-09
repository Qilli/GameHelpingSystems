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
            BOOL,
            VECTOR
        }
        public string name;
        public string typeName;
        public SharedType type;

        public virtual SharedVariable getCopy()
        {
            return null;
        }
    }
    [System.Serializable]
    public class SharedVariable<T> : SharedVariable
    {
        public T value;
        public override SharedVariable getCopy()
        {
            return new SharedVariable<T>() { name = this.name, type = this.type, value = this.value };
        }
    }
    [System.Serializable]
    public class SharedTransform: SharedVariable<Transform>
    {
        public SharedTransform()
        {
            type = SharedVariable<Transform>.SharedType.TRANSFORM;
            typeName = "Transform";
        }
        public override SharedVariable getCopy()
        {
            return new SharedTransform () { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedInt() { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedString() { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedGameObject() { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedObject() { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedFloat() { name = this.name, type = this.type, value = this.value };
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
        public override SharedVariable getCopy()
        {
            return new SharedBool() { name = this.name, type = this.type, value = this.value };
        }
    }
    [System.Serializable]
    public class SharedVector : SharedVariable<Vector3>
    {
        public SharedVector()
        {
            type = SharedVariable<int>.SharedType.VECTOR;
            typeName = "Vector";
        }
        public override SharedVariable getCopy()
        {
            return new SharedVector() { name = this.name, type = this.type, value = this.value };
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
